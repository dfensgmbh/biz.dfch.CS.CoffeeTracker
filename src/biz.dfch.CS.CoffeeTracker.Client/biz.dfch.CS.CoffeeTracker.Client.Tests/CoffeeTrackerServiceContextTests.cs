using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using biz.dfch.CS.Testing.Attributes;
using Microsoft.Data.OData;

namespace biz.dfch.CS.CoffeeTracker.Client.Tests
{
    [TestClass]
    public class CoffeeTrackerServiceContextTests
    {
        private string uri = "http://CoffeeTracker/";
        private string adminUserName = "steven.pilatschek@d-fens.net";
        private string adminPassword = "123456";
        private const string INVALID_USERNAME = "notAnEmail";
        private const string INVALID_PASSWORD = "0000";


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
        [ExpectContractFailure]
        public async Task TryInstantiateWithInvalidUsernameAndPasswordThrows()
        {
            // Arrange
            var sut = new CoffeeTrackerServiceContext(uri);

            // Act / Assert
            await sut.authenticationHelper.ReceiveAndSetToken(INVALID_USERNAME, INVALID_PASSWORD);
        }

        // If this test works, the client is able to send CRUD requests
        [TestMethod]
        public void CreateReadUpdateAndDeleteUser()
        {
            // Arrange
            var userName = string.Format("ClientIntegrationTest-{0}@Example.com", Guid.NewGuid());
            var newUserName = string.Format("ClientIntegrationTest-{0}@Example.com", Guid.NewGuid());
            var password = "123456";

            var user = new CoffeeTrackerService.ApplicationUser
            {
                AspNetUserId = string.Empty,
                Name = userName,
                Password = password
            };

            // Act

            // / Create
            var sut = new CoffeeTrackerServiceContext(uri);
            Assert.IsNotNull(sut);

            sut.container.AddToUsers(user);
            sut.container.SaveChanges();

            // Login into the api
            sut.authenticationHelper.ReceiveAndSetToken(userName, password).Wait();

            // / Read
            // oData Client can't resolve FirstOrDefault() directly
            var createdUserFromDb = sut.container.Users.Where(u => u.Name == userName).FirstOrDefault();
            Assert.IsNotNull(createdUserFromDb);

            // / Update
            createdUserFromDb.Name = newUserName;
            sut.container.SaveChanges();

            // oData Client can't resolve FirstOrDefault() directly
            createdUserFromDb = sut.container.Users.Where(u => u.Name == userName).FirstOrDefault();
            Assert.IsNotNull(createdUserFromDb);
            Assert.AreEqual(newUserName, createdUserFromDb.Name);

            // / Delete
            sut.container.DeleteObject(createdUserFromDb);
            sut.container.SaveChanges();

            var userShouldBeNull = sut.container.Users.Where(u => u.Name == newUserName);
            Assert.IsNull(userShouldBeNull);
        }
    }
}
