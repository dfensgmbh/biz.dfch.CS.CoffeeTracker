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
using biz.dfch.CS.CoffeeTracker.Core.Model;

namespace biz.dfch.CS.CoffeeTracker.Core.Security.PermissionChecker
{
    public class PermissionChecker
    {
        private readonly ApplicationUser currentUser;

        public PermissionChecker(ApplicationUser user)
        {
            Contract.Requires(null != user);

            this.currentUser = user;
        }

        public bool HasPermission(ApplicationUser user)
        {
            Contract.Requires(null != user, "|400|");
            if (currentUser.IsAdmin)
            {
                return true;
            }
            return user.Id == currentUser.Id;
        }

        public bool HasPermission(Coffee coffee)
        {
            Contract.Requires(null != coffee, "|400|");

            return currentUser.IsAdmin;
        }

        public bool HasPermission(CoffeeOrder coffeeOrder)
        {
            Contract.Requires(null != coffeeOrder, "|400|");
            if (currentUser.IsAdmin)
            {
                return true;
            }
            return currentUser.Id == coffeeOrder.UserId;
        }
    }
}