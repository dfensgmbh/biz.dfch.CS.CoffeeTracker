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
using System.Linq;
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

        public IQueryable<ApplicationUser> GetUsers()
        {
            return db.ApplicationUsers;
        }

        public ApplicationUser GetUser(string name)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(name), "|400|");

            return db.ApplicationUsers.FirstOrDefault(u => u.Name == name);
        }

        public ApplicationUser GetUser(long id)
        {
            Contract.Requires(0 < id, "|404|");

            return db.ApplicationUsers.FirstOrDefault(u => u.Id == id);
        }

        public IQueryable<ApplicationUser> GetUserAsQueryable(long id)
        {
            Contract.Requires(0 < id, "|404|");

            return db.ApplicationUsers.Where(u => u.Id == id);
        }

        public ApplicationUser GetCurrentUser(ODataController controller)
        {
            Contract.Requires(null != controller);

            var currentUserName = controller.User.Identity.Name;
            return GetUser(currentUserName);
        }

        public ApplicationUser CreateAndPersistUser(ApplicationUser user)
        {
            Contract.Requires(null != user, "|400|");

            db.ApplicationUsers.Add(user);
            db.SaveChanges();
            return GetUser(user.Name);
        }

        public ApplicationUser UpdateUser(long id, ApplicationUser update)
        {
            Contract.Requires(0 < id, "|404|");
            Contract.Requires(null != update, "|400|");

            var user = GetUser(id);
            Contract.Assert(null != user);

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

            db.ApplicationUsers.Remove(user);
            db.SaveChanges();
        }
    }
}