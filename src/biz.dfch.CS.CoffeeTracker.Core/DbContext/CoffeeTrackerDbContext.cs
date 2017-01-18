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
using System.Data.Entity;
using System.Linq;
using System.Web;
using biz.dfch.CS.CoffeeTracker.Core.Model;

namespace biz.dfch.CS.CoffeeTracker.Core.DbContext
{
    public class CoffeeTrackerDbContext : System.Data.Entity.DbContext
    {
        private const string CONNECTION_STRING_NAME = "name=CoffeeTrackerDbContext";

        public CoffeeTrackerDbContext() : base(CONNECTION_STRING_NAME)
        {
            
        }

        // Registration of database tables
        public DbSet<CoffeeOrder> CoffeeMachines { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Coffee> Statistics { get; set; }
    }
}
