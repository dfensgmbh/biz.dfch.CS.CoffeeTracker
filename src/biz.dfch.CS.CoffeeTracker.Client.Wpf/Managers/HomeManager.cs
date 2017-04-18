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
using System.Text;
using System.Threading.Tasks;
using biz.dfch.CS.CoffeeTracker.Client.CoffeeTrackerService;
using biz.dfch.CS.CoffeeTracker.Client.Wpf.Controls;

namespace biz.dfch.CS.CoffeeTracker.Client.Wpf.Managers
{
    public class HomeManager
    {
        public CoffeeTrackerServiceContext context;

        public HomeManager(CoffeeTrackerServiceContext ctx)
        {
            Contract.Requires(null != ctx);

            context = ctx;
        }

        public Coffee RefreshCoffee(Coffee coffee)
        {
            var client = ClientContext.CreateServiceContext();
            return client.Coffees.Where(c => c.Brand == coffee.Brand).Where(x => x.Name == coffee.Name).FirstOrDefault();
        }

        public void AddCoffeeOrder(long coffeeId)
        {
            var client = ClientContext.CreateServiceContext();
            var coffeeOrder = new CoffeeOrder()
            {
                Name = ClientContext.CurrentUserName + DateTimeOffset.Now,
                UserId = ClientContext.CurrentUserId,
                CoffeeId = coffeeId
            };
            client.AddToCoffeeOrders(coffeeOrder);
            client.SaveChanges();
        }
    }
}
