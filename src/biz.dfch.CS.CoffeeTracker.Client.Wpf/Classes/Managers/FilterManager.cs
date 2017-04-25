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

namespace biz.dfch.CS.CoffeeTracker.Client.Wpf.Classes.Managers
{
    public class FilterManager
    {
        public IEnumerable<CoffeeOrder> CoffeeOrders;

        public FilterManager(IEnumerable<CoffeeOrder> coffeeOrders)
        {
            CoffeeOrders = coffeeOrders;
        }

        public IEnumerable<CoffeeOrder> ApplyUserFilter(params ApplicationUser[] users)
        {
            Contract.Requires(0 < users.Length);
            Contract.Requires(null != users[0]);

            IEnumerable<CoffeeOrder> filterdEnumerable = null;

            foreach (var applicationUser in users)
            {
                filterdEnumerable =
                    CoffeeOrders.Where(u => u.ApplicationUser.Name.Equals(applicationUser.Name)).AsEnumerable();
            }

            return filterdEnumerable;
        }

        public IEnumerable<CoffeeOrder> ApplyTimeFilter(DateTimeOffset from, DateTimeOffset until)
        {
            Contract.Requires(null != from);
            Contract.Requires(null != until);
            Contract.Requires(from <= until);

            return CoffeeOrders.Where(c => c.Created >= from && c.Created <= until);
        }

        public IEnumerable<CoffeeOrder> ApplyCoffeeFilter(params Coffee[] coffees)
        {
            Contract.Requires(0 < coffees.Length);
            Contract.Requires(null != coffees[0]);

            IEnumerable<CoffeeOrder> filterdEnumerable = null;

            foreach (var coffee in coffees)
            {
                filterdEnumerable = CoffeeOrders.Where(u => u.Coffee.Name.Equals(coffee.Name)
                                                            && u.Coffee.Brand.Equals(coffee.Brand))
                                                            .AsEnumerable();
            }

            return filterdEnumerable;
        }
    }
}