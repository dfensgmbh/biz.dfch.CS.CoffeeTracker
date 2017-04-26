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
using biz.dfch.CS.CoffeeTracker.Client.CoffeeTrackerService;
using biz.dfch.CS.CoffeeTracker.Client.Wpf.Classes;
using biz.dfch.CS.CoffeeTracker.Client.Wpf.Classes.Managers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Telerik.JustMock.Helpers;

namespace biz.dfch.CS.CoffeeTracker.Client.Wpf.Tests.Classes.Managers
{
    [TestClass]
    public class FilterManagerTests
    {
        [TestMethod]
        public void FilterManagerApplyUserReturnsListOfCoffeeOrdersWithSpecifiedUser()
        {
            // Arrange
            var coffeeOrders = SharedTestData.GetCoffeeOrdersExample();
            //var sut = new FilterManager(coffeeOrders);

            // Act
            //var result = sut.ApplyUserFilter(ExampleApplicationUser);

            // Assert
            //Assert.IsNotNull(result);
            //Assert.IsTrue(result.All(c => c.ApplicationUser == ExampleApplicationUser));
        }

        [TestMethod]
        public void FilterManagerApplyTimeReturnsListOfSpecifiedTime()
        {
            // Arrange


            // Act

            // Assert

        }
    }
}