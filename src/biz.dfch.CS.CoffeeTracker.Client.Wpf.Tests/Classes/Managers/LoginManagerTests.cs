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
using System.Threading.Tasks;
using biz.dfch.CS.CoffeeTracker.Client.Wpf.Classes;
using biz.dfch.CS.CoffeeTracker.Client.Wpf.Managers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Telerik.JustMock;

namespace biz.dfch.CS.CoffeeTracker.Client.Wpf.Tests.Classes.Managers
{
    [TestClass]
    public class LoginManagerTests
    {
        [TestMethod]
        public async Task LoginManagerLoginSucceeds()
        {
            // Arrange
            var fakeReference = Mock.Create<CoffeeTrackerClientWpfServiceContext>();
            Mock.Arrange(
                    () =>
                        fakeReference.authenticationHelper.ReceiveAndSetToken(Arg.AnyString, Arg.AnyString))
                .DoNothing()
                .MustBeCalled("ReceiveAndSetToken was not called");

            // Act
            var sut = new LoginManager(fakeReference);
            await sut.Login(SharedTestData.UserWhichExists, SharedTestData.UserNameWhichShouldNotExist);

            // Assert
            Mock.Assert(fakeReference);
        }

        [TestMethod]
        public async Task LoginManagerOnLoginFailedReturnsFalse()
        {
            // Arrange
            var mockedReference = Mock.Create<CoffeeTrackerClientWpfServiceContext>();
            Mock.Arrange(
                    () =>
                        mockedReference.authenticationHelper.ReceiveAndSetToken(Arg.AnyString, Arg.AnyString))
                .Throws(new Exception())
                .OccursOnce();

            var sut = new LoginManager(mockedReference);

            // Act
            var result = await sut.Login(SharedTestData.UserWhichExists, SharedTestData.PasswordForUserWhichExists);

            // Assert
            Assert.IsFalse(result);
            Mock.Assert(mockedReference);
        }
    }
}