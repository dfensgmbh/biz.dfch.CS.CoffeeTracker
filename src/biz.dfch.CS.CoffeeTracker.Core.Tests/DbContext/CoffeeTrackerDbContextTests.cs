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
using System.Linq;
using biz.dfch.CS.CoffeeTracker.Core.DbContext;
using biz.dfch.CS.CoffeeTracker.Core.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace biz.dfch.CS.CoffeeTracker.Core.Tests.DbContext
{
    [TestClass]
    public class CoffeeTrackerDbContextTests
    {
        private static readonly ApplicationUser _testApplicationUser = new ApplicationUser("Test-ApplicationUser",
            "123456", "example@example.com");

        private static readonly Coffee _testCoffee = new Coffee
        {
            Name = "Test-Coffee",
            Brand = "Test-Brand",
            LastDelivery = DateTime.Now,
            Price = 2.50M,
            Stock = 42
        };

        private static readonly CoffeeOrder _testCoffeeOrder = new CoffeeOrder()
        {
            Name = "Test-CoffeeOrder",
            Created = DateTime.Now,
            Coffee = _testCoffee,
            CoffeeId = _testCoffee.Id,
            ApplicationUser = _testApplicationUser,
            UserId = _testApplicationUser.Id
        };


        [TestMethod]
        [TestCategory("SkipOnTeamCity")]
        public void AddUserSucceeds()
        {
            using (var sut = new CoffeeTrackerDbContext())
            {
                sut.ApplicationUsers.Add(_testApplicationUser);
                sut.SaveChanges();

                var result = sut.ApplicationUsers.FirstOrDefault(u => u.Name == _testApplicationUser.Name);

                Assert.IsNotNull(result);
                Assert.AreEqual(_testApplicationUser.Name, result.Name);

                sut.ApplicationUsers.Remove(_testApplicationUser);
                sut.SaveChanges();
            }
        }

        [TestMethod]
        [TestCategory("SkipOnTeamCity")]
        public void AddCoffeeSucceeds()
        {
            using (var sut = new CoffeeTrackerDbContext())
            {
                sut.Coffees.Add(_testCoffee);
                sut.SaveChanges();

                var result = sut.Coffees.FirstOrDefault(c => c.Name == _testCoffee.Name);

                Assert.IsNotNull(result);
                Assert.AreEqual(_testCoffee.Name, result.Name);

                sut.Coffees.Remove(_testCoffee);
                sut.SaveChanges();
            }
        }

        [TestMethod]
        [TestCategory("SkipOnTeamCity")]
        public void AddCoffeeOrderSucceeds()
        {
            using (var sut = new CoffeeTrackerDbContext())
            {
                sut.CoffeeOrders.Add(_testCoffeeOrder);
                sut.SaveChanges();

                var result = sut.CoffeeOrders.FirstOrDefault(c => c.Coffee.LastDelivery == _testCoffee.LastDelivery);

                Assert.IsNotNull(result);
                Assert.AreEqual(_testCoffeeOrder.CoffeeId, result.CoffeeId);

                sut.CoffeeOrders.Remove(_testCoffeeOrder);
                sut.Coffees.Remove(_testCoffee);
                sut.ApplicationUsers.Remove(_testApplicationUser);
                sut.SaveChanges();
            }
        }
    }
}
