using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
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
using biz.dfch.CS.CoffeeTracker.Core.Model;

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
    builder.EntitySet<User>("Users"); 
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class CoffeeOrdersController : ODataController
    {
        private readonly CoffeeTrackerDbContext db = new CoffeeTrackerDbContext();

        // GET: odata/CoffeeOrders
        [EnableQuery]
        public IQueryable<CoffeeOrder> GetCoffeeOrders()
        {
            return db.CoffeeOrders;
        }

        // GET: odata/CoffeeOrders(5)
        [EnableQuery]
        public SingleResult<CoffeeOrder> GetCoffeeOrder([FromODataUri] long key)
        {
            return SingleResult.Create(db.CoffeeOrders.Where(coffeeOrder => coffeeOrder.Id == key));
        }

        // PUT: odata/CoffeeOrders(5)
        public async Task<IHttpActionResult> Put([FromODataUri] long key, Delta<CoffeeOrder> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            CoffeeOrder coffeeOrder = await db.CoffeeOrders.FindAsync(key);
            if (coffeeOrder == null)
            {
                return NotFound();
            }

            patch.Put(coffeeOrder);

            await db.SaveChangesAsync();

            return Updated(coffeeOrder);
        }

        // POST: odata/CoffeeOrders
        public async Task<IHttpActionResult> Post(CoffeeOrder coffeeOrder)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.CoffeeOrders.Add(coffeeOrder);
            await db.SaveChangesAsync();

            return Created(coffeeOrder);
        }

        // PATCH: odata/CoffeeOrders(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IHttpActionResult> Patch([FromODataUri] long key, Delta<CoffeeOrder> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            CoffeeOrder coffeeOrder = await db.CoffeeOrders.FindAsync(key);
            if (coffeeOrder == null)
            {
                return NotFound();
            }

            patch.Patch(coffeeOrder);

            await db.SaveChangesAsync();

            return Updated(coffeeOrder);
        }

        // DELETE: odata/CoffeeOrders(5)
        public async Task<IHttpActionResult> Delete([FromODataUri] long key)
        {
            CoffeeOrder coffeeOrder = await db.CoffeeOrders.FindAsync(key);
            if (coffeeOrder == null)
            {
                return NotFound();
            }

            db.CoffeeOrders.Remove(coffeeOrder);
            await db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: odata/CoffeeOrders(5)/Coffee
        [EnableQuery]
        public SingleResult<Coffee> GetCoffee([FromODataUri] long key)
        {
            return SingleResult.Create(db.CoffeeOrders.Where(m => m.Id == key).Select(m => m.Coffee));
        }

        // GET: odata/CoffeeOrders(5)/User
        [EnableQuery]
        public SingleResult<User> GetUser([FromODataUri] long key)
        {
            return SingleResult.Create(db.CoffeeOrders.Where(m => m.Id == key).Select(m => m.User));
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
