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
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using biz.dfch.CS.CoffeeTracker.Client.Wpf.Classes;
using biz.dfch.CS.CoffeeTracker.Client.Wpf.Classes.Managers;
using biz.dfch.CS.CoffeeTracker.Client.Wpf.Controls;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Telerik.JustMock;
using Telerik.JustMock.Helpers;

namespace biz.dfch.CS.CoffeeTracker.Client.Wpf.Tests.Classes
{
    [TestClass]
    public class BaseWindowManagerTests
    {
        [TestMethod]
        public void BaseWindowManagerOn401ThrowReturnsToLoginPage()
        {
            // Arrange
            var sut = Mock.Create<BaseWindowManager>();
            var client = Mock.Create<CoffeeTrackerClientWpfServiceContext>();
            
            // arrange mocks
            Mock.Arrange(() => sut.Logout()).DoNothing().OccursOnce();
            Mock.Arrange(() => client.Users).DoInstead(() =>
            {
                
            });
            
            // arrange events
            //client.OnExceptionalStatusCode += sut.UnauthorizedEventHandler;

            // Act
            // send request and get unauthorized since it has no bearer token
            client.Users.ToList();

            // Assert
        }
    }
}
