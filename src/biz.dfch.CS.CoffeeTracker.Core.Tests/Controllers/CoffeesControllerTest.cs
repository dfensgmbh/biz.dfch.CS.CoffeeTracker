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

using System.Threading.Tasks;
using System.Web.Http.OData;
using biz.dfch.CS.CoffeeTracker.Core.Controllers;
using biz.dfch.CS.CoffeeTracker.Core.Model;
using biz.dfch.CS.Testing.Attributes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace biz.dfch.CS.CoffeeTracker.Core.Tests.Controllers
{
    /// <summary>
    /// If a name of a test contains "fails" means the test should throw an exception (mostly contract exceptions in this case)
    /// </summary>
    [TestClass]
    public class CoffeesControllerTest
    {
        CoffeesController sut = new CoffeesController();
        private const long INVALID_ID = 0;

        [TestMethod]
        [ExpectContractFailure(MessagePattern = "key")]
        public void CoffeesControllerCodeContractsGetCoffeeInvalidKeyFails()
        {
            // Arrange
            // N/A

            // Act / Assert
            sut.GetCoffee(INVALID_ID);
        }

        [TestMethod]
        [ExpectContractFailure(MessagePattern = "key")]
        public async Task CoffeesControllerCodeContractsPatchCoffeeInvalidKeyFails()
        {
            // Arrange
            // N/A

            // Act / Assert
            sut.Patch(INVALID_ID, new Delta<Coffee>());
        }

        [TestMethod]
        [ExpectContractFailure(MessagePattern = "patch")]
        public async Task CoffeesControllerCodeContractsPatchCoffeeDeltaNullFails()
        {
            // Arrange
            // N/A

            // Act / Assert
            sut.Patch(42, null);
        }

        [TestMethod]
        [ExpectContractFailure(MessagePattern = "key")]
        public async Task CoffeesControllerCodeContractsPutCoffeeInvalidKeyFails()
        {
            // Arrange
            // N/A

            // Act / Assert
            await sut.Put(INVALID_ID, new Coffee());
        }

        [TestMethod]
        [ExpectContractFailure(MessagePattern = "modifiedCoffee")]
        public async Task CoffeesControllerCodeContractsPutCoffeeNullFails()
        {
            // Arrange
            // N/A

            // Act / Assert
            await sut.Put(42, null);
        }

        [TestMethod]
        [ExpectContractFailure(MessagePattern = "key")]
        public async Task CoffeesControllerCodeContractsDeleteCoffeeInvalidKeyFails()
        {
            // Arrange
            // N/A

            // Act / Assert
            await sut.Delete(INVALID_ID);
        }

        [TestMethod]
        [ExpectContractFailure(MessagePattern = "coffee")]
        public async Task CoffeesControllerCodeContractsPostCoffeeWithNullFails()
        {
            // Arrange
            // N/A

            // Act / Assert
            await sut.Post(null);
        }
    }
}
