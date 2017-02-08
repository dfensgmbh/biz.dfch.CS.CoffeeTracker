using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using System.Web.Http.OData;
using System.Web.Http.OData.Routing;
using biz.dfch.CS.CoffeeTracker.Core.DbContext;
using biz.dfch.CS.CoffeeTracker.Core.Model;
using biz.dfch.CS.Commons.Diagnostics;
using biz.dfch.CS.CoffeeTracker.Core.Logging;
using biz.dfch.CS.CoffeeTracker.Core.Managers;

namespace biz.dfch.CS.CoffeeTracker.Core.Controllers
{
    /*
    The WebApiConfig class may require additional changes to add a route for this controller. Merge these statements into the Register method of the WebApiConfig class as applicable. Note that OData URLs are case sensitive.

    using System.Web.Http.OData.Builder;
    using System.Web.Http.OData.Extensions;
    using biz.dfch.CS.CoffeeTracker.Core.Model;
    ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
    builder.EntitySet<CoffeeOrder>("CoffeeOrders");
    builder.EntitySet<Coffee>("Coffees"); 
    builder.EntitySet<ApplicationUser>("Users"); 
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    [Authorize]
    public class CoffeeOrdersController : ODataController
    {
        private const string MODELNAME = ControllerLogging.ModelNames.COFFEEORDER;
        private CoffeeOrdersManager coffeeOrdersManager;

        public CoffeeOrdersController()
        {
            coffeeOrdersManager = new CoffeeOrdersManager();
        }

        // GET: odata/CoffeeOrders
        [EnableQuery]
        public IQueryable<CoffeeOrder> GetCoffeeOrders()
        {
            ControllerLogging.LogGetEntities(MODELNAME);

            return coffeeOrdersManager.GetCoffeeOrdersOfCurrentUser(this).AsQueryable();
        }

        // GET: odata/CoffeeOrders(5)
        [EnableQuery]
        public SingleResult<CoffeeOrder> GetCoffeeOrder([FromODataUri] long key)
        {
            Contract.Requires(0 < key, "|404|");

            ControllerLogging.LogGetEntity(MODELNAME, key.ToString());

            return SingleResult.Create(db.CoffeeOrders.Where(coffeeOrder => coffeeOrder.Id == key));
        }

        // PUT: odata/CoffeeOrders(5)
        public async Task<IHttpActionResult> Put([FromODataUri] long key, CoffeeOrder modifiedCoffeeOrder)
        {
            Contract.Requires(0 < key, "|404|");
            Contract.Requires(null != modifiedCoffeeOrder, "|404|");

            Validate(modifiedCoffeeOrder);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var coffeeOrder = await db.CoffeeOrders.FindAsync(key);
            Contract.Assert(null != coffeeOrder, "|404|");

            ControllerLogging.LogUpdateEntityStartPut(MODELNAME, key.ToString());

            coffeeOrder.Created = modifiedCoffeeOrder.Created;
            coffeeOrder.UserId = modifiedCoffeeOrder.UserId;
            coffeeOrder.CoffeeId = modifiedCoffeeOrder.CoffeeId;

            await db.SaveChangesAsync();

            ControllerLogging.LogUpdateEntityStopPut(MODELNAME, coffeeOrder);

            return Updated(coffeeOrder);
        }

        // POST: odata/CoffeeOrders
        public async Task<IHttpActionResult> Post(CoffeeOrder coffeeOrder)
        {
            Contract.Requires(null != coffeeOrder, "|404|");

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ControllerLogging.LogInsertEntityStart(MODELNAME, coffeeOrder);

            db.CoffeeOrders.Add(coffeeOrder);
            await db.SaveChangesAsync();

            ControllerLogging.LogInsertEntityStop(MODELNAME, coffeeOrder);

            return Created(coffeeOrder);
        }

        // PATCH: odata/CoffeeOrders(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IHttpActionResult> Patch([FromODataUri] long key, Delta<CoffeeOrder> patch)
        {
            Contract.Requires(0 < key, "|404|");
            Contract.Requires(null != patch, "|404|");

            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var coffeeOrder = await db.CoffeeOrders.FindAsync(key);
            if (coffeeOrder == null)
            {
                return NotFound();
            }
          
            ControllerLogging.LogUpdateEntityStartPatch(MODELNAME, key.ToString());

            patch.Patch(coffeeOrder);

            await db.SaveChangesAsync();

            coffeeOrder = await db.CoffeeOrders.FindAsync(key);
            Contract.Assert(null != coffeeOrder);

            ControllerLogging.LogUpdateEntityStopPatch(MODELNAME, coffeeOrder);

            return Updated(coffeeOrder);
        }

        // DELETE: odata/CoffeeOrders(5)
        public async Task<IHttpActionResult> Delete([FromODataUri] long key)
        {
            Contract.Requires(0 < key, "|404|");

            var coffeeOrder = await db.CoffeeOrders.FindAsync(key);
            Contract.Assert(null != coffeeOrder, "|404|");

            ControllerLogging.LogDeleteEntityStart(MODELNAME, coffeeOrder);

            db.CoffeeOrders.Remove(coffeeOrder);
            await db.SaveChangesAsync();

            ControllerLogging.LogDeleteEntityStop(MODELNAME, coffeeOrder);

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: odata/CoffeeOrders(5)/Coffee
        [EnableQuery]
        public SingleResult<Coffee> GetCoffee([FromODataUri] long key)
        {
            Contract.Requires(0 < key, "|404|");

            ControllerLogging.LogGetEntity(ControllerLogging.ModelNames.COFFEE, key.ToString());

            return SingleResult.Create(db.CoffeeOrders.Where(m => m.Id == key).Select(m => m.Coffee));
        }

        // GET: odata/CoffeeOrders(5)/ApplicationUser
        [EnableQuery]
        public SingleResult<ApplicationUser> GetUser([FromODataUri] long key)
        {
            Contract.Requires(0 < key, "|404|");

            ControllerLogging.LogGetEntity(ControllerLogging.ModelNames.USER, key.ToString());

            return SingleResult.Create(db.CoffeeOrders.Where(m => m.Id == key).Select(m => m.ApplicationUser));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
