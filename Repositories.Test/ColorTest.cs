using API.Interface;
using Entities;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Presentation.Test.Repositories
{
 
    public class ColorTest
    {
        [Test]
        public void colortextwith()
        {
            // Arrange
            var ColorInMemoryDatabase = new List<Color>
            {
                new Color() {Id = 1, Name = "RED", ColorCode="#524587"},
                new Color() {Id = 2, Name = "BLUE",ColorCode="#965874"},
                new Color() {Id = 3, Name = "GREEN",ColorCode="#123587"}
            };
            var repository = new Mock<IColorRepository>();
            repository.Setup(x => x.GetById(It.IsAny<int>()))
           .Returns((int i) => ColorInMemoryDatabase.Single(co => co.Id == i));

            // Act
            var ColorThatExists = repository.Object.GetById(3);

            // Assert
            Assert.IsNotNull(ColorThatExists);
            Assert.AreEqual(ColorThatExists.Id, 3);
            Assert.AreEqual(ColorThatExists.Name, "GREEN");
            Assert.That(() => repository.Object.GetById(3),
                               Throws.Exception
                                     .TypeOf<InvalidOperationException>());
        }
    }
}