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
using biz.dfch.CS.CoffeeTracker.Core.Managers;
using biz.dfch.CS.CoffeeTracker.Core.Model;

namespace biz.dfch.CS.CoffeeTracker.Core.Validation
{
    public class CoffeeOrdersValidator
    {
        private CoffeeTrackerDbContext db;
        private CoffeeOrdersManager manager;

        public CoffeeOrdersValidator(CoffeeTrackerDbContext db, CoffeeOrdersManager manager)
        {
            this.db = db;
            this.manager = manager;
        }

        public bool ExistsInDatabase(string name)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(name));

            var result = manager.Get(name);

            return null != result;
        }

        public bool ExistsInDatabase(long key)
        {
            Contract.Requires(0 < key);

            var result = manager.Get(key);

            return null != result;
        }

        public bool HasPermissions(CoffeeOrder coffeeOrder)
        {
            return HasPermissions(coffeeOrder.Id);
        }

        public bool HasPermissions(long key)
        {
            Contract.Requires(0 < key);

            var coffeeOrder = manager.Get(key);
            var user = ApplicationUserManager.GetCurrentUser(manager.oDataController);
            Contract.Assert(!string.IsNullOrWhiteSpace(user.Name));

            return user.Id == coffeeOrder.UserId;
        }
    }
}