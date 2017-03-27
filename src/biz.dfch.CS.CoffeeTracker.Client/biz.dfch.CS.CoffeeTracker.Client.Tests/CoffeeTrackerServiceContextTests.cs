using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace biz.dfch.CS.CoffeeTracker.Client.Tests
{
    [TestClass]
    public class CoffeeTrackerServiceContextTests
    {
        private string uri = "http://localhost:49270/";
        private string adminUserName = "steven.pilatschek@d-fens.net";
        private string adminPassword = "123456";

        [TestMethod]
        public void GetCoffeeSucceeds()
        {
            // Arrange
            // Test assumes coffee is existing
            var id = 1183;

            // Act
            var sut = new CoffeeTrackerServiceContext(uri, adminUserName, adminPassword);
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
            sut.container.Coffees.Where(c => c.Id == id).FirstOrDefault();
        }
    }
}
