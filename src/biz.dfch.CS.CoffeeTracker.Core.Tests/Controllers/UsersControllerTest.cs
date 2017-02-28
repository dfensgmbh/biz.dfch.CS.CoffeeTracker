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
using biz.dfch.CS.CoffeeTracker.Core.Controllers;
using biz.dfch.CS.CoffeeTracker.Core.Model;
using biz.dfch.CS.Testing.Attributes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace biz.dfch.CS.CoffeeTracker.Core.Tests.Controllers
{
    /// <summary>
    /// Summary description for UsersControllerTest
    /// </summary>
    [TestClass]
    public class UsersControllerTest
    {
        private UsersController sut = new UsersController();
        private const long INVALID_ID = 0;

        [TestMethod]
        [ExpectContractFailure(MessagePattern = "key")]
        public void UsersControllerCodeContractsGetUserInvalidIdFails()
        {
            // Arrange
            // N/A

            // Act/Assert
            sut.Get(INVALID_ID);
        }

        [TestMethod]
        [ExpectContractFailure(MessagePattern = "key")]
        public async Task UsersControllerCodeContractsDeleteUserInvalidIdFails()
        {
            // Arrange
            // N/A

            // Act/Assert
            await sut.Delete(INVALID_ID);
        }

        [TestMethod]
        [ExpectContractFailure(MessagePattern = "applicationUser")]
        public async Task UsersControllerCodeContractsPostUserNullCoffeeFails()
        {
            // Arrange
            // N/A

            // Act/Assert
            await sut.Post(null);
        }

        [TestMethod]
        [ExpectContractFailure(MessagePattern = "key")]
        public async Task UsersControllerCodeContractsPatchUserInvalidIdFails()
        {
            // Arrange
            // N/A

            // Act/Assert
            await sut.Patch(INVALID_ID, new ApplicationUser());
        }


        [TestMethod]
        [ExpectContractFailure(MessagePattern = "modified")]
        public async Task UsersControllerCodeContractsPatchUserNullDeltaFails()
        {
            // Arrange
            // N/A

            // Act/Assert
            await sut.Patch(42, null);
        }

        [TestMethod]
        [ExpectContractFailure(MessagePattern = "key")]
        public async Task UsersControllerCodeContractsPutUserInvalidIdFails()
        {
            // Arrange
            // N/A

            // Act/Assert
            await sut.Put(INVALID_ID, new ApplicationUser());
        }


        [TestMethod]
        [ExpectContractFailure(MessagePattern = "modified")]
        public async Task UsersControllerCodeContractsPutUserNullDeltaFails()
        {
            // Arrange
            // N/A

            // Act/Assert
            await sut.Put(42, null);
        }
    }
}
