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
using System.Threading.Tasks;
using biz.dfch.CS.CoffeeTracker.Core.DbContext;
using biz.dfch.CS.CoffeeTracker.Core.Managers;
using biz.dfch.CS.CoffeeTracker.Core.Model;
using biz.dfch.CS.CoffeeTracker.Core.Validation;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace biz.dfch.CS.CoffeeTracker.Core.Security
{
    public class AuthorizationManager : IDisposable
    {
        private readonly CoffeeTrackerDbContext db;
        private readonly ApplicationUserManager userManager;

        public AuthorizationManager()
        {
            db = new CoffeeTrackerDbContext();
            userManager = new ApplicationUserManager(new UserStore<IdentityUser>(new CoffeeTrackerDbContext()));

            userManager.UserValidator = new UserValidator<IdentityUser>(userManager)
            {
                AllowOnlyAlphanumericUserNames = false
            };
        }

        public async Task<IdentityResult> RegisterUser(ApplicationUser applicationUser)
        {
            Contract.Requires(null != applicationUser, "|400|");
            // Check if User does not already exist
            Contract.Requires(!new ApplicationUserValidator(applicationUser).UserExists(), "|400|");

            var identityUser = new IdentityUser
            {
                UserName = applicationUser.Name
            };

            var result = await userManager.CreateAsync(identityUser, applicationUser.Password);

            // Create Link with application user
            var user = await FindUser(applicationUser.Name, applicationUser.Password);
            applicationUser.AspNetUserId = user.Id;
            applicationUser.Password = userManager.PasswordHasher.HashPassword(applicationUser.Password);
            userManager.CreateAndPersistUser(applicationUser);

            return result;
        }

        public async Task<IdentityUser> FindUser(string userName, string password)
        {
            var user = await userManager.FindAsync(userName, password);

            return user;
        }

        public void Dispose()
        {
            db.Dispose();
            userManager.Dispose();
        }
    }
}