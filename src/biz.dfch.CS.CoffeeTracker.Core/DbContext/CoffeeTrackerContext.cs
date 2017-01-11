using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using biz.dfch.CS.CoffeeTracker.Core.Models;

namespace biz.dfch.CS.CoffeeTracker.Core.DbContext
{
    public class CoffeeTrackerContext : System.Data.Entity.DbContext
    {
        private const string CONNECTION_STRING_NAME = "name=CoffeeTrackerContext";

        public CoffeeTrackerContext() : base(CONNECTION_STRING_NAME)
        {
            
        }

        // Registration of database tables
        public DbSet<CoffeeMachine> CoffeeMachines { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Statistic> Statistics { get; set; }
    }
}
