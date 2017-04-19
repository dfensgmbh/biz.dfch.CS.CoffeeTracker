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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using biz.dfch.CS.CoffeeTracker.Client.CoffeeTrackerService;

namespace biz.dfch.CS.CoffeeTracker.Client.Wpf.Managers
{
    public class CoffeeSelectorManager
    {
        private readonly CoffeeTrackerServiceContext context;
        private IEnumerable<Coffee> coffees;

        public CoffeeSelectorManager(CoffeeTrackerServiceContext ctx)
        {
            context = ctx;
            coffees = context.Coffees.AsEnumerable();
        }

        public IEnumerable<string> GetCoffeeBrandNames()
        {
            if (coffees.Any())
            {
                context.Detach(context.Coffees);
            }

            coffees = context.Coffees.ToList();
            var allBrands = coffees.Select(coffee => coffee.Brand).ToList().Distinct().AsEnumerable();
            return allBrands.ToList();
        }

        public IEnumerable<Coffee> GetCoffees()
        {
            if (coffees.Any())
            {
                context.Detach(coffees);
            }

            coffees = context.Coffees.AsEnumerable();
            return coffees;
        }

        public IEnumerable<Coffee> GetCoffeesOfBrand(string brandName)
        {
            return coffees.Where(c => c.Brand.Equals(brandName)).AsEnumerable();
        }
    }
}