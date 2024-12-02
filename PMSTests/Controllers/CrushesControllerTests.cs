using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using PMSWeb.Controllers;
using static PMS.Common.ErrorMessages;

namespace PMSTests.Controllers
{
    [TestFixture]
    public class CrushesControllerTests
    {
        private CrushesController _controller;

        [SetUp]
        public void SetUp()
        {
            _controller = new CrushesController();
        }

        [TearDown]
        public void TearDown()
        {
            _controller?.Dispose();
        }

        [Test]
        public void NotFound_ReturnsViewWithCorrectViewBag()
        {
            // Act
            var result = _controller.NotFound("CallerName");

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
            var viewResult = result as ViewResult;
            Assert.AreEqual("Home", viewResult.ViewData["Controller"]);
            Assert.AreEqual(NotFoundMessage, viewResult.ViewData["Message"]);
        }

        [Test]
        public void ModelNotValid_ReturnsViewWithCorrectViewBag()
        {
            // Act
            var result = _controller.ModelNotValid("CallerName");

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
            var viewResult = result as ViewResult;
            Assert.AreEqual("Home", viewResult.ViewData["Controller"]);
            Assert.AreEqual(ModelNotValidMessage, viewResult.ViewData["Message"]);
        }

        [Test]
        public void WrongData_ReturnsViewWithCorrectViewBag()
        {
            // Act
            var result = _controller.WrongData("CallerName");

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
            var viewResult = result as ViewResult;
            Assert.AreEqual("Home", viewResult.ViewData["Controller"]);
            Assert.AreEqual(WrongDataMessage, viewResult.ViewData["Message"]);
        }

        [Test]
        public void NotDeleted_ReturnsViewWithCorrectViewBag()
        {
            // Act
            var result = _controller.NotDeleted("CallerName");

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
            var viewResult = result as ViewResult;
            Assert.AreEqual("Home", viewResult.ViewData["Controller"]);
            Assert.AreEqual(NotDeletedMessage, viewResult.ViewData["Message"]);
        }

        [Test]
        public void NotEdited_ReturnsViewWithCorrectViewBag()
        {
            // Act
            var result = _controller.NotEdited("CallerName");

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
            var viewResult = result as ViewResult;
            Assert.AreEqual("Home", viewResult.ViewData["Controller"]);
            Assert.AreEqual(NotEditedMessage, viewResult.ViewData["Message"]);
        }

        [Test]
        public void NotCreated_ReturnsViewWithCorrectViewBag()
        {
            // Act
            var result = _controller.NotCreated("CallerName");

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
            var viewResult = result as ViewResult;
            Assert.AreEqual("Home", viewResult.ViewData["Controller"]);
            Assert.AreEqual(NotCreatedMessage, viewResult.ViewData["Message"]);
        }

        [Test]
        public void NotUpdated_ReturnsViewWithCorrectViewBag()
        {
            // Act
            var result = _controller.NotUpdated("CallerName");

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
            var viewResult = result as ViewResult;
            Assert.AreEqual("Home", viewResult.ViewData["Controller"]);
            Assert.AreEqual(NotUpdatedMessage, viewResult.ViewData["Message"]);
        }

        [Test]
        public void EmptyList_ReturnsViewWithCorrectViewBag()
        {
            // Act
            var result = _controller.EmptyList("CallerName");

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
            var viewResult = result as ViewResult;
            Assert.AreEqual("Home", viewResult.ViewData["Controller"]);
            Assert.AreEqual(EmptyListMessage, viewResult.ViewData["Message"]);
        }
    }
}
