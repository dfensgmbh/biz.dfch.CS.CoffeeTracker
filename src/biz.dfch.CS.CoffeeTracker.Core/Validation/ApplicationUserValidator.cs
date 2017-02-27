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
using biz.dfch.CS.CoffeeTracker.Core.Security;
using Microsoft.AspNet.Identity.EntityFramework;

namespace biz.dfch.CS.CoffeeTracker.Core.Validation
{
    public class ApplicationUserValidator
    {
        private readonly ApplicationUser user;
        private ApplicationUserManager applicationUserManager;

        public ApplicationUserValidator(ApplicationUser user)
        {
            this.user = user;
            applicationUserManager = new ApplicationUserManager(new UserStore<IdentityUser>());
        }

        public bool UserExists()
        {
            var result = applicationUserManager.GetUser(user.Name);
            if (null != result)
            {
                return true;
            }
            return false;
        }
    }
}