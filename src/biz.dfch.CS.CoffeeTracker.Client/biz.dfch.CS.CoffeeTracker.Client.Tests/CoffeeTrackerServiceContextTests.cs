﻿using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace biz.dfch.CS.CoffeeTracker.Client.Tests
{
    [TestClass]
    public class CoffeeTrackerServiceContextTests
    {
        private string uri = "http://localhost:49270/api/";
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
            var result = sut.container.Coffees.FirstOrDefault(c => c.Id == id);

            // Assert
            Assert.IsNotNull(sut);
            Assert.IsNotNull(result);
            Assert.AreEqual(id, result.Id);
        }
    }
}
