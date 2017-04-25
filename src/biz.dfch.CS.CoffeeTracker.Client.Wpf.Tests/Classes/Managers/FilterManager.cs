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
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace biz.dfch.CS.CoffeeTracker.Client.Wpf.Tests.Classes.Managers
{
    [TestClass]
    public class FilterManager
    {
        #region Test related data

        private const string EXAMPLE_USER_NAME = "Example@ex.com";
        private const string SECOND_EXAMPLE_USER_NAME = "Example@ex.com";
        private const string EXAMPLE_COFFEE_NAME = "ExampleCoffee";
        private const string EXAMPLE_COFFEE_BRAND = "ExampleBrand";
        private const string SECOND_EXAMPLE_COFFEE_NAME = "SecondExampleCoffee";
        private const string SECOND_EXAMPLE_COFFEE_BRAND = "SecondExampleBrand";

        private ApplicationUser ExampleApplicationUser
        {
            get
            {
                var user = new ApplicationUser();
                user.Name = EXAMPLE_USER_NAME;
                user.Id = 1;
                return user;
            }
        }

        private ApplicationUser SecondExampleApplicationUser
        {
            get
            {
                var user = new ApplicationUser();
                user.Name = SECOND_EXAMPLE_USER_NAME;
                user.Id = 2;
                return user;
            }
        }

        private Coffee ExampleCoffee
        {
            get
            {
                var coffee = new Coffee
                {
                    Name = EXAMPLE_COFFEE_NAME,
                    Brand = EXAMPLE_COFFEE_BRAND
                };
                return coffee;
            }
        }

        private Coffee SecondExampleCoffee
        {
            get
            {
                var coffee = new Coffee
                {
                    Name = SECOND_EXAMPLE_COFFEE_NAME,
                    Brand = SECOND_EXAMPLE_COFFEE_BRAND
                };
                return coffee;
            }
        }

        private List<CoffeeOrder> GetCoffeeOrdersExample()
        {
            var coffeeOrders = new List<CoffeeOrder>();
            for (var i = 0; i < 10; i++)
            {
                var coffeeOrder = new CoffeeOrder();
                if (i % 2 == 0)
                {
                    coffeeOrder.ApplicationUser = ExampleApplicationUser;
                    coffeeOrder.UserId = coffeeOrder.ApplicationUser.Id;
                    coffeeOrder.Coffee = ExampleCoffee;
                    coffeeOrder.CoffeeId = coffeeOrder.Coffee.Id;
                    coffeeOrder.Id = i;
                }
                else
                {
                    coffeeOrder.ApplicationUser = SecondExampleApplicationUser;
                    coffeeOrder.UserId = coffeeOrder.ApplicationUser.Id;
                    coffeeOrder.Coffee = SecondExampleCoffee;
                    coffeeOrder.CoffeeId = coffeeOrder.Coffee.Id;
                    coffeeOrder.Id = i;
                }

                coffeeOrders.Add(coffeeOrder);
            }
            return coffeeOrders;
        }

        #endregion

        [TestMethod]
        public void FilterManagerApplyUserReturnsListOfCoffeeOrdersWithSpecifiedUser()
        {

        }

    }
}