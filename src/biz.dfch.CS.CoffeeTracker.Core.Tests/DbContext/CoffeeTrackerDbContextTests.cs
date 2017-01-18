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
        private static readonly User TestUser = new User
        {
            Name = "Test-User",
            Password = "1234"
        };

        private static readonly Coffee TestCoffee = new Coffee
        {
            Name = "Test-User",
            Brand = "Test-Brand",
            LastDelivery = DateTime.Now,
            Price = 2.50M,
            Stock = 42
        };

        private static readonly CoffeeOrder TestCoffeeOrder = new CoffeeOrder()
        {
            Created = DateTime.Now,
            Coffee = TestCoffee,
            CoffeeId = TestCoffee.Id,
            User = TestUser,
            UserId = TestUser.Id
        };


        [TestMethod]
        [TestCategory("SkipOnTeamCity")]
        public void AddUserSucceeds()
        {
            using (var sut = new CoffeeTrackerDbContext())
            {
                sut.Users.Add(TestUser);
                sut.SaveChanges();

                var result = sut.Users.FirstOrDefault(u => u.Name == TestUser.Name);

                Assert.IsNotNull(result);
                Assert.AreEqual(TestUser.Name, result.Name);

                sut.Users.Remove(TestUser);
                sut.SaveChanges();
            }
        }

        [TestMethod]
        [TestCategory("SkipOnTeamCity")]
        public void AddCoffeeSucceeds()
        {
            using (var sut = new CoffeeTrackerDbContext())
            {
                sut.Coffees.Add(TestCoffee);
                sut.SaveChanges();

                var result = sut.Coffees.FirstOrDefault(c => c.Name == TestCoffee.Name);

                Assert.IsNotNull(result);
                Assert.AreEqual(TestCoffee.Name, result.Name);

                sut.Coffees.Remove(TestCoffee);
                sut.SaveChanges();
            }
        }

        [TestMethod]
        [TestCategory("SkipOnTeamCity")]
        public void AddCoffeeOrderSucceeds()
        {
            using (var sut = new CoffeeTrackerDbContext())
            {
                
                sut.CoffeeOrders.Add(TestCoffeeOrder);
                sut.SaveChanges();

                var result = sut.CoffeeOrders.FirstOrDefault(c => c.Coffee.LastDelivery == TestCoffee.LastDelivery);

                Assert.IsNotNull(result);
                Assert.AreEqual(TestCoffeeOrder.CoffeeId, result.CoffeeId);

                sut.CoffeeOrders.Remove(TestCoffeeOrder);
                sut.SaveChanges();
            }
        }
    }
}
