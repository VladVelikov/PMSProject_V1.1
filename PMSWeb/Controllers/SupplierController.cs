using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PMS.Services.Data.Interfaces;
using PMSWeb.ViewModels.SupplierVM;

namespace PMSWeb.Controllers
{
    [Authorize]
    public class SupplierController(ISupplierService supplierService) : BasicController
    {
        [HttpGet]
        public async Task<IActionResult> Select()
        {
            var modelList = await supplierService.GetListOfViewModelsAsync();
            if (modelList == null)
            {
                return RedirectToAction("EmptyList", "Crushes");
            }
            return View(modelList);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var model = await supplierService.GetItemForCreateAsync();    
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SupplierCreateViewModel model,
            List<Guid> Spareparts, List<Guid> Consumables)
        {
            if (GetUserId() == null || !IsValidGuid(GetUserId()!))
            {
                return RedirectToAction("WrongData", "Crushes");
            }
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            if (string.IsNullOrWhiteSpace(model.Name) ||
                string.IsNullOrWhiteSpace(model.Address) ||
                string.IsNullOrWhiteSpace(model.Email) ||
                string.IsNullOrWhiteSpace(model.PhoneNumber) ||
                !IsValidGuid(model.CityId.ToString()) ||
                !IsValidGuid(model.CountryId.ToString())
                )
            {
                return RedirectToAction("WrongData", "Crushes");
            }
            bool isCreated = await supplierService.CreateSparepartAsync(model, GetUserId()!, Spareparts, Consumables);
            if (!isCreated)
            {
                return RedirectToAction("NotCreated", "Crushes");
            }
            return RedirectToAction(nameof(Select));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            if (!IsValidGuid(id))
            {
                return RedirectToAction("WrongData", "Crushes");
            }
            var model = await supplierService.GetItemForEditAsync(id);
            if (model == null || string.IsNullOrWhiteSpace(model.SupplierId))
            {
                return RedirectToAction("NotFound", "Crushes");
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(SupplierEditViewModel model,
            List<Guid> Spareparts, List<Guid> Consumables, List<Guid> AvailableSpareparts, List<Guid> AvailableConsumables)
        {
            if (!ModelState.IsValid)
            {
               return RedirectToAction("ModelNotValid", "Crushes");
            }
            if (model == null || model.SupplierId == null || GetUserId() == null || !IsValidGuid(GetUserId()!))
            {
                return RedirectToAction("WrongData", "Crushes");
            }
            if (string.IsNullOrWhiteSpace(model.Name) ||
                string.IsNullOrWhiteSpace(model.Address) ||
                string.IsNullOrWhiteSpace(model.Email) ||
                string.IsNullOrWhiteSpace(model.PhoneNumber) ||
                !IsValidGuid(model.CityId.ToString()) ||
                !IsValidGuid(model.CountryId.ToString())
                )
            {
                return RedirectToAction("WrongData", "Crushes");
            }
            bool isEdited = await supplierService.SaveItemToEditAsync(model, GetUserId()!, Spareparts, Consumables, AvailableSpareparts, AvailableConsumables);
            if (!isEdited)
            {
                return RedirectToAction("NotEdited", "Crushes");
            }
            return RedirectToAction(nameof(Select));
        }

        [Authorize(Roles = "Manager")]
        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            if (!IsValidGuid(id))
            {
                return RedirectToAction("WrongData", "Crushes");
            }
            var model = await supplierService.GetItemToDeleteAsync(id);
            if (model == null || string.IsNullOrWhiteSpace(model.SupplierId))
            {
                return RedirectToAction("NotFound", "Crushes");
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(SupplierDeleteViewModel model)
        {
            if (!ModelState.IsValid || string.IsNullOrWhiteSpace(model.SupplierId))
            {
                return RedirectToAction("ModelNotValid", "Crushes");
            }
            bool isDeleted = await supplierService.ConfirmDeleteAsync(model);
            if (!isDeleted)
            {
                return RedirectToAction("NotDeleted", "Crushes");
            }
            return RedirectToAction(nameof(Select));
        }

        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            if (!IsValidGuid(id))
            {
                return RedirectToAction("WrongData", "Crushes");
            }
            var model = await supplierService.GetDetailsAsync(id);
            if (model == null || string.IsNullOrWhiteSpace(model.SupplierId))
            {
                return RedirectToAction("NotFound", "Crushes");
            }
            return View(model);
        }
    }
}
