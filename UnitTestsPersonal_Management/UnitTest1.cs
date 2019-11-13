using System.Collections.Generic;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Personal_Management.Controllers;
using Personal_Management.Models;

namespace UnitTestsPersonal_Management
{
    [TestClass]
    public class UnitTest1
    {
        //[TestMethod]
        //public void RatesCreate()
        //{
        //    // Arrange
        //    RatesController controller = new RatesController();
        //    // Act
        //    ViewResult result = controller.Create() as ViewResult;
        //    // Assert
        //    Assert.IsNotNull(result);
        //}

        //[TestMethod]
        //public void RatesView()
        //{
        //    // Arrange
        //    var mock = new Mock<IReposit>();
        //    mock.Setup(a => a.GetComputerList()).Returns(new List<Rates>());
        //    RatesController controller = new RatesController(mock.Object);

        //    // Act
        //    ViewResult result = controller.Index() as ViewResult;

        //    // Assert
        //    Assert.IsNotNull(result.Model);
        //}

        //[TestMethod]
        //public void IndexViewBagMessage()
        //{
        //    // arrange
        //    string expected = "Index";
        //    var mock = new Mock<IReposit>();
        //    Rates comp = new Rates();
        //    RatesController controller = new RatesController(mock.Object);
        //    // act
        //    RedirectToRouteResult result = controller.Create(comp) as RedirectToRouteResult;

        //    // assert
        //    Assert.IsNotNull(result);
        //    Assert.AreEqual(expected, result.RouteValues["action"]);
        //}


    }
}
