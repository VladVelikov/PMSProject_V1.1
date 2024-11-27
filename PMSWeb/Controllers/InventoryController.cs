using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PMS.Data;
using PMS.Services.Data.Interfaces;
using PMSWeb.ViewModels.InventoryVM;

namespace PMSWeb.Controllers
{
    [Authorize]
    public class InventoryController(PMSDbContext context, IInventoryService inventoryService) : Controller
    {
        [HttpGet]
        public async Task<IActionResult> SparesInventory()
        {
            var model = await inventoryService.GetSparesInventoryViewModelAsync();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateSparesInventory(SparesInventoryViewModel model)
        {
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
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateConsumablesInventory(ConsumablesInventoryViewModel model)
        {
            bool result = await inventoryService.UpdateConsumablesInventoryAsync(model);
            if (!result)
            {
                return RedirectToAction("NotUpdated", "Crushes");
            }
            return RedirectToAction(nameof(ConsumablesInventory));
        }
    }
}
