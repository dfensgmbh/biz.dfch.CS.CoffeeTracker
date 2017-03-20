/**
 * Copyright 2017 d-fens GmbH
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;
using System.Collections.Generic;
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
            Contract.Requires(null != modifiedCoffeeOrder, "|400|");

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
        public IHttpActionResult Post(CoffeeOrder coffeeOrder)
        {
            Contract.Requires(null != coffeeOrder, "|404|");

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ControllerLogging.LogInsertEntityStart(MODELNAME, coffeeOrder);

            coffeeOrder = coffeeOrdersManager.Create(coffeeOrder);

            ControllerLogging.LogInsertEntityStop(MODELNAME, coffeeOrder);

            return Created(coffeeOrder);
        }

        // PATCH: api/CoffeeOrders(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public IHttpActionResult Patch([FromODataUri] long key, Delta<CoffeeOrder> patch)
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
        public IHttpActionResult Delete([FromODataUri] long key)
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
        public IHttpActionResult GetCoffee([FromODataUri] long key)
        {
            Contract.Requires(0 < key, "|400|");

            var coffeeOrder = coffeeOrdersManager.Get(key);
            Contract.Assert(null != coffeeOrder, "|404|");

            ControllerLogging.LogGetEntity(ControllerLogging.ModelNames.COFFEE, coffeeOrder.CoffeeId.ToString());

            var coffee = coffeesManager.Get(coffeeOrder.CoffeeId);
            Contract.Assert(null != coffee, "|500|");

            return Ok(coffee);
        }

        // GET: api/CoffeeOrders(5)/ApplicationUser
        [EnableQuery]
        public IHttpActionResult GetApplicationUser([FromODataUri] long key)
        {
            Contract.Requires(0 < key, "|400|");

            var coffeeOrder = coffeeOrdersManager.Get(key);
            Contract.Assert(null != coffeeOrder, "|404|");

            ControllerLogging.LogGetEntity(ControllerLogging.ModelNames.USER, coffeeOrder.CoffeeId.ToString());

            var user = userManager.GetUser(coffeeOrder.UserId);
            Contract.Assert(null != user, "|500|");

            return Ok(user);
        }

        [HttpPost]
        public IHttpActionResult GetCoffeeConsumptionByUser(ODataActionParameters parameters)
        {
            Contract.Requires(null != parameters, "|400|");
            Contract.Requires(null != parameters["From"], "|400|");
            Contract.Requires(null != parameters["Until"], "|400|");


            var from = (DateTimeOffset)parameters["From"];
            var until = (DateTimeOffset)parameters["Until"];

            var orderedCoffeesCount = statisticsManager.CoffeeConsumptionByUser(userManager.GetCurrentUser(), from, until);
            return Ok(orderedCoffeesCount);
        }

        [HttpPost]
        public IHttpActionResult GetCoffeeConsumptionByCurrentUser(ODataActionParameters parameters)
        {
            Contract.Requires(null != parameters, "|400|");
            Contract.Requires(null != parameters["From"], "|400|");
            Contract.Requires(null != parameters["Until"], "|400|");

            var from = (DateTimeOffset) parameters["From"];
            var until = (DateTimeOffset) parameters["Until"];

            var coffeesOrdered = statisticsManager.CoffeeConsumptionByUser(userManager.GetCurrentUser(), from, until);
            return Ok(coffeesOrdered);
        }

        [HttpPost]
        public IHttpActionResult GetCoffeeConsumption(ODataActionParameters parameters)
        {
            Contract.Requires(null != parameters, "|400|");
            Contract.Requires(null != parameters["From"], "|400|");
            Contract.Requires(null != parameters["Until"], "|400|");

            var from = (DateTimeOffset)parameters["From"];
            var until = (DateTimeOffset)parameters["Until"];

            var coffeesOrdered = statisticsManager.CoffeeConsumption(from, until);
            return Ok(coffeesOrdered);
        }

        [HttpPost]
        public IHttpActionResult GetCoffeeConsumptionByCoffee(ODataActionParameters parameters)
        {
            Contract.Requires(null != parameters, "|400|");
            Contract.Requires(null != parameters["From"], "|400|");
            Contract.Requires(null != parameters["Until"], "|400|");
            Contract.Requires(!string.IsNullOrWhiteSpace((string)parameters["Name"]), "|400|");
            Contract.Requires(!string.IsNullOrWhiteSpace((string)parameters["Brand"]), "|400|");

            var coffeeName = (string)parameters["Name"];
            var coffeeBrand = (string)parameters["Brand"];
            var from = (DateTimeOffset)parameters["From"];
            var until = (DateTimeOffset)parameters["Until"];

            var coffee = coffeesManager.Get(coffeeName, coffeeBrand);

            var coffeesOrdered = statisticsManager.CoffeeConsumptionByCoffee(coffee, from, until);
            return Ok(coffeesOrdered);
        }

        [HttpPost]
        public IHttpActionResult GetMostOrderedCoffee(ODataActionParameters parameters)
        {
            Contract.Requires(null != parameters, "|400|");
            Contract.Requires(null != parameters["From"], "|400|");
            Contract.Requires(null != parameters["Until"], "|400|");

            var from = (DateTimeOffset)parameters["From"];
            var until = (DateTimeOffset)parameters["Until"];

            var mostOrderedCoffee = statisticsManager.MostOrderedCoffee(from, until);
            return Ok(mostOrderedCoffee);
        }

        [HttpPost]
        public IHttpActionResult GetMostOrderedCoffeeByUser(ODataActionParameters parameters)
        {
            Contract.Requires(null != parameters, "|400|");
            Contract.Requires(null != parameters["From"], "|400|");
            Contract.Requires(null != parameters["Until"], "|400|");
            Contract.Requires(!string.IsNullOrWhiteSpace((string)parameters["Email"]), "|400|");

            var from = (DateTimeOffset)parameters["From"];
            var until = (DateTimeOffset)parameters["Until"];
            var userEmail = (string) parameters["Email"];

            var isValidMail = ApplicationUser.IsValidEmail(userEmail);
            Contract.Assert(isValidMail, "|400|");

            var user = userManager.GetUser(userEmail);
            Contract.Assert(null != user, "|404|");

            var favouriteCoffee = statisticsManager.MostOrderedCoffeeByUser(user, from, until);
            return Ok(favouriteCoffee);
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