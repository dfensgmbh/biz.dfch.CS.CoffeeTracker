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
using System.Text;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace biz.dfch.CS.CoffeeTracker.Client.Tests
{
    /// <summary>
    /// Summary description for AuthenticationHelperTests
    /// </summary>
    [TestClass]
    public class AuthenticationHelperTests
    {
        [TestMethod]
        public async Task ReceiveAndSetTokenSucceeds()
        {
            // Arrange
            // Test assumes username and password exists in database
            var userName = "steven.pilatschek@d-fens.net";
            var password = "123456";

            var hostUri = new Uri("http://localhost:49270/");

            // Act 
            var sut = new AuthenticationHelper(hostUri, userName, password);
            await sut.ReceiveAndSetToken(userName, password);

            // Assert
            Assert.AreNotEqual(String.Empty, sut.tokenUri);
            Assert.AreNotEqual(String.Empty, sut.bearerToken);
        }
    }
}
