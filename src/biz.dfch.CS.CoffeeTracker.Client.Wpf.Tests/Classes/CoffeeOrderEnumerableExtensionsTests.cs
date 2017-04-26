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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using biz.dfch.CS.CoffeeTracker.Client.Wpf.Classes.Managers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace biz.dfch.CS.CoffeeTracker.Client.Wpf.Tests.Classes
{
    [TestClass]
    public class CoffeeOrderEnumerableExtensionsTests
    {
        [TestMethod]
        public void ExtensionCoffeeOrdersApplyUserReturnsListWithSpecifiedUserOnly()
        {
            // Arrange
            var coffeeOrders = SharedTestData.GetCoffeeOrdersExample();

            // Act
            var result = coffeeOrders.ApplyUserFilter(SharedTestData.ExampleApplicationUser);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.All(c => c.ApplicationUser.Name.Equals(SharedTestData.ExampleApplicationUser.Name)));
        }

        [TestMethod]
        public void ExtensionCoffeeOrdersApplyCoffeeReturnsListWithSpecifiedCoffeeOnly()
        {
            // Arrange
            var coffeeOrders = SharedTestData.GetCoffeeOrdersExample();

            // Act
            var result = coffeeOrders.ApplyCoffeeFilter(SharedTestData.ExampleCoffee);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.All(c => c.Coffee.Name.Equals(SharedTestData.ExampleCoffee.Name)));
            Assert.IsTrue(result.All(c => c.Coffee.Brand.Equals(SharedTestData.ExampleCoffee.Brand)));
        }

        [TestMethod]
        public void ExtensionCoffeeOrdersApplyTimeReturnsCoffeeOrdersCreatedInSpecifiedTimeLineOnly()
        {
            // Arrange

            // Act

            // Assert
        }
    }
}
