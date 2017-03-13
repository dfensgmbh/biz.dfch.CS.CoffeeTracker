using System;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.OData;
using System.Web.Http.Results;
using biz.dfch.CS.CoffeeTracker.Core.DbContext;
using biz.dfch.CS.CoffeeTracker.Core.Logging;
using biz.dfch.CS.CoffeeTracker.Core.Managers;
using biz.dfch.CS.CoffeeTracker.Core.Model;

namespace biz.dfch.CS.CoffeeTracker.Core.Controllers
{
    [Authorize]
    public class CoffeesController : ODataController
    {
        private readonly Lazy<CoffeesManager> coffeesManagerLazy = new Lazy<CoffeesManager>(() =>
            new CoffeesManager());
        private CoffeesManager coffeesManager => coffeesManagerLazy.Value;

        private const string MODELNAME = ControllerLogging.ModelNames.COFFEE;

        public CoffeesController()
        {
        }

        [EnableQuery]
        public IQueryable<Coffee> GetCoffees()
        {
            ControllerLogging.LogGetEntities(MODELNAME);

            return coffeesManager.Get();
        }

        [EnableQuery]
        public IHttpActionResult GetCoffee([FromODataUri] long key)
        {
            Contract.Requires(0 < key);

            ControllerLogging.LogGetEntity(MODELNAME, key.ToString());

            return Ok(coffeesManager.Get(key));
        }

        // PUT: odata/Coffees(5)
        public async Task<IHttpActionResult> Put([FromODataUri] long key, Coffee modifiedCoffee)
        {
            Contract.Requires(0 < key, "|404|");
            Contract.Requires(null != modifiedCoffee, "|404|");

            Validate(modifiedCoffee);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var coffee = coffeesManager.Get(key);
            Contract.Assert(null != coffee, "|404|");

            ControllerLogging.LogUpdateEntityStartPut(MODELNAME, key.ToString());

            coffeesManager.Update(key, modifiedCoffee);

            ControllerLogging.LogUpdateEntityStopPut(MODELNAME, coffee);

            return Updated(coffee);
        }

        // POST: odata/Coffees
        public async Task<IHttpActionResult> Post(Coffee coffee)
        {
            Contract.Requires(null != coffee, "|400|");

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ControllerLogging.LogInsertEntityStart(MODELNAME, coffee);

            coffeesManager.Create(coffee);

            ControllerLogging.LogInsertEntityStop(MODELNAME, coffee);

            return Created(coffee);
        }

        // PATCH: odata/Coffees(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public IHttpActionResult Patch([FromODataUri] long key, Delta<Coffee> patch)
        {
            Contract.Requires(0 < key, "|404|");
            Contract.Requires(null != patch, "|400|");

            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var coffee = coffeesManager.Get(key);
            Contract.Assert(null != coffee, "|404|");
            
            ControllerLogging.LogUpdateEntityStartPatch(MODELNAME, key.ToString());

            coffeesManager.Update(key, patch);
               
            ControllerLogging.LogUpdateEntityStopPatch(MODELNAME, coffee);

            return Updated(coffee);
        }

        // DELETE: odata/Coffees(5)
        public async Task<IHttpActionResult> Delete([FromODataUri] long key)
        {
            Contract.Requires(0 < key);

            var coffee = coffeesManager.Get(key);
            Contract.Assert(null != coffee, "|404|");

            ControllerLogging.LogDeleteEntityStart(MODELNAME, coffee);
            
            coffeesManager.Delete(coffee);

            ControllerLogging.LogDeleteEntityStop(MODELNAME, coffee);

            return StatusCode(HttpStatusCode.NoContent);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                
            }
            base.Dispose(disposing);
        }
    }
}
