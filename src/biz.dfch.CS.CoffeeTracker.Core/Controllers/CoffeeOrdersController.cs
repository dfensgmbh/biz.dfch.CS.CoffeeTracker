﻿using System;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.OData;
using biz.dfch.CS.CoffeeTracker.Core.Model;
using biz.dfch.CS.CoffeeTracker.Core.Logging;
using biz.dfch.CS.CoffeeTracker.Core.Managers;
using biz.dfch.CS.CoffeeTracker.Core.Stores;
using biz.dfch.CS.CoffeeTracker.Core.Validation;
using Microsoft.AspNet.Identity.EntityFramework;

namespace biz.dfch.CS.CoffeeTracker.Core.Controllers
{
    [Authorize]
    public class CoffeeOrdersController : ODataController
    {
        private const string MODELNAME = ControllerLogging.ModelNames.COFFEEORDER;

        private readonly Lazy<CoffeesManager> coffeesManagerLazy = new Lazy<CoffeesManager>(() =>
            new CoffeesManager());
        private CoffeesManager coffeesManager => coffeesManagerLazy.Value;

        private readonly Lazy<ApplicationUserManager> userManagerLazy = new Lazy<ApplicationUserManager>(() =>
           new ApplicationUserManager(new AppUserStore()));
        private ApplicationUserManager userManager => userManagerLazy.Value;

        private readonly Lazy<CoffeeOrdersManager> coffeeOrdersManagerLazy = new Lazy<CoffeeOrdersManager>(() =>
            new CoffeeOrdersManager());
        private CoffeeOrdersManager coffeeOrdersManager => coffeeOrdersManagerLazy.Value;

        private readonly Lazy<StatisticsManager> statisticsManagerLazy = new Lazy<StatisticsManager>(() =>
            new StatisticsManager());
        private StatisticsManager statisticsManager => statisticsManagerLazy.Value;

        public CoffeeOrdersController()
        {
        }

        // GET: api/CoffeeOrders
        [EnableQuery]
        public IQueryable<CoffeeOrder> GetCoffeeOrders()
        {
            ControllerLogging.LogGetEntities(MODELNAME);

            return coffeeOrdersManager.Get().AsQueryable();
        }

        // GET: api/CoffeeOrders(5)
        [EnableQuery]
        public IHttpActionResult GetCoffeeOrder([FromODataUri] long key)
        {
            Contract.Requires(0 < key, "|404|");
            ControllerLogging.LogGetEntity(MODELNAME, key.ToString());

            return Ok(coffeeOrdersManager.Get(key));
        }

        // PUT: api/CoffeeOrders(5)
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

        // POST: api/CoffeeOrders
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

        // PATCH: api/CoffeeOrders(5)
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

        // DELETE: api/CoffeeOrders(5)
        public async Task<IHttpActionResult> Delete([FromODataUri] long key)
        {
            Contract.Requires(0 < key, "|404|");

            coffeeOrdersManager.Validator.ExistsInDatabase(key);
            var coffeeOrder = coffeeOrdersManager.Get(key);

            ControllerLogging.LogDeleteEntityStart(MODELNAME, coffeeOrder);

            coffeeOrdersManager.Delete(coffeeOrder);

            ControllerLogging.LogDeleteEntityStop(MODELNAME, coffeeOrder);

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: api/CoffeeOrders(5)/Coffee
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

        [HttpPost]
        public IHttpActionResult GetCoffeeConsumptionByUser(ODataActionParameters parameters)
        {
            var coffeesOrdered = statisticsManager.CoffeeConsumptionByUser(userManager.GetCurrentUser());
            return Ok(coffeesOrdered);
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
