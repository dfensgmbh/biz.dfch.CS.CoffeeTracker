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
using System.IO;
using System.Linq;
using biz.dfch.CS.CoffeeTracker.Client.CoffeeTrackerService;

namespace biz.dfch.CS.CoffeeTracker.Client.Wpf.Tests
{
    public static class SharedTestData
    {
        public static readonly string ProjectName = "biz.dfch.CS.CoffeeTracker.Client.Wpf";
        public static readonly string UserWhichExists = "steven.pilatschek@d-fens.net";
        public static readonly string PasswordForUserWhichExists = "123456";
        public static readonly string UserNameWhichShouldNotExist = "NotExistentName@existent.com";

        #region Example Coffees, Users and CoffeeOrders

        private const string EXAMPLE_USER_NAME = "Example@ex.com";
        private const string SECOND_EXAMPLE_USER_NAME = "Example@ex.com";
        private const string EXAMPLE_COFFEE_NAME = "ExampleCoffee";
        private const string EXAMPLE_COFFEE_BRAND = "ExampleBrand";
        private const string SECOND_EXAMPLE_COFFEE_NAME = "SecondExampleCoffee";
        private const string SECOND_EXAMPLE_COFFEE_BRAND = "SecondExampleBrand";

        public static ApplicationUser ExampleApplicationUser
        {
            get
            {
                var user = new ApplicationUser();
                user.Name = EXAMPLE_USER_NAME;
                user.Id = 1;
                return user;
            }
        }

        public static ApplicationUser SecondExampleApplicationUser
        {
            get
            {
                var user = new ApplicationUser();
                user.Name = SECOND_EXAMPLE_USER_NAME;
                user.Id = 2;
                return user;
            }
        }

        public static Coffee ExampleCoffee
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

        public static Coffee SecondExampleCoffee
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

        public static IEnumerable<CoffeeOrder> GetCoffeeOrdersExample()
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

            return coffeeOrders.AsEnumerable();
        }

        #endregion

        public static readonly string InvalidPassword = "InvPa";
            //InvPa = InvalidPassword, it contains 5 characters while the password needs at least 6, so it's invalid

        public static string ExecutablePath
        {
            get
            {
                // create path to executable file in biz.dfch.CS.CoffeeTracker.Client.Wpf/bin/Debug
                var baseDirectory = AppContext.BaseDirectory;
                var toReplace = string.Format("{0}{1}", ProjectName, ".Tests");
                var newBaseDirectory = baseDirectory.Replace(toReplace, ProjectName);
                return Path.Combine(newBaseDirectory, ProjectName + ".exe");
            }
        }
    }
}