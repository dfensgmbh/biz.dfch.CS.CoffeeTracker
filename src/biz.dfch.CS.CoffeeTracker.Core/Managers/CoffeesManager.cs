﻿/**
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
using System.Configuration;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Web.Http.OData;
using biz.dfch.CS.CoffeeTracker.Core.DbContext;
using biz.dfch.CS.CoffeeTracker.Core.Model;
using biz.dfch.CS.CoffeeTracker.Core.Security.PermissionChecker;
using biz.dfch.CS.CoffeeTracker.Core.Stores;

namespace biz.dfch.CS.CoffeeTracker.Core.Managers
{
    public class CoffeesManager
    {
        private readonly CoffeeTrackerDbContext db;

        private readonly Lazy<EmailManager> emailManagerLazy = new Lazy<EmailManager>(() => new EmailManager());
        private EmailManager EmailManager => emailManagerLazy.Value;
        private readonly PermissionChecker permissionChecker;

        private static readonly CoffeeStockWarningConfigurationSection _coffeeStockWarningConfigurationSection
            = (CoffeeStockWarningConfigurationSection)ConfigurationManager.GetSection("coffeeStockWarningConfigurationSection");


        public CoffeesManager()
        {
            db = new CoffeeTrackerDbContext();
            var currentUser = new ApplicationUserManager(new AppUserStore()).GetCurrentUser();
            permissionChecker = new PermissionChecker(currentUser);
        }

        public IQueryable<Coffee> Get()
        {
            return db.Coffees;
        }

        public Coffee Get(long key)
        {
            Contract.Requires(0 < key, "|404|");

            return db.Coffees.FirstOrDefault(c => c.Id == key);
        }

        public void DecreaseStock(long key)
        {
            Contract.Requires(0 < key, "|404|");

            var coffee = db.Coffees.FirstOrDefault(c => c.Id == key);
            Contract.Assert(null != coffee, "|404|");
            Contract.Assert(0 < coffee.Stock, "|400|");

            coffee.Stock--;

            db.SaveChanges();
            CheckStock(coffee.Id);
        }

        public void CheckStock(long key)
        {
            Contract.Requires(0 < key, "|404|");
            var coffee = db.Coffees.FirstOrDefault(c => c.Id == key);
            Contract.Assert(null != coffee, "|404|");

            var adminMails = new ApplicationUserManager(new AppUserStore()).GetAdminEmailAddresses();

            if (int.Parse(_coffeeStockWarningConfigurationSection.CoffeeStock) >= coffee.Stock)
            {
                EmailManager.CreateAndSendOutOfStockEmail(adminMails, coffee);
            }
        }

        public Coffee Get(string name, string brand)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(name), "|404|");

            return db.Coffees.FirstOrDefault(c => c.Name == name && c.Brand == brand);
        }

        public IQueryable<Coffee> GetAsQueryable(long key)
        {
            Contract.Requires(0 < key, "|404|");

            return db.Coffees.Where(c => c.Id == key);
        }

        public Coffee Update(long id, Coffee modifiedCoffee)
        {
            Contract.Requires(null != modifiedCoffee, "|400|");
            Contract.Requires(0 < id, "|404|");

            var hasPermission = permissionChecker.HasPermission(modifiedCoffee);
            Contract.Assert(hasPermission, "|403|");

            var isExistingNameAndBrandCombination = Get(modifiedCoffee.Name, modifiedCoffee.Brand) != null;
            Contract.Assert(!isExistingNameAndBrandCombination, "|400|");
            

            var coffee = Get(id);

            coffee.Name = modifiedCoffee.Name;
            coffee.Brand = modifiedCoffee.Brand;
            coffee.LastDelivery = modifiedCoffee.LastDelivery;
            coffee.Price = modifiedCoffee.Price;
            coffee.Stock = modifiedCoffee.Stock;

            db.SaveChanges();

            CheckStock(coffee.Id);

            return Get(coffee.Id);
        }

        public Coffee Update(long id, Delta<Coffee> patch)
        {
            var coffee = Get(id);
            var modifiedCoffee = patch.GetEntity();
            var changedPropertynames = patch.GetChangedPropertyNames();

            if (!coffee.Name.Equals(modifiedCoffee.Name) || !coffee.Brand.Equals(modifiedCoffee.Brand))
            {
                var isExistingNameAndBrandCombination = Get(modifiedCoffee.Name, modifiedCoffee.Brand) != null;
                Contract.Assert(!isExistingNameAndBrandCombination, "|400|");
            }

            var hasPermission = permissionChecker.HasPermission(modifiedCoffee);
            Contract.Assert(hasPermission, "|403|");

            if (changedPropertynames.Contains(nameof(Coffee.Name)))
            {
                coffee.Name = modifiedCoffee.Name;
            }
            if (changedPropertynames.Contains(nameof(Coffee.Brand)))
            {
                coffee.Brand = modifiedCoffee.Brand;
            }
            if (changedPropertynames.Contains(nameof(Coffee.Price)))
            {
                coffee.Price = modifiedCoffee.Price;
            }
            if (changedPropertynames.Contains(nameof(Coffee.Stock)))
            {
                coffee.Stock = modifiedCoffee.Stock;
            }
            if (changedPropertynames.Contains(nameof(Coffee.LastDelivery)))
            {
                coffee.LastDelivery = modifiedCoffee.LastDelivery;
            }

            db.SaveChanges();

            CheckStock(coffee.Id);

            return coffee;
        }

        public Coffee Create(Coffee coffee)
        {
            Contract.Requires(null != coffee, "|400|");

            var hasPermission = permissionChecker.HasPermission(coffee);
            Contract.Assert(hasPermission, "|403|");

            db.Coffees.Add(coffee);
            db.SaveChanges();

            return Get(coffee.Name, coffee.Brand);
        }

        public void Delete(long id)
        {
            Contract.Requires(0 < id, "|404|");

            var hasPermission = permissionChecker.HasPermission(Get(id));
            Contract.Assert(hasPermission, "|403|");

            Delete(Get(id));
        }

        public void Delete(Coffee coffee)
        {
            Contract.Requires(null != coffee, "|400|");

            var hasPermission = permissionChecker.HasPermission(coffee);
            Contract.Assert(hasPermission, "|403|");

            db.Coffees.Remove(coffee);
            db.SaveChanges();
        }
    }
}