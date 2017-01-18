using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
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
    builder.EntitySet<Coffee>("Coffees");
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class CoffeesController : ODataController
    {
        private CoffeeTrackerDbContext db = new CoffeeTrackerDbContext();

        // GET: odata/Coffees
        [EnableQuery]
        public IQueryable<Coffee> GetCoffees()
        {
            return db.Statistics;
        }

        // GET: odata/Coffees(5)
        [EnableQuery]
        public SingleResult<Coffee> GetCoffee([FromODataUri] long key)
        {
            return SingleResult.Create(db.Statistics.Where(coffee => coffee.Id == key));
        }

        // PUT: odata/Coffees(5)
        public async Task<IHttpActionResult> Put([FromODataUri] long key, Delta<Coffee> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Coffee coffee = await db.Statistics.FindAsync(key);
            if (coffee == null)
            {
                return NotFound();
            }

            patch.Put(coffee);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CoffeeExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(coffee);
        }

        // POST: odata/Coffees
        public async Task<IHttpActionResult> Post(Coffee coffee)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Statistics.Add(coffee);
            await db.SaveChangesAsync();

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

            Coffee coffee = await db.Statistics.FindAsync(key);
            if (coffee == null)
            {
                return NotFound();
            }

            patch.Patch(coffee);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CoffeeExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(coffee);
        }

        // DELETE: odata/Coffees(5)
        public async Task<IHttpActionResult> Delete([FromODataUri] long key)
        {
            Coffee coffee = await db.Statistics.FindAsync(key);
            if (coffee == null)
            {
                return NotFound();
            }

            db.Statistics.Remove(coffee);
            await db.SaveChangesAsync();

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

        private bool CoffeeExists(long key)
        {
            return db.Statistics.Count(e => e.Id == key) > 0;
        }
    }
}
