using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PMS.Services.Data.Interfaces;
using PMSWeb.ViewModels.InventoryVM;

namespace PMSWeb.Controllers
{
    [Authorize]
    public class InventoryController(IInventoryService inventoryService) : BasicController
    {
        [HttpGet]
        public async Task<IActionResult> SparesInventory()
        {
            var model = await inventoryService.GetSparesInventoryViewModelAsync();
            if (model == null || model.Spares.Count == 0)
            {
                return RedirectToAction("EmptyList", "Crushes");
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateSparesInventory(SparesInventoryViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction(nameof(SparesInventory));
            }
            if (model.Spares.Count == 0)
            {
                return RedirectToAction("EmptyList", "Crushes");
            }
            foreach (var item in model.Spares)
            {
                if (item == null || item.Id == null || !IsValidGuid(item.Id) || !IsValidDouble(item.Used.ToString())) 
                {
                    return RedirectToAction("WrongData", "Crushes");
                }  
            }
            bool result = await inventoryService.UpdateSparesInventoryAsync(model);
            if (!result)
            {
                return RedirectToAction("NotUpdated", "Crushes");
            }
            return RedirectToAction(nameof(SparesInventory));
        }

        [HttpGet]
        public async Task<IActionResult> ConsumablesInventory()
        {
            var model = await inventoryService.GetConsumablesInventoryViewModelAsync();
            if (model == null || model.Consumables.Count == 0)
            {
                return RedirectToAction("EmptyList", "Crushes");
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateConsumablesInventory(ConsumablesInventoryViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction(nameof(ConsumablesInventory));
            }
            if (model.Consumables.Count == 0)
            {
                return RedirectToAction("EmptyList", "Crushes");
            }
            foreach (var item in model.Consumables)
            {
                if (item == null || item.Id == null || !IsValidGuid(item.Id) || !IsValidDouble(item.Used.ToString()))
                {
                    return RedirectToAction("WrongData", "Crushes");
                }
            }
            bool result = await inventoryService.UpdateConsumablesInventoryAsync(model);
            if (!result)
            {
                return RedirectToAction("NotUpdated", "Crushes");
            }
            return RedirectToAction(nameof(ConsumablesInventory));
        }
    }
}
