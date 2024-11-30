using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PMS.Services.Data.Interfaces;
using PMSWeb.ViewModels.Consumable;

namespace PMSWeb.Controllers
{
    [Authorize]
    public class ConsumableController(IConsumableService consumableService) : BasicController
    {
        public async Task<IActionResult> Select()
        {
            var models = await consumableService.GetListOfViewModelsAsync();
            if (models.Count() == 0)
            {
                return RedirectToAction("EmptyList", "Crushes");
            }
            return View(models);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var model = new ConsumableCreateViewModel();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ConsumableCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            if (GetUserId() == null)
            {
                return RedirectToAction("WrongData", "Crushes");
            }
            if (!IsValidDecimal(model.Price.ToString()))
            {
                return RedirectToAction("WrongData", "Crushes");
            }
           
            bool result = await consumableService.CreateConsumableAsync(model, GetUserId()!);
            if (!result)
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
                return RedirectToAction("NotFound", "Crushes");
            }
            ConsumableEditViewModel model = await consumableService.GetItemForEditAsync(id);
            if (model == null || string.IsNullOrEmpty(model.ConsumableId))
            {
                return RedirectToAction("NotFound", "Crushes");
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ConsumableEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            if (GetUserId() == null)
            {
                return RedirectToAction("WrongData", "Crushes");
            }
            if (model.ConsumableId == null || 
                !IsValidGuid(model.ConsumableId) || 
                !IsValidDecimal(model.Price.ToString()) ||
                !IsValidDouble(model.ROB.ToString()))
            {
                return RedirectToAction("WrongData", "Crushes");
            }

            bool isEdited = await consumableService.SaveItemToEditAsync(model, GetUserId()!);
            if (!isEdited)
            {
                return RedirectToAction("NotEdited", "Crushes");
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
            var model = await consumableService.GetDetailsAsync(id);
            if (string.IsNullOrEmpty(model.ConsumableId))
            {
                return RedirectToAction("NotFound", "Crushes");
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            if (!IsValidGuid(id))
            {
                return RedirectToAction("WrongData", "Crushes");
            }
            ConsumableDeleteViewModel model = await consumableService.GetItemToDeleteAsync(id);
            if (string.IsNullOrEmpty(model.ConsumableId))
            {
                return RedirectToAction("NotFound", "Crushes");
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(ConsumableDeleteViewModel model)
        {
            if (!IsValidGuid(model.ConsumableId))
            {
                return RedirectToAction("NotFound", "Crushes");
            }
            bool result = await consumableService.ConfirmDeleteAsync(model);
            if (!result)
            {
                return RedirectToAction("NotDeleted", "Crushes");
            }
            return RedirectToAction(nameof(Select));
        }
    }
}
