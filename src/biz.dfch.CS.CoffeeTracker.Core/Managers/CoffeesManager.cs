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

using System.Diagnostics.Contracts;
using System.Linq;
using biz.dfch.CS.CoffeeTracker.Core.DbContext;
using biz.dfch.CS.CoffeeTracker.Core.Model;
using biz.dfch.CS.CoffeeTracker.Core.Security.PermissionChecker;
using biz.dfch.CS.CoffeeTracker.Core.Stores;
using Microsoft.AspNet.Identity.EntityFramework;

namespace biz.dfch.CS.CoffeeTracker.Core.Managers
{
    public class CoffeesManager
    {
        private readonly CoffeeTrackerDbContext db;
        private readonly PermissionChecker permissionChecker;

        public CoffeesManager()
        {
            db = new CoffeeTrackerDbContext();
            var currentUser = new ApplicationUserManager(new AppUserStore()).GetCurrentUser();
            permissionChecker = new PermissionChecker(currentUser);
        }

        public CoffeesManager(bool skipPermissionChecks)
        {
            db = new CoffeeTrackerDbContext();
            permissionChecker = new PermissionChecker(skipPermissionChecks);
        }

        public IQueryable<Coffee> Get()
        {
            return db.Coffees;
        }

        public Coffee Get(long key)
        {
            Contract.Requires(0 < key, "|404|");

            return db.Coffees.FirstOrDefault(c => c.Id == key);
        }

        public Coffee Get(string name)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(name), "|404|");

            return db.Coffees.FirstOrDefault(c => c.Name == name);
        }

        public IQueryable<Coffee> GetAsQueryable(long key)
        {
            Contract.Requires(0 < key, "|404|");

            return db.Coffees.Where(c => c.Id == key);
        }

        public Coffee Update(Coffee modifiedCoffee)
        {
            Contract.Requires(null != modifiedCoffee, "|400|");
            Contract.Requires(0 < modifiedCoffee.Id, "|404|");

            var hasPermission = permissionChecker.HasPermission(modifiedCoffee);
            Contract.Assert(hasPermission, "|403|");

            var coffee = Get(modifiedCoffee.Id);

            coffee.Name = modifiedCoffee.Name;
            coffee.Brand = modifiedCoffee.Brand;
            coffee.LastDelivery = modifiedCoffee.LastDelivery;
            coffee.Price = modifiedCoffee.Price;
            coffee.Stock = coffee.Stock;

            db.SaveChanges();

            return Get(coffee.Id);
        }

        public Coffee Create(Coffee coffee)
        {
            Contract.Requires(null != coffee, "|400|");

            var hasPermission = permissionChecker.HasPermission(coffee);
            Contract.Assert(hasPermission, "|403|");

            db.Coffees.Add(coffee);
            db.SaveChanges();

            return Get(coffee.Name);
        }

        public void Delete(long id)
        {
            Contract.Requires(0 < id, "|404|");

            var hasPermission = permissionChecker.HasPermission(Get(id));
            Contract.Assert(hasPermission, "|403|");

            Delete(Get(id));
        }

        public void Delete(Coffee coffee)
        {
            Contract.Requires(null != coffee, "|400|");

            var hasPermission = permissionChecker.HasPermission(coffee);
            Contract.Assert(hasPermission, "|403|");

            db.Coffees.Remove(coffee);
            db.SaveChanges();
        }
    }
}