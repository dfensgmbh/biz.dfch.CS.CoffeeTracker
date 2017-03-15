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
using System.Diagnostics.Contracts;
using System.Linq;
using biz.dfch.CS.CoffeeTracker.Core.DbContext;
using biz.dfch.CS.CoffeeTracker.Core.Model;
using biz.dfch.CS.CoffeeTracker.Core.Security.PermissionChecker;
using biz.dfch.CS.CoffeeTracker.Core.Stores;

namespace biz.dfch.CS.CoffeeTracker.Core.Managers
{
    public class StatisticsManager : IDisposable
    {
        private readonly CoffeeTrackerDbContext db;
        private readonly PermissionChecker permissionChecker;

        public StatisticsManager()
        {
            db = new CoffeeTrackerDbContext();
            var currentUser = new ApplicationUserManager(new AppUserStore()).GetCurrentUser();
            permissionChecker = new PermissionChecker(currentUser);
        }

        public Coffee MostOrderedCoffeeByUser(ApplicationUser user)
        {
            var coffeeOrders = db.CoffeeOrders.Where(co => co.UserId == user.Id);

            var mostOrdered = coffeeOrders
                .GroupBy(c => c)
                .OrderByDescending(d => d.Count())
                .Take(1)
                .Select(d => d.Key)
                .FirstOrDefault();

            Contract.Assert(null != mostOrdered, "|404|");
            return mostOrdered.Coffee;
        }

        public long CoffeeConsumptionByUser(ApplicationUser user, DateTimeOffset from, DateTimeOffset until)
        {
            Contract.Requires(null != user, "|400|");
            Contract.Requires(null != from, "|400|");
            Contract.Requires(null != until, "|400|");

            var hasPermission = permissionChecker.HasPermission(user);
            Contract.Assert(hasPermission, "|403|");

            var ordersOfUser = db.CoffeeOrders
                .Where(co => co.UserId == user.Id 
                    && co.Created >= from 
                    && co.Created <= until);
            
            return ordersOfUser.Count();
        }

        public long CoffeeConsumption(DateTimeOffset from, DateTimeOffset until)
        {
            Contract.Requires(null != from, "|400|");
            Contract.Requires(null != until, "|400|");

            var coffeeOrders = db.CoffeeOrders
                .Where(co => co.Created >= from
                    && co.Created <= until)
                .ToList();

            return coffeeOrders.Count;
        }

        public long CoffeeConsumptionByCoffee(Coffee coffee, DateTimeOffset from, DateTimeOffset until)
        {
            Contract.Requires(null != coffee, "|400|");
            Contract.Requires(null != until, "|400|");
            Contract.Requires(null != from, "|400|");

            var coffeeOrders = db.CoffeeOrders
                .Where(co => co.CoffeeId == coffee.Id
                             && co.Created >= from
                             && co.Created <= until)
                .ToList();

            return coffeeOrders.Count;
        }

        public Coffee MostOrderedCoffee(DateTimeOffset from, DateTimeOffset until)
        {
            Contract.Requires(null != from, "|400|");
            Contract.Requires(null != until, "|400|");

            var coffeeOrderList = db.CoffeeOrders.Where(co => co.Created >= from && co.Created <= until);

            var coffeeIdListFiltered = coffeeOrderList.GroupBy(x => x.CoffeeId)
                .OrderByDescending(g => g.Count())
                .Take(5)
                .Select(g => g.Key)
                .ToList();

            var coffeeId = coffeeIdListFiltered[0];
            var coffee = db.Coffees.FirstOrDefault(c => c.Id == coffeeId);
            Contract.Assert(null != coffee, "|500|");

            return coffee;
        }

        public void Dispose()
        {
            db?.Dispose();
        }
    }
}