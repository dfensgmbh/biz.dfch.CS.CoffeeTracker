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
using System.Web.Http.OData;
using biz.dfch.CS.CoffeeTracker.Core.Controllers;
using biz.dfch.CS.CoffeeTracker.Core.DbContext;
using biz.dfch.CS.CoffeeTracker.Core.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace biz.dfch.CS.CoffeeTracker.Core.Tests.Controllers
{
    [TestClass]
    public class CoffeesControllerTest
    {
        private readonly Coffee testCoffee = new Coffee
        {
            Name = "Test-Coffee",
            Brand = "Test-Brand",
            LastDelivery = DateTimeOffset.Now,
            Stock = 42
        };

        [TestMethod]
        public void CoffeesControlerPostSucceeds()
        {
            // Arrange
            var sut = new CoffeesController();
            
            // Act
            var awaitResult = sut.Post(testCoffee);

            var result = GetTestCoffeeFromDb();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(testCoffee.Name, result.Name);
            Assert.AreEqual(testCoffee.Brand, result.Brand);
            Assert.AreEqual(testCoffee.LastDelivery, result.LastDelivery);
            Assert.AreEqual(testCoffee.Stock, result.Stock);

            using (var dbContext = new CoffeeTrackerDbContext())
            {
                dbContext.Coffees.Remove(testCoffee);
                dbContext.SaveChanges();
            }
        }

        [TestMethod]
        public void CoffeesControlerPatchSucceeds()
        {
            // Arrange
            var sut = new CoffeesController();
            var awaitResult = sut.Post(testCoffee);
            var updatedName = "Test-Coffee-Updated";

            // Act
            var coffee = GetTestCoffeeFromDb();
            dynamic testCoffeeDelta = new Delta<Coffee>();
            testCoffeeDelta.Name = updatedName;

            sut.Patch(coffee.Id, testCoffeeDelta);
            var result = GetTestCoffeeFromDb();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(updatedName, result.Name);
        }

        public Coffee GetTestCoffeeFromDb()
        {
            Coffee result;
            using (var dbContext = new CoffeeTrackerDbContext())
            {
                result = dbContext.Coffees.FirstOrDefault(c => c.Name == testCoffee.Name);
            }

            return result;
        }
    }
}
