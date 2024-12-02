using Microsoft.AspNetCore.Mvc;
using Moq;
using PMS.Services.Data.Interfaces;
using PMSWeb.Controllers;
using PMSWeb.ViewModels.RequisitionVM;

namespace PMSTests.Controllers
{


    [TestFixture]
    public class RequisitionControllerTests
    {
        private Mock<IRequisitionService> _requisitionServiceMock;
        private RequisitionController _controller;

        [SetUp]
        public void SetUp()
        {
            _requisitionServiceMock = new Mock<IRequisitionService>();
            _controller = new RequisitionController(_requisitionServiceMock.Object);
        }

        [TearDown]
        public void TearDown()
        {
            _controller?.Dispose();
        }

        [Test]
        public async Task Select_ReturnsRedirectToEmptyList_WhenNoItemsExist()
        {
            // Arrange
            _requisitionServiceMock.Setup(service => service.GetAllItemsListAsync()).ReturnsAsync((List<RequisitionDisplayViewModel>)null);

            // Act
            var result = await _controller.Select();

            // Assert
            Assert.IsInstanceOf<RedirectToActionResult>(result);
            var redirectResult = result as RedirectToActionResult;
            Assert.AreEqual("EmptyList", redirectResult.ActionName);
            Assert.AreEqual("Crushes", redirectResult.ControllerName);
        }

        [Test]
        public async Task Select_ReturnsViewWithItems_WhenItemsExist()
        {
            // Arrange
            var items = new List<RequisitionDisplayViewModel>
        {
            new RequisitionDisplayViewModel { RequisitionId = Guid.NewGuid().ToString(), RequisitionName = "Test Requisition" }
        };
            _requisitionServiceMock.Setup(service => service.GetAllItemsListAsync()).ReturnsAsync(items);

            // Act
            var result = await _controller.Select();

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
            var viewResult = result as ViewResult;
            Assert.AreEqual(items, viewResult.Model);
        }

        [Test]
        public async Task CreateSpareparts_ReturnsRedirectToEmptyList_WhenNoItemsExist()
        {
            // Arrange
            _requisitionServiceMock.Setup(service => service.GetCreateSparesRequisitionModelAsync())
                                   .ReturnsAsync((RequisitionCreateViewModel)null);

            // Act
            var result = await _controller.CreateSpareparts();

            // Assert
            Assert.IsInstanceOf<RedirectToActionResult>(result);
            var redirectResult = result as RedirectToActionResult;
            Assert.AreEqual("EmptyList", redirectResult.ActionName);
            Assert.AreEqual("Crushes", redirectResult.ControllerName);
        }

        [Test]
        public async Task CreateSpareparts_ReturnsViewWithModel_WhenItemsExist()
        {
            // Arrange
            var model = new RequisitionCreateViewModel
            {
                RequisitionItems = new List<RequisitionItemViewModel> { new RequisitionItemViewModel { Id = Guid.NewGuid().ToString() } }
            };
            _requisitionServiceMock.Setup(service => service.GetCreateSparesRequisitionModelAsync()).ReturnsAsync(model);

            // Act
            var result = await _controller.CreateSpareparts();

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
            var viewResult = result as ViewResult;
            Assert.AreEqual(model, viewResult.Model);
        }

        [Test]
        public async Task Details_ReturnsRedirectToWrongData_WhenIdIsInvalid()
        {
            // Act
            var result = await _controller.Details("invalid-guid");

            // Assert
            Assert.IsInstanceOf<RedirectToActionResult>(result);
            var redirectResult = result as RedirectToActionResult;
            Assert.AreEqual("WrongData", redirectResult.ActionName);
            Assert.AreEqual("Crushes", redirectResult.ControllerName);
        }

        [Test]
        public async Task Details_ReturnsRedirectToNotFound_WhenRequisitionDoesNotExist()
        {
            // Arrange
            _requisitionServiceMock.Setup(service => service.GetRequisitionDetailsModelAsync(It.IsAny<string>()))
                                   .ReturnsAsync((RequisitionDetailsViewModel)null);

            // Act
            var result = await _controller.Details(Guid.NewGuid().ToString());

            // Assert
            Assert.IsInstanceOf<RedirectToActionResult>(result);
            var redirectResult = result as RedirectToActionResult;
            Assert.AreEqual("NotFound", redirectResult.ActionName);
            Assert.AreEqual("Crushes", redirectResult.ControllerName);
        }

        [Test]
        public async Task Details_ReturnsViewWithModel_WhenRequisitionExists()
        {
            // Arrange
            var model = new RequisitionDetailsViewModel { RequisitionId = Guid.NewGuid().ToString() };
            _requisitionServiceMock.Setup(service => service.GetRequisitionDetailsModelAsync(It.IsAny<string>())).ReturnsAsync(model);

            // Act
            var result = await _controller.Details(model.RequisitionId);

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
            var viewResult = result as ViewResult;
            Assert.AreEqual(model, viewResult.Model);
        }

        [Test]
        public async Task Approve_ReturnsRedirectToSelect_WhenResultIsNullOrApproved()
        {
            // Arrange
            _requisitionServiceMock.Setup(service => service.ApproveRequisition(It.IsAny<string>())).ReturnsAsync("NullOrApproved");

            // Act
            var result = await _controller.Approve(Guid.NewGuid().ToString());

            // Assert
            Assert.IsInstanceOf<RedirectToActionResult>(result);
            var redirectResult = result as RedirectToActionResult;
            Assert.AreEqual(nameof(RequisitionController.Select), redirectResult.ActionName);
        }

        [Test]
        public async Task Approve_ReturnsRedirectToNotUpdated_WhenResultIsError()
        {
            // Arrange
            _requisitionServiceMock.Setup(service => service.ApproveRequisition(It.IsAny<string>())).ReturnsAsync("Error");

            // Act
            var result = await _controller.Approve(Guid.NewGuid().ToString());

            // Assert
            Assert.IsInstanceOf<RedirectToActionResult>(result);
            var redirectResult = result as RedirectToActionResult;
            Assert.AreEqual("NotUpdated", redirectResult.ActionName);
            Assert.AreEqual("Crushes", redirectResult.ControllerName);
        }

        [Test]
        public async Task ConfirmDelete_RedirectsToSelect_WhenSuccessful()
        {
            // Arrange
            _requisitionServiceMock.Setup(service => service.DeleteRequisitionAsync(It.IsAny<string>())).ReturnsAsync(true);

            // Act
            var result = await _controller.ConfirmDelete(Guid.NewGuid().ToString());

            // Assert
            Assert.IsInstanceOf<RedirectToActionResult>(result);
            var redirectResult = result as RedirectToActionResult;
            Assert.AreEqual(nameof(RequisitionController.Select), redirectResult.ActionName);
        }
    }

}
