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

using System.Data.Entity;
using biz.dfch.CS.CoffeeTracker.Core.Model;
using Microsoft.AspNet.Identity.EntityFramework;

namespace biz.dfch.CS.CoffeeTracker.Core.DbContext
{
    public class CoffeeTrackerDbContext : IdentityDbContext<IdentityUser>
    {
        private const string CONNECTION_STRING_NAME_TEMPLATE = "name={0}";

        public CoffeeTrackerDbContext() 
            : base(string.Format(CONNECTION_STRING_NAME_TEMPLATE, nameof(CoffeeTrackerDbContext)))
        {
            
        }

        // Registration of database tables
        public DbSet<CoffeeOrder> CoffeeOrders { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Coffee> Coffees { get; set; }
    }
}
