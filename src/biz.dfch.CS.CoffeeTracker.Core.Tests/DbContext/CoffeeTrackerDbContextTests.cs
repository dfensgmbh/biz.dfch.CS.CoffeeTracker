using System;
using System.Linq;
using biz.dfch.CS.CoffeeTracker.Core.DbContext;
using biz.dfch.CS.CoffeeTracker.Core.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace biz.dfch.CS.CoffeeTracker.Core.Tests.DbContext
{
    [TestClass]
    public class CoffeeTrackerDbContextTests
    {
        [TestMethod]
        public void AddUserSucceeds()
        {

            using (var sut = new CoffeeTrackerContext())
            {
                
                var user = new User
                {
                    Id = 1,
                    Name = "Test-User"
                };

                sut.Users.Add(user);
                sut.SaveChanges();

                var users = sut.Users;
                Assert.IsTrue(users.Contains(user));
            }
        }
    }
}
