using System;
using System.Linq;
using biz.dfch.CS.CoffeeTracker.Core.DbContext;
using biz.dfch.CS.CoffeeTracker.Core.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace biz.dfch.CS.CoffeeTracker.Core.Tests.DbContext
{
    [TestClass]
    public class CoffeeTrackerDbContextTests
    {
        [TestMethod]
        [TestCategory(("SkipOnTeamCity"))]
        public void AddUserSucceeds()
        {

            using (var sut = new CoffeeTrackerDbContext())
            {
                
                var user = new User
                {
                    Name = "Test-User"
                };

                sut.Users.Add(user);
                sut.SaveChanges();

                var result = sut.Users.Find(user.Name);

                Assert.IsNotNull(result);
                Assert.Equals(result.Name, user.Name);
            }
        }
    }
}
