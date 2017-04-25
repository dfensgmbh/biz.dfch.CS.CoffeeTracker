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
using biz.dfch.CS.CoffeeTracker.Client.Wpf.UserControls.CompleteViews.Base;

namespace biz.dfch.CS.CoffeeTracker.Client.Wpf.Classes.Managers
{
    public class CoffeeOrdersManager : BaseManager
    {
        private IEnumerable<CoffeeOrder> coffeeOrders;

        public CoffeeOrdersManager(CoffeeTrackerClientWpfServiceContext ctx)
            : base(ctx)
        {
        }

        private IEnumerable<CoffeeOrder> ApplyUserFilter(params ApplicationUser[] users)
        {
            Contract.Requires(0 < users.Length);
            Contract.Requires(null != users[0]);

            IEnumerable<CoffeeOrder> filterdEnumerable = null;

            foreach (var applicationUser in users)
            {
                filterdEnumerable = coffeeOrders.Where(u => u.Name.Equals(applicationUser.Name)).AsEnumerable();
            }

            return filterdEnumerable;
        }

        private IEnumerable<CoffeeOrder> ApplyTimeFilter(DateTimeOffset from, DateTimeOffset until)
        {
            Contract.Requires(null != from);
            Contract.Requires(null != until);
            Contract.Requires(from <= until);
        }
    }
}
