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
using System.Web.Http.OData;
using biz.dfch.CS.CoffeeTracker.Core.Controllers;
using biz.dfch.CS.CoffeeTracker.Core.Model;
using biz.dfch.CS.Testing.Attributes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace biz.dfch.CS.CoffeeTracker.Core.Tests.Controllers
{
    [TestClass]
    public class CoffeeOrdersControllerTest
    {
        CoffeeOrdersController sut = new CoffeeOrdersController();
        private const long INVALID_ID = 0;

        [TestMethod]
        [ExpectContractFailure(MessagePattern = "key")]
        public void CoffeeOrdersControllerCodeContractsGetCoffeeOrderInvalidIdFails()
        {
            // Arrange
            // N/A

            // Act/Assert
            sut.GetCoffeeOrder(INVALID_ID);
        }

        [TestMethod]
        [ExpectContractFailure(MessagePattern = "key")]
        public void CoffeeOrdersControllerCodeContractsGetCoffeeInvalidIdFails()
        {
            // Arrange
            // N/A

            // Act/Assert
            sut.GetCoffee(INVALID_ID);
        }

        [TestMethod]
        [ExpectContractFailure(MessagePattern = "key")]
        public void CoffeeOrdersControllerCodeContractsGetUserInvalidIdFails()
        {
            // Arrange
            // N/A

            // Act/Assert
            sut.GetUser(INVALID_ID);
        }

        [TestMethod]
        [ExpectContractFailure(MessagePattern = "coffeeOrder")]
        public async Task CoffeeOrdersControllerCodeContractsPostNullCoffeeOrderFails()
        {
            // Arrange
            // N/A

            // Act/Assert
            await sut.Post(null);
        }

        [TestMethod]
        [ExpectContractFailure(MessagePattern = "key")]
        public async Task CoffeeOrdersControllerCodeContractsDeleteInvalidIdFails()
        {
            // Arrange
            // N/A

            // Act/Assert
            await sut.Delete(INVALID_ID);
        }

        [TestMethod]
        [ExpectContractFailure(MessagePattern = "key")]
        public async Task CoffeeOrdersControllerCodeContractsPatchInvalidIdFails()
        {
            // Arrange
            // N/A

            // Act/Assert
            await sut.Patch(INVALID_ID, new Delta<CoffeeOrder>());
        }

        [TestMethod]
        [ExpectContractFailure(MessagePattern = "patch")]
        public async Task CoffeeOrdersControllerCodeContractsPatchNullDeltaFails()
        {
            // Arrange
            // N/A

            // Act/Assert
            await sut.Patch(42, null);
        }

        [TestMethod]
        [ExpectContractFailure(MessagePattern = "key")]
        public async Task CoffeeOrdersControllerCodeContractsPutInvalidIdFails()
        {
            // Arrange
            // N/A

            // Act/Assert
            await sut.Put(INVALID_ID, new CoffeeOrder());
        }

        [TestMethod]
        [ExpectContractFailure(MessagePattern = "modified")]
        public async Task CoffeeOrdersControllerCodeContractsPutNullObjectFails()
        {
            // Arrange
            // N/A

            // Act/Assert
            await sut.Put(42, null);
        }
    }
}
