using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using biz.dfch.CS.Testing.Attributes;

namespace biz.dfch.CS.CoffeeTracker.Client.Tests
{
    [TestClass]
    public class CoffeeTrackerServiceContextTests
    {
        private string uri = "http://localhost:49270/";
        private string adminUserName = "steven.pilatschek@d-fens.net";
        private string adminPassword = "123456";
        private const string INVALID_USERNAME = "notAnEmail";
        private const string INVALID_PASSWORD = "0000";

        [TestMethod]
        public void GetCoffeeSucceeds()
        {
            // Arrange
            // Test assumes coffee is existing
            var id = 1183;

            // Act
            var sut = new CoffeeTrackerServiceContext(uri, adminUserName, adminPassword);
            // oData Client can't resolve FirstOrDefault() directly
            var result = sut.container.Coffees.Where(c => c.Id == id).FirstOrDefault();

            // Assert
            Assert.IsNotNull(sut);
            Assert.IsNotNull(result);
            Assert.AreEqual(id, result.Id);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "token")]
        public void TryRequestWithoutTokenThrowsException()
        {
            // Arrange
            var id = 1183;

            // Act / Assert
            var sut = new CoffeeTrackerServiceContext(uri);
            // oData Client can't resolve FirstOrDefault() directly
            sut.container.Coffees.Where(c => c.Id == id).FirstOrDefault();
        }

        [TestMethod]
        [ExpectContractFailure(MessagePattern = "Bad request")]
        public void TryInstantiateWithInvalidUsernameAndPasswordThrows()
        {
            // Arrange
            var sut = new CoffeeTrackerServiceContext(uri);

            // Act / Assert
            sut.authenticationHelper.ReceiveAndSetToken(INVALID_USERNAME, INVALID_PASSWORD).Wait();
        }
    }
}
