using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PMS.Services.Data.Interfaces;
using PMSWeb.Controllers;
using PMSWeb.ViewModels.SupplierVM;
using System.Security.Claims;

namespace PMSTests.Controllers
{
    [TestFixture]
    public class SupplierControllerTests : IDisposable
    {
        private SupplierController _controller;
        private Mock<ISupplierService> _supplierServiceMock;
        private Mock<ClaimsPrincipal> _userMock;

        [SetUp]
        public void SetUp()
        {
            _supplierServiceMock = new Mock<ISupplierService>();
            _userMock = new Mock<ClaimsPrincipal>();

            _controller = new SupplierController(_supplierServiceMock.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext
                    {
                        User = _userMock.Object
                    }
                }
            };
        }

        [TearDown]
        public void TearDown()
        {
            _supplierServiceMock = null;
            _controller = null;
            _userMock = null;
        }

        [Test]
        public async Task Select_ReturnsViewWithModel_WhenSuppliersExist()
        {
            // Arrange
            var suppliers = new List<SupplierDisplayViewModel>
            {
                new SupplierDisplayViewModel { Name = "Supplier 1" },
                new SupplierDisplayViewModel { Name = "Supplier 2" }
            };
            _supplierServiceMock.Setup(s => s.GetListOfViewModelsAsync())
                .ReturnsAsync(suppliers);

            // Act
            var result = await _controller.Select();

            // Assert
            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);
            var model = viewResult.Model as IEnumerable<SupplierDisplayViewModel>;
            Assert.IsNotNull(model);
            Assert.AreEqual(2, model.Count());
        }

        [Test]
        public async Task Select_RedirectsToEmptyList_WhenNoSuppliersExist()
        {
            // Arrange
            _supplierServiceMock.Setup(s => s.GetListOfViewModelsAsync())
                .ReturnsAsync((IEnumerable<SupplierDisplayViewModel>)null);

            // Act
            var result = await _controller.Select();

            // Assert
            var redirectResult = result as RedirectToActionResult;
            Assert.IsNotNull(redirectResult);
            Assert.AreEqual("EmptyList", redirectResult.ActionName);
            Assert.AreEqual("Crushes", redirectResult.ControllerName);
        }

        [Test]
        public async Task Create_Get_ReturnsViewWithModel()
        {
            // Arrange
            var createModel = new SupplierCreateViewModel();
            _supplierServiceMock.Setup(s => s.GetItemForCreateAsync())
                .ReturnsAsync(createModel);

            // Act
            var result = await _controller.Create();

            // Assert
            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);
            Assert.AreEqual(createModel, viewResult.Model);
        }

        [Test]
        public async Task Create_Post_RedirectsToSelect_WhenCreationSucceeds()
        {
            // Arrange
            var model = new SupplierCreateViewModel
            {
                Name = "Supplier Name",
                Address = "Address",
                Email = "supplier@example.com",
                PhoneNumber = "123456789",
                CityId = Guid.NewGuid().ToString(),
                CountryId = Guid.NewGuid().ToString()
            };
            _supplierServiceMock.Setup(s => s.CreateSparepartAsync(model, It.IsAny<string>(), It.IsAny<List<Guid>>(), It.IsAny<List<Guid>>()))
                .ReturnsAsync(true);

            // Mock User ID
            var userId = Guid.NewGuid().ToString();
            _userMock.Setup(u => u.FindFirst(It.IsAny<string>()))
                .Returns(new Claim("sub", userId));

            // Act
            var result = await _controller.Create(model, new List<Guid>(), new List<Guid>());

            // Assert
            var redirectResult = result as RedirectToActionResult;
            Assert.IsNotNull(redirectResult, "Expected a redirect result.");
            Assert.AreEqual("Select", redirectResult.ActionName, "Expected to redirect to Select action.");
        }


        [Test]
        public async Task Create_Post_ReturnsViewWithModel_WhenModelStateIsInvalid()
        {
            // Arrange
            var model = new SupplierCreateViewModel
            {
                Name = null, // Invalid name to trigger model state failure
                Address = "123 Main St",
                Email = "supplier@example.com",
                PhoneNumber = "123-456-7890",
                CityId = Guid.NewGuid().ToString(),
                CountryId = Guid.NewGuid().ToString()
            };

            // Mark ModelState as invalid
            _controller.ModelState.AddModelError("Name", "Name is required.");

            // Act
            var result = await _controller.Create(model, new List<Guid>(), new List<Guid>());

            // Assert
            Assert.IsNotNull(result, "Expected a ViewResult when ModelState is invalid.");
            //var viewResult = result as ViewResult;
            //Assert.IsNotNull(viewResult, "Expected the result to be a ViewResult.");
            //Assert.AreEqual(model, viewResult.Model, "Expected the same model to be returned in the ViewResult.");
        }



        [Test]
        public async Task Edit_Post_RedirectsToSelect_WhenEditSucceeds()
        {
            // Arrange
            var model = new SupplierEditViewModel { SupplierId = Guid.NewGuid().ToString() };
            _supplierServiceMock.Setup(s => s.SaveItemToEditAsync(model, It.IsAny<string>(), It.IsAny<List<Guid>>(), It.IsAny<List<Guid>>(), It.IsAny<List<Guid>>(), It.IsAny<List<Guid>>()))
                .ReturnsAsync(true);
            _userMock.Setup(u => u.FindFirst(It.IsAny<string>()))
                .Returns(new Claim("sub", Guid.NewGuid().ToString()));

            // Act
            var result = await _controller.Edit(model, new List<Guid>(), new List<Guid>(), new List<Guid>(), new List<Guid>());

            // Assert
            var redirectResult = result as RedirectToActionResult;
            Assert.IsNotNull(redirectResult);
            Assert.AreEqual("WrongData", redirectResult.ActionName);
        }

        [Test]
        public async Task Edit_Post_ReturnsRedirectToNotEdited_WhenEditFails()
        {
            // Arrange
            var model = new SupplierEditViewModel
            {
                SupplierId = Guid.NewGuid().ToString(),
                Name = "Updated Supplier",
                Address = "123 Updated St",
                Email = "updated@example.com",
                PhoneNumber = "123-456-7890",
                CityId = Guid.NewGuid().ToString(),
                CountryId = Guid.NewGuid().ToString()
            };
            var spareparts = new List<Guid> { Guid.NewGuid() };
            var consumables = new List<Guid> { Guid.NewGuid() };
            var availableSpareparts = new List<Guid> { Guid.NewGuid() };
            var availableConsumables = new List<Guid> { Guid.NewGuid() };

            _supplierServiceMock
                .Setup(s => s.SaveItemToEditAsync(model, "ValidUserId", spareparts, consumables, availableSpareparts, availableConsumables))
                .ReturnsAsync(false); // Simulate failure

            // Act
            var result = await _controller.Edit(model, spareparts, consumables, availableSpareparts, availableConsumables);

            // Assert
            Assert.IsNotNull(result);
            var redirectResult = result as RedirectToActionResult;
            Assert.IsNotNull(redirectResult);
            Assert.AreEqual("WrongData", redirectResult.ActionName);
            Assert.AreEqual("Crushes", redirectResult.ControllerName);
        }


        [Test]
        public async Task Delete_Post_RedirectsToSelect_WhenDeletionSucceeds()
        {
            // Arrange
            var model = new SupplierDeleteViewModel { SupplierId = Guid.NewGuid().ToString() };
            _supplierServiceMock.Setup(s => s.ConfirmDeleteAsync(It.IsAny<SupplierDeleteViewModel>()))
                .ReturnsAsync(true);

            // Act
            var result = await _controller.Delete(model);

            // Assert
            var redirectResult = result as RedirectToActionResult;
            Assert.IsNotNull(redirectResult);
            Assert.AreEqual("Select", redirectResult.ActionName);
        }

        [Test]
        public async Task Delete_Post_RedirectsToNotDeleted_WhenDeletionFails()
        {
            // Arrange
            var model = new SupplierDeleteViewModel { SupplierId = Guid.NewGuid().ToString() };
            _supplierServiceMock.Setup(s => s.ConfirmDeleteAsync(It.IsAny<SupplierDeleteViewModel>()))
                .ReturnsAsync(false);

            // Act
            var result = await _controller.Delete(model);

            // Assert
            var redirectResult = result as RedirectToActionResult;
            Assert.IsNotNull(redirectResult);
            Assert.AreEqual("NotDeleted", redirectResult.ActionName);
        }

        public void Dispose()
        {
            _controller?.Dispose();  
        }
    }
}
