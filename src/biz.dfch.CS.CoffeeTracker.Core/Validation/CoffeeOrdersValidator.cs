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
using biz.dfch.CS.CoffeeTracker.Core.DbContext;
using biz.dfch.CS.CoffeeTracker.Core.Managers;
using biz.dfch.CS.CoffeeTracker.Core.Model;
using Microsoft.AspNet.Identity.EntityFramework;

namespace biz.dfch.CS.CoffeeTracker.Core.Validation
{
    public class CoffeeOrdersValidator
    {
        private CoffeeOrdersManager coffeeOrdersManager;
        private readonly ApplicationUserManager userManager;


        public CoffeeOrdersValidator(CoffeeOrdersManager coffeeOrdersManager)
        {
            this.coffeeOrdersManager = coffeeOrdersManager;
            userManager = new ApplicationUserManager(new UserStore<IdentityUser>(), null);
        }

        public bool ExistsInDatabase(string name)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(name));

            var result = coffeeOrdersManager.Get(name);

            return null != result;
        }

        public bool ExistsInDatabase(long key)
        {
            Contract.Requires(0 < key);

            var result = coffeeOrdersManager.Get(key);

            return null != result;
        }
    }
}