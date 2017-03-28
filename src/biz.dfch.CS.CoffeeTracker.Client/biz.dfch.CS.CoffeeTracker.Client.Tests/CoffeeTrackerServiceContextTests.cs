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
            sut.Coffees.Where(c => c.Id == id).FirstOrDefault();
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

            // / Create
            var sut = new CoffeeTrackerServiceContext(uri);
            Assert.IsNotNull(sut);

            sut.AddToUsers(user);
            sut.SaveChanges();

            // Login into the api
            sut.authenticationHelper.ReceiveAndSetToken(userName, password).Wait();

            // / Read
            // oData Client can't resolve FirstOrDefault() directly
            var createdUserFromDb = sut.Users.Where(u => u.Name == userName).FirstOrDefault();
            Assert.IsNotNull(createdUserFromDb);

            // / Update
            createdUserFromDb.Name = newUserName;
            sut.SaveChanges();

            // oData Client can't resolve FirstOrDefault() directly
            createdUserFromDb = sut.Users.Where(u => u.Name == userName).FirstOrDefault();
            Assert.IsNotNull(createdUserFromDb);
            Assert.AreEqual(newUserName, createdUserFromDb.Name);

            // / Delete
            sut.DeleteObject(createdUserFromDb);
            sut.SaveChanges();

            // Assert deletion
            // Since the old user should be deleted, the client needs a new token, and an admin token because
            // the client is requesting users
            sut.authenticationHelper.ReceiveAndSetToken(adminUserName, adminPassword).Wait();

            var userShouldBeNull = sut.Users.Where(u => u.Name == newUserName).FirstOrDefault();
            Assert.IsNull(userShouldBeNull);
        }
    }
}
