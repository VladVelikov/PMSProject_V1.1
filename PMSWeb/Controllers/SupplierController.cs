using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PMS.Data;
using PMS.Services.Data.Interfaces;
using PMSWeb.ViewModels.SupplierVM;
using System.Security.Claims;
using static PMS.Common.EntityValidationConstants;

namespace PMSWeb.Controllers
{
    [Authorize]
    public class SupplierController(PMSDbContext context, ISupplierService supplierService) : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Select()
        {
            var modelList = await supplierService.GetListOfViewModelsAsync();
            return View(modelList);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var model = await supplierService.GetItemForCreateAsync();    
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(SupplierCreateViewModel model,
            List<Guid> Spareparts, List<Guid> Consumables)
        {
            if (GetUserId() == null)
            {
                return RedirectToAction("WrongData", "Crushes");
            }
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            bool isCreated = await supplierService.CreateSparepartAsync(model, GetUserId()!, Spareparts, Consumables);
            return RedirectToAction(nameof(Select));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var model = await supplierService.GetItemForEditAsync(id);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(SupplierEditViewModel model,
            List<Guid> Spareparts, List<Guid> Consumables, List<Guid> AvailableSpareparts, List<Guid> AvailableConsumables)
        {
            if (!ModelState.IsValid)
            {
               return RedirectToAction(nameof(Select));
            }
            if (model == null || model.SupplierId == null || GetUserId() == null)
            {
                return RedirectToAction(nameof(Select));
            }

            bool isEdited = await supplierService.SaveItemToEditAsync(model, GetUserId(), Spareparts, Consumables, AvailableSpareparts, AvailableConsumables);
            return RedirectToAction(nameof(Select));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            var model = await supplierService.GetItemToDeleteAsync(id);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(SupplierDeleteViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("ModelNotValid", "Crushes");
            }

            bool isDeleted = await supplierService.ConfirmDeleteAsync(model);
            return RedirectToAction(nameof(Select));
        }

        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            var model = await supplierService.GetDetailsAsync(id);
            return View(model);
        }

        private string? GetUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier);
        }
    }
}
