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
using biz.dfch.CS.CoffeeTracker.Client.Wpf.Controls;

namespace biz.dfch.CS.CoffeeTracker.Client.Wpf.Managers
{
    public class LoginManager
    {
        private CoffeeTrackerServiceContext context;

        public LoginManager(CoffeeTrackerServiceContext ctx)
        {
            Contract.Requires(null != ctx);

            context = ctx;
        }

        public async Task<bool> Login(string email, string password)
        {
            Contract.Requires(!String.IsNullOrWhiteSpace(email));
            Contract.Requires(!String.IsNullOrWhiteSpace(password));

            try
            {
                await ClientContext.CreateServiceContext().authenticationHelper.ReceiveAndSetToken(email, password);
                // ReSharper disable once ReplaceWithSingleCallToFirstOrDefault
                var user = ClientContext.CreateServiceContext().Users.Where(u => u.Name == email).FirstOrDefault();
                Contract.Assert(null != user, email);
                ClientContext.CurrentUserName = user.Name;
                ClientContext.CurrentUserId = user.Id;

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
