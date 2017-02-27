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
using System.Net.Http;
using System.Security.Claims;
using System.Web;
using System.Web.Http.OData;
using biz.dfch.CS.CoffeeTracker.Core.DbContext;
using biz.dfch.CS.CoffeeTracker.Core.Model;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace biz.dfch.CS.CoffeeTracker.Core.Managers
{
    public class ApplicationUserManager : UserManager<IdentityUser>
    {
        private CoffeeTrackerDbContext db = new CoffeeTrackerDbContext();

        public ApplicationUserManager(IUserStore<IdentityUser> store) : base(store)
        {
        }

        public ApplicationUser GetUser(string name)
        {
            return db.ApplicationUsers.FirstOrDefault(u => u.Name == name);
        }

        public ApplicationUser GetUser(long id)
        {
            return db.ApplicationUsers.FirstOrDefault(u => u.Id == id);
        }

        public IQueryable<ApplicationUser> GetUserAsQueryable(long id)
        {
            return db.ApplicationUsers.Where(u => u.Id == id);
        }

        public ApplicationUser GetCurrentUser(ODataController controller)
        {
            /*var currentUserId = HttpContext.Current.User.Identity.GetUserId();
            var currentUserName = HttpContext.Current.User.Identity.Name;
            return db.ApplicationUsers.FirstOrDefault(u => u.Name == currentUserName);*/

            var currentUserName = controller.User.Identity.Name;
            return GetUser(currentUserName);
        }

        public ApplicationUser CreateAndPersistUser(ApplicationUser user)
        {
            db.ApplicationUsers.Add(user);
            db.SaveChanges();
            return GetUser(user.Name);
        }
    }
}