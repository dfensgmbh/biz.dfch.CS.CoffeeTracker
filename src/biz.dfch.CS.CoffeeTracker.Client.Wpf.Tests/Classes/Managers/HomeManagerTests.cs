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

using biz.dfch.CS.CoffeeTracker.Client.CoffeeTrackerService;
using biz.dfch.CS.CoffeeTracker.Client.Wpf.Classes;
using biz.dfch.CS.CoffeeTracker.Client.Wpf.Managers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Telerik.JustMock;

namespace biz.dfch.CS.CoffeeTracker.Client.Wpf.Tests.Managers
{
    [TestClass]
    public class HomeManagerTests
    {
        [TestMethod]
        public void HomeManagerAddOrderSucceeds()
        {
            // Arrange
            const int ARBITRARY_COFFEEID = 1;
            var mockedContext = Mock.Create<CoffeeTrackerClientWpfServiceContext>();
            var testCoffeeOrder = new CoffeeOrder()
            {
                CoffeeId = ARBITRARY_COFFEEID,
                UserId = 1,
                Name = "ArbitraryName"
            };
            Mock.Arrange(() => mockedContext.AddToCoffeeOrders(testCoffeeOrder)).DoNothing().MustBeCalled();
            Mock.Arrange(() => mockedContext.SaveChanges()).DoNothing().MustBeCalled();

            // Act
            var sut = new HomeManager(mockedContext);
            sut.AddCoffeeOrder(ARBITRARY_COFFEEID);

            // Assert
            Mock.Assert(mockedContext);
        }
    }
}
