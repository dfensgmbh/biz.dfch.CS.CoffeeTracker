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
using System.Web;
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

        public Coffee MostOrderedCoffee()
        {
            var coffeeOrders = db.CoffeeOrders;

            var mostOrdered = coffeeOrders
                .GroupBy(c => c)
                .OrderByDescending(d => d.Count())
                .Take(1)
                .Select(d => d.Key)
                .FirstOrDefault();

            Contract.Assert(null != mostOrdered, "|404|");
            return mostOrdered.Coffee;
        }

        public long CoffeeConsumption(ApplicationUser user)
        {
            var beginOfTime = DateTimeOffset.MinValue;
            var endOfTime = DateTimeOffset.MaxValue;

            return CoffeeConsumption(user, beginOfTime, endOfTime);
        }

        public long CoffeeConsumption(ApplicationUser user, DateTimeOffset from, DateTimeOffset until)
        {
            Contract.Requires(null != user, "|400|");
            Contract.Requires(null != from, "|400|");
            Contract.Requires(null != until, "|400|");

            var hasPermission = permissionChecker.HasPermission(user);
            Contract.Assert(hasPermission, "|403|");

            var ordersOfUser = db.CoffeeOrders
                .Where(co => co.UserId == user.Id && co.Created >= from && co.Created <= until);
            
            return ordersOfUser.Count();
        }

        public void Dispose()
        {
            db?.Dispose();
        }
    }
}