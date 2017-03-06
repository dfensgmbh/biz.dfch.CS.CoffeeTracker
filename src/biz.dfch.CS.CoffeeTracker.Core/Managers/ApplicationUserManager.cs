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
using System.Net.Http;
using System.Web;
using System.Web.Http.OData;
using System.Web.Routing;
using biz.dfch.CS.CoffeeTracker.Core.DbContext;
using biz.dfch.CS.CoffeeTracker.Core.Model;
using biz.dfch.CS.CoffeeTracker.Core.Security.PermissionChecker;
using biz.dfch.CS.CoffeeTracker.Core.Stores;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace biz.dfch.CS.CoffeeTracker.Core.Managers
{
    public class ApplicationUserManager : UserManager<IdentityUser>
    {
        private readonly CoffeeTrackerDbContext db = new CoffeeTrackerDbContext();
        internal PermissionChecker PermissionChecker;

        public ApplicationUserManager(AppUserStore store)
            : base(store)
        {
            var currentUser = GetCurrentUser();
            PermissionChecker = new PermissionChecker(currentUser);
        }

        public ApplicationUserManager(AppUserStore store, bool skipPermissionChecks)
            : base(store)
        {
            if (skipPermissionChecks)
            {
                PermissionChecker = new PermissionChecker(skipPermissionChecks);
            }
            else
            {
                var currentUser = GetCurrentUser();
                PermissionChecker = new PermissionChecker(currentUser);
            }
        }

        public IQueryable<ApplicationUser> GetUsers()
        {
            var currentUser = GetCurrentUser();

            return db.ApplicationUsers.Where(u => u.Id == currentUser.Id);
        }

        public ApplicationUser GetUser(string name)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(name), "|400|");

            var user = db.ApplicationUsers.FirstOrDefault(u => u.Name == name);
            Contract.Assert(null != user, "|404|");
            var hasPermission = PermissionChecker.HasPermission(user);
            Contract.Assert(hasPermission, "|403|");

            return user;
        }

        public ApplicationUser GetUser(long id)
        {
            Contract.Requires(0 < id, "|404|");
            var user = db.ApplicationUsers.FirstOrDefault(u => u.Id == id);
            Contract.Assert(null != user, "|404|");
            var hasPermission = PermissionChecker.HasPermission(user);
            Contract.Assert(hasPermission, "|403|");

            return db.ApplicationUsers.FirstOrDefault(u => u.Id == id);
        }

        public IQueryable<ApplicationUser> GetUserAsQueryable(long id)
        {
            Contract.Requires(0 < id, "|404|");
            var user = db.ApplicationUsers.FirstOrDefault(u => u.Id == id);
            Contract.Assert(null != user, "|404|");
            var hasPermission = PermissionChecker.HasPermission(user);
            Contract.Assert(hasPermission, "|403|");

            return db.ApplicationUsers.Where(u => u.Id == id);
        }

        public ApplicationUser GetCurrentUser()
        {
            if (HttpContext.Current != null)
            {
                var currentUserName = HttpContext.Current.User.Identity.Name;
                return db.ApplicationUsers.FirstOrDefault(u => u.Name == currentUserName);
            }
            return null;
        }

        public ApplicationUser CreateAndPersistUser(ApplicationUser user)
        {
            Contract.Requires(null != user, "|400|");
            Contract.Requires(!string.IsNullOrWhiteSpace(user.Name));
            Contract.Requires(!string.IsNullOrWhiteSpace(user.Password));

            db.ApplicationUsers.Add(user);
            db.SaveChanges();
            return user;
        }

        public ApplicationUser UpdateUser(long id, ApplicationUser update)
        {
            Contract.Requires(0 < id, "|404|");
            Contract.Requires(null != update, "|400|");


            var user = GetUser(id);
            Contract.Assert(null != user, "|404|");
            var hasPermission = PermissionChecker.HasPermission(user);
            Contract.Assert(hasPermission, "|403|");

            user.Name = update.Name;
            user.Password = update.Password;

            db.SaveChanges();

            return GetUser(id);
        }

        public void DeleteUser(long id)
        {
            Contract.Requires(0 < id, "|404|");

            var user = GetUser(id);
            Contract.Assert(null != user, "|404|");
            var hasPermission = PermissionChecker.HasPermission(user);
            Contract.Assert(hasPermission, "|403|");

            db.ApplicationUsers.Remove(user);
            db.SaveChanges();

            HttpContext.Current.Request.GetOwinContext().Authentication.SignOut();
        }

        public bool UserExists(ApplicationUser user)
        {
            return UserExists(user.Name);
        }

        public bool UserExists(string name)
        {
            var result = db.Users.FirstOrDefault(u => u.UserName == name);
            return null != result;
        }
    }
}