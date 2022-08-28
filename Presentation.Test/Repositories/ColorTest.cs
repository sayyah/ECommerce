using API.Interface;
using Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Assert = NUnit.Framework.Assert;

namespace Presentation.Test.Repositories
{
    [Test]
    public class ColorTest
    {
        [TestMethod]
        public void colortextwith()
        {
            var ColorInMemoryDatabase = new List<Color>
            {
                new Color() {Id = 1, Name = "RED", ColorCode="#312"},
                new Color() {Id = 2, Name = "BLUE",ColorCode="#432"},
                new Color() {Id = 3, Name = "GREEN",ColorCode="#827"}
            };
            var repository = new Mock<IColorRepository>();
            repository.Setup(x => x.GetById(It.IsAny<int>()))
           .Returns((int i) => ColorInMemoryDatabase.Single(co => co.Id == i));
            var ColorThatExists = repository.Object.GetById(3);
            Assert.IsNotNull(ColorThatExists);
            Assert.AreEqual(ColorThatExists.Id, 3);
            Assert.AreEqual(ColorThatExists.Name, "GREEN");
            Assert.That(() => repository.Object.GetById(3),
                               Throws.Exception
                                     .TypeOf<InvalidOperationException>());
        }
    }
}