using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
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
        private CoffeeTrackerDbContext db = new CoffeeTrackerDbContext();

        // GET: odata/Coffees
        [EnableQuery]
        public IQueryable<Coffee> GetCoffees()
        {
            return db.Coffees;
        }

        // GET: odata/Coffees(5)
        [EnableQuery]
        public SingleResult<Coffee> GetCoffee([FromODataUri] long key)
        {
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

            Coffee coffee = await db.Coffees.FindAsync(key);
            if (coffee == null)
            {
                return NotFound();
            }

            Logger.Get(Logging.TraceSourceName.COFFEES_CONTROLLER)
                .TraceEvent(TraceEventType.Information, (int)Logging.EventId.Default, 
                "Start update coffee with Id {0} values \n Name: {1} \n Brand: {2} \n LastDelivery {3} \n Price: {4} \n Stock: {5}", coffee.Id, coffee.Name, coffee.Brand, coffee.LastDelivery, coffee.Price, coffee.Stock);

            patch.Put(coffee);

            await db.SaveChangesAsync();

            return Updated(coffee);
        }

        // POST: odata/Coffees
        public async Task<IHttpActionResult> Post(Coffee coffee)
        {
            Logger.Get(Logging.TraceSourceName.COFFEE_TRACKER_CORE)
                .TraceEvent(TraceEventType.Start, (int)Logging.EventId.Start, "Start insert coffee \n Id: {0} \n Name: {1} \n Brand: {2}", coffee.Id, coffee.Name, coffee.Brand);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Coffees.Add(coffee);
            await db.SaveChangesAsync();

            Logger.Get(Logging.TraceSourceName.COFFEE_TRACKER_CORE)
                .TraceEvent(TraceEventType.Stop, (int)Logging.EventId.Stop, "Stop insert {0}-{1}-{2}", coffee.Id, coffee.Name, coffee.Brand);

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

            Coffee coffee = await db.Coffees.FindAsync(key);
            if (coffee == null)
            {
                return NotFound();
            }
            patch.GetChangedPropertyNames();
            Logger.Get(Logging.TraceSourceName.COFFEES_CONTROLLER)
                .TraceEvent(TraceEventType.Information, (int)Logging.EventId.Default, "Start update Id: {0}", coffee.Id);

            patch.Patch(coffee);

            await db.SaveChangesAsync();

            return Updated(coffee);
        }

        // DELETE: odata/Coffees(5)
        public async Task<IHttpActionResult> Delete([FromODataUri] long key)
        {
            Coffee coffee = await db.Coffees.FindAsync(key);
            if (coffee == null)
            {
                return NotFound();
            }

            Logger.Get(Logging.TraceSourceName.COFFEES_CONTROLLER)
                .TraceEvent(TraceEventType.Information, (int)Logging.EventId.Default, "Start delete with Id {0}", coffee.Id);

            db.Coffees.Remove(coffee);
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
    }
}
