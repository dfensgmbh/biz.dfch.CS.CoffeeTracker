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
using biz.dfch.CS.CoffeeTracker.Core.DbContext;
using biz.dfch.CS.CoffeeTracker.Core.Model;
using biz.dfch.CS.CoffeeTracker.Core.Security.PermissionChecker;
using biz.dfch.CS.CoffeeTracker.Core.Stores;
using biz.dfch.CS.CoffeeTracker.Core.Validation;

namespace biz.dfch.CS.CoffeeTracker.Core.Managers
{
    public class CoffeeOrdersManager : IDisposable
    {
        private readonly CoffeeTrackerDbContext db;
        private readonly ApplicationUserManager userManager;
        private readonly CoffeesManager coffeesManager;
        private readonly PermissionChecker permissionChecker;
        public readonly CoffeeOrdersValidator Validator;

        public CoffeeOrdersManager()
        {

            db = new CoffeeTrackerDbContext();
            userManager = new ApplicationUserManager(new AppUserStore());
            coffeesManager = new CoffeesManager();
            Validator = new CoffeeOrdersValidator(this);
            permissionChecker = new PermissionChecker(userManager.GetCurrentUser());
        }

        public IEnumerable<CoffeeOrder> GetCoffeeOrdersOfCurrentUser()
        {
            var user = userManager.GetCurrentUser();
            var coffeeOrders = db.CoffeeOrders.Where(c => c.UserId == user.Id);

            return coffeeOrders;
        }

        public IEnumerable<CoffeeOrder> Get()
        {
            if (userManager.GetCurrentUser().IsAdmin)
            {
                return db.CoffeeOrders;
            }
            return GetCoffeeOrdersOfCurrentUser();
        }

        public CoffeeOrder Get(long id)
        {
            Contract.Requires(0 < id, "|404|");
            var coffeeOrder = db.CoffeeOrders.FirstOrDefault(c => c.Id == id);
            Contract.Assert(null != coffeeOrder, "|404|");

            var hasPermission = permissionChecker.HasPermission(coffeeOrder);
            Contract.Assert(hasPermission, "|403|");

            return coffeeOrder;
        }

        public CoffeeOrder Get(string name)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(name), "|400|");
            var coffeeOrder = db.CoffeeOrders.FirstOrDefault(c => c.Name == name);
            Contract.Assert(null != coffeeOrder, "|404|");

            var hasPermission = permissionChecker.HasPermission(coffeeOrder);
            Contract.Assert(hasPermission, "|403|");


            return coffeeOrder;
        }

        public CoffeeOrder Update(long key, CoffeeOrder update)
        {
            Contract.Requires(null != update, "|400|");
            Contract.Requires(0 < key, "|404|");

            var exists = Validator.ExistsInDatabase(key);
            Contract.Assert(exists, "|404|");

            var hasPermission = permissionChecker.HasPermission(Get(key), update);
            Contract.Assert(hasPermission, "|403|");

            var coffeeOrder = Get(key);
            coffeeOrder.CoffeeId = update.CoffeeId;

            db.SaveChanges();

            return Get(key);
        }

        public CoffeeOrder Create(CoffeeOrder coffeeOrder)
        {
            Contract.Requires(null != coffeeOrder, "|400|");
            Contract.Requires(0 < coffeeOrder.CoffeeId, "|404|");
            Contract.Requires(0 < coffeeOrder.UserId, "|404|");

            coffeeOrder.Created = DateTimeOffset.Now;
            coffeesManager.DecreaseStock(coffeeOrder.CoffeeId);
            
            db.CoffeeOrders.Add(coffeeOrder);
            db.SaveChanges();

            return Get(coffeeOrder.Name);
        }

        public void Delete(CoffeeOrder coffeeOrder)
        {
            Contract.Requires(null != coffeeOrder, "|400|");
            Contract.Requires(0 < coffeeOrder.Id, "|404|");

            var hasPermission = permissionChecker.HasPermission(coffeeOrder);
            Contract.Assert(hasPermission, "|403|");

            db.CoffeeOrders.Remove(coffeeOrder);
            db.SaveChanges();
        }

        public void Dispose()
        {
            db?.Dispose();
        }
    }
}