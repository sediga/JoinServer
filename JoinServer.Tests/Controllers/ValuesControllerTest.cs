using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JoinServer;
using JoinServer.Controllers;
using JoinServer.Models;
using Newtonsoft.Json;

namespace JoinServer.Tests.Controllers
{
    [TestClass]
    public class ValuesControllerTest
    {
        [TestMethod]
        public void Get()
        {
            // Arrange
            LocationController controller = new LocationController();

            // Act
            IEnumerable<string> result = controller.Get();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());
            Assert.AreEqual("value1", result.ElementAt(0));
            Assert.AreEqual("value2", result.ElementAt(1));
        }

        [TestMethod]
        public void GetById()
        {
            // Arrange
            LocationController controller = new LocationController();

            // Act
            string result = controller.Get(5);

            // Assert
            Assert.AreEqual("value", result);
        }

        [TestMethod]
        public void Post()
        {
            // Arrange
            LocationController controller = new LocationController();
            CurrentLocation location = new CurrentLocation() { DeviceID = Guid.NewGuid().ToString(), Lat = 40.482367, Long = -79.697782 };
            // Act
            controller.PostLocation(location);

            // Assert
        }

        [TestMethod]
        public void Put()
        {
            // Arrange
            LocationController controller = new LocationController();

            // Act
            controller.Put(5, "value");

            // Assert
        }

        [TestMethod]
        public void Delete()
        {
            // Arrange
            LocationController controller = new LocationController();

            // Act
            controller.Delete(5);

            // Assert
        }
    }
}
