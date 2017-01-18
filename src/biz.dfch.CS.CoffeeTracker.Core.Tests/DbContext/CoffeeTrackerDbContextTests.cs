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
using System.IO;
using System.Linq;
using biz.dfch.CS.CoffeeTracker.Core.DbContext;
using biz.dfch.CS.CoffeeTracker.Core.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace biz.dfch.CS.CoffeeTracker.Core.Tests.DbContext
{
    [TestClass]
    public class CoffeeTrackerDbContextTests
    {
        [TestMethod]
        [TestCategory(("SkipOnTeamCity"))]
        public void AddUserSucceeds()
        {
            using (var sut = new CoffeeTrackerDbContext())
            {
                var user = new User
                {
                    Name = "Test-User"
                };

                sut.Users.Add(user);
                sut.SaveChanges();

                var result = sut.Users.FirstOrDefault(u => u.Name == user.Name);

                Assert.IsNotNull(result);
                Assert.AreEqual(user.Name, result.Name);
            }
        }
    }
}
