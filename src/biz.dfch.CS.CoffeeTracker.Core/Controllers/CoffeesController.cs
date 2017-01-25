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
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using System.Web.Http.OData;
using System.Web.Http.OData.Routing;
using biz.dfch.CS.CoffeeTracker.Core.DbContext;
using biz.dfch.CS.CoffeeTracker.Core.Logging;
using biz.dfch.CS.CoffeeTracker.Core.Model;
using biz.dfch.CS.Commons.Diagnostics;

namespace biz.dfch.CS.CoffeeTracker.Core.Controllers
{
    /*
    The WebApiConfig class may require additional changes to add a route for this controller. Merge these statements into the Register method of the WebApiConfig class as applicable. Note that OData URLs are case sensitive.

    using System.Web.Http.OData.Builder;
    using System.Web.Http.OData.Extensions;
    using biz.dfch.CS.CoffeeTracker.Core.Model;
    ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
    builder.EntitySet<Coffee>("Coffees");
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class CoffeesController : ODataController
    {
        private readonly CoffeeTrackerDbContext db = new CoffeeTrackerDbContext();
        private const string MODELNAME = ControllerLogging.ModelNames.COFFEE;

        // GET: odata/Coffees
        [EnableQuery]
        public IQueryable<Coffee> GetCoffees()
        {
            ControllerLogging.LogGetEntities(MODELNAME);

            return db.Coffees;
        }

        // GET: odata/Coffees(5)
        [EnableQuery]
        public SingleResult<Coffee> GetCoffee([FromODataUri] long key)
        {
            ControllerLogging.LogGetEntity(MODELNAME, key.ToString());

            return SingleResult.Create(db.Coffees.Where(coffee => coffee.Id == key));
        }

        // PUT: odata/Coffees(5)
        public async Task<IHttpActionResult> Put([FromODataUri] long key, Delta<Coffee> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var coffee = await db.Coffees.FindAsync(key);
            if (coffee == null)
            {
                return NotFound();
            }

            ControllerLogging.LogUpdateEntityStartPut(MODELNAME, key.ToString());
            
            patch.Put(coffee);
            await db.SaveChangesAsync();

            ControllerLogging.LogUpdateEntityStopPut(MODELNAME, coffee);

            return Updated(coffee);
        }

        // POST: odata/Coffees
        public async Task<IHttpActionResult> Post(Coffee coffee)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ControllerLogging.LogInsertEntityStart(MODELNAME, coffee);

            db.Coffees.Add(coffee);
            await db.SaveChangesAsync();

            ControllerLogging.LogInsertEntityStop(MODELNAME, coffee);

            return Created(coffee);
        }

        // PATCH: odata/Coffees(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IHttpActionResult> Patch([FromODataUri] long key, Delta<Coffee> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var coffee = await db.Coffees.FindAsync(key);
            if (coffee == null)
            {
                return NotFound();
            }
            
            ControllerLogging.LogUpdateEntityStartPatch(MODELNAME, key.ToString());

            patch.Patch(coffee);

            await db.SaveChangesAsync();
            coffee = await db.Coffees.FindAsync(key);
            Contract.Assert(null != coffee);

            ControllerLogging.LogUpdateEntityStopPatch(MODELNAME, coffee);

            return Updated(coffee);
        }

        // DELETE: odata/Coffees(5)
        public async Task<IHttpActionResult> Delete([FromODataUri] long key)
        {
            var coffee = await db.Coffees.FindAsync(key);
            if (coffee == null)
            {
                return NotFound();
            }

            ControllerLogging.LogDeleteEntityStart(MODELNAME, coffee);
            
            db.Coffees.Remove(coffee);
            await db.SaveChangesAsync();

            ControllerLogging.LogDeleteEntityStop(MODELNAME, coffee);

            return StatusCode(HttpStatusCode.NoContent);
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
