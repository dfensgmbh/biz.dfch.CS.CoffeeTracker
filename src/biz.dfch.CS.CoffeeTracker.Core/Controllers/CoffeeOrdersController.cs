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
    builder.EntitySet<CoffeeOrder>("CoffeeOrders");
    builder.EntitySet<Coffee>("Coffees"); 
    builder.EntitySet<User>("Users"); 
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class CoffeeOrdersController : ODataController
    {
        private readonly CoffeeTrackerDbContext db = new CoffeeTrackerDbContext();
        private const string MODELNAME = "CoffeeOrder";

        // GET: odata/CoffeeOrders
        [EnableQuery]
        public IQueryable<CoffeeOrder> GetCoffeeOrders()
        {
            Logger.Get(Logging.TraceSourceName.COFFEETRACKER_CONTROLLERS)
                .TraceEvent(TraceEventType.Information, (int)Logging.EventId.Default, "Get {0}s", MODELNAME);
            return db.CoffeeOrders;
        }

        // GET: odata/CoffeeOrders(5)
        [EnableQuery]
        public SingleResult<CoffeeOrder> GetCoffeeOrder([FromODataUri] long key)
        {
            Logger.Get(Logging.TraceSourceName.COFFEETRACKER_CONTROLLERS)
                .TraceEvent(TraceEventType.Information, (int)Logging.EventId.Default, "Get {0} with Id: {1}", MODELNAME, key);
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
            Logger.Get(Logging.TraceSourceName.COFFEETRACKER_CONTROLLERS)
                .TraceEvent(TraceEventType.Start, (int)Logging.EventId.Start, "Start Update of {0} with id {1}...", MODELNAME, key);

            patch.Put(coffeeOrder);

            await db.SaveChangesAsync();
            Logger.Get(Logging.TraceSourceName.COFFEETRACKER_CONTROLLERS)
                .TraceEvent(TraceEventType.Stop, (int)Logging.EventId.Stop, "{0} with id {1} updated. entity: \n {2}", MODELNAME, key, coffeeOrder);

            return Updated(coffeeOrder);
        }

        // POST: odata/CoffeeOrders
        public async Task<IHttpActionResult> Post(CoffeeOrder coffeeOrder)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Logger.Get(Logging.TraceSourceName.COFFEETRACKER_CONTROLLERS)
                .TraceEvent(TraceEventType.Start, (int)Logging.EventId.Start, "Start Insert {0}: \n {1}...", MODELNAME, coffeeOrder);

            db.CoffeeOrders.Add(coffeeOrder);
            await db.SaveChangesAsync();

            Logger.Get(Logging.TraceSourceName.COFFEETRACKER_CONTROLLERS)
                            .TraceEvent(TraceEventType.Stop, (int)Logging.EventId.Stop, "Insert finished. EntityId: {0}", coffeeOrder.Id);
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
            Logger.Get(Logging.TraceSourceName.COFFEETRACKER_CONTROLLERS)
                            .TraceEvent(TraceEventType.Start, (int)Logging.EventId.Start, "Start Update {0} with Id: {1}...", MODELNAME, key);

            patch.Patch(coffeeOrder);

            await db.SaveChangesAsync();

            coffeeOrder = await db.CoffeeOrders.FindAsync(key);
            Contract.Assert(null != coffeeOrder);

            Logger.Get(Logging.TraceSourceName.COFFEETRACKER_CONTROLLERS)
                .TraceEvent(TraceEventType.Information, (int)Logging.EventId.Default, "Stop update {0} with Id: {1} \n {2}", MODELNAME, coffeeOrder.Id, Logging.FormatEntity(coffeeOrder));

            return Updated(coffeeOrder);
        }

        // DELETE: odata/CoffeeOrders(5)
        public async Task<IHttpActionResult> Delete([FromODataUri] long key)
        {
            var coffeeOrder = await db.CoffeeOrders.FindAsync(key);
            if (coffeeOrder == null)
            {
                return NotFound();
            }

            Logger.Get(Logging.TraceSourceName.COFFEETRACKER_CONTROLLERS)
                .TraceEvent(TraceEventType.Warning, (int)Logging.EventId.Default, "Start remove {0} with Id: {1} \n {2}", MODELNAME, coffeeOrder.Id, Logging.FormatEntity(coffeeOrder));

            db.CoffeeOrders.Remove(coffeeOrder);
            await db.SaveChangesAsync();

            Logger.Get(Logging.TraceSourceName.COFFEETRACKER_CONTROLLERS)
                .TraceEvent(TraceEventType.Warning, (int)Logging.EventId.Default, "{0} with id {1} Removed", MODELNAME, coffeeOrder.Id);

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: odata/CoffeeOrders(5)/Coffee
        [EnableQuery]
        public SingleResult<Coffee> GetCoffee([FromODataUri] long key)
        {
            Logger.Get(Logging.TraceSourceName.COFFEETRACKER_CONTROLLERS)
                .TraceEvent(TraceEventType.Information, (int)Logging.EventId.Default, "Get {0} with Id: {1}", MODELNAME, key);

            return SingleResult.Create(db.CoffeeOrders.Where(m => m.Id == key).Select(m => m.Coffee));
        }

        // GET: odata/CoffeeOrders(5)/User
        [EnableQuery]
        public SingleResult<User> GetUser([FromODataUri] long key)
        {
            Logger.Get(Logging.TraceSourceName.COFFEETRACKER_CONTROLLERS)
                .TraceEvent(TraceEventType.Information, (int)Logging.EventId.Default, "Get {0} with Id: {1}", MODELNAME, key);

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
