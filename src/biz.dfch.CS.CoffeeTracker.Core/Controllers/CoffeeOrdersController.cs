using System.Diagnostics.Contracts;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.OData;
using biz.dfch.CS.CoffeeTracker.Core.Model;
using biz.dfch.CS.CoffeeTracker.Core.Logging;
using biz.dfch.CS.CoffeeTracker.Core.Managers;
using biz.dfch.CS.CoffeeTracker.Core.Validation;
using Microsoft.AspNet.Identity.EntityFramework;

namespace biz.dfch.CS.CoffeeTracker.Core.Controllers
{
    /*
    The WebApiConfig class may require additional changes to add a route for this Controller. Merge these statements into the Register method of the WebApiConfig class as applicable. Note that OData URLs are case sensitive.

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
        private readonly CoffeesManager coffeesManager;
        private readonly ApplicationUserManager userManager;
        private readonly CoffeeOrdersManager coffeeOrdersManager;

        public CoffeeOrdersController()
        {
            coffeeOrdersManager = new CoffeeOrdersManager(this);
            coffeesManager = new CoffeesManager(this);
            userManager = new ApplicationUserManager(new UserStore<IdentityUser>(), this);
        }

        // GET: odata/CoffeeOrders
        [EnableQuery]
        public IQueryable<CoffeeOrder> GetCoffeeOrders()
        {
            ControllerLogging.LogGetEntities(MODELNAME);
            
            return coffeeOrdersManager.GetCoffeeOrdersOfCurrentUser().AsQueryable();
        }

        // GET: odata/CoffeeOrders(5)
        [EnableQuery]
        public SingleResult<CoffeeOrder> GetCoffeeOrder([FromODataUri] long key)
        {
            Contract.Requires(0 < key, "|404|");
            ControllerLogging.LogGetEntity(MODELNAME, key.ToString());

            return SingleResult.Create(coffeeOrdersManager.GetAsQueryable(key));
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

            ControllerLogging.LogUpdateEntityStartPut(MODELNAME, key.ToString());

            var coffeeOrder = coffeeOrdersManager.Update(key, modifiedCoffeeOrder);

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

            coffeeOrdersManager.Create(coffeeOrder);

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

            ControllerLogging.LogUpdateEntityStartPatch(MODELNAME, key.ToString());

            var coffeeOrder = coffeeOrdersManager.Update(key, patch.GetEntity());

            coffeeOrder = coffeeOrdersManager.Get(key);
            Contract.Assert(null != coffeeOrder);

            ControllerLogging.LogUpdateEntityStopPatch(MODELNAME, coffeeOrder);

            return Updated(coffeeOrder);
        }

        // DELETE: odata/CoffeeOrders(5)
        public async Task<IHttpActionResult> Delete([FromODataUri] long key)
        {
            Contract.Requires(0 < key, "|404|");

            coffeeOrdersManager.validator.ExistsInDatabase(key);
            var coffeeOrder = coffeeOrdersManager.Get(key);

            ControllerLogging.LogDeleteEntityStart(MODELNAME, coffeeOrder);

            coffeeOrdersManager.Delete(coffeeOrder);

            ControllerLogging.LogDeleteEntityStop(MODELNAME, coffeeOrder);

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: odata/CoffeeOrders(5)/Coffee
        [EnableQuery]
        public SingleResult<Coffee> GetCoffee([FromODataUri] long key)
        {
            Contract.Requires(0 < key, "|404|");

            ControllerLogging.LogGetEntity(ControllerLogging.ModelNames.COFFEE, key.ToString());

            return SingleResult.Create(coffeesManager.GetAsQueryable(key));
        }

        // GET: api/CoffeeOrders(5)/ApplicationUser
        [EnableQuery]
        public SingleResult<ApplicationUser> GetUser([FromODataUri] long key)
        {
            Contract.Requires(0 < key, "|404|");

            ControllerLogging.LogGetEntity(ControllerLogging.ModelNames.USER, key.ToString());

            return SingleResult.Create(userManager.GetUserAsQueryable(key));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                coffeeOrdersManager.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
