using PMSWeb.ViewModels.InventoryVM;
using Microsoft.AspNetCore.Mvc;
using PMS.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace PMSWeb.Controllers
{
    [Authorize]
    public class InventoryController(PMSDbContext context) : Controller
    {
        [HttpGet]
        public async Task<IActionResult> SparesInventory()
        {
            var model = new SparesInventoryViewModel();
            model.Name = "Spare Parts Inventory";

            var sparesList = await context
                .Spareparts
                .Where(x => !x.IsDeleted)
                .AsNoTracking()
                .OrderByDescending(x=>x.EditedOn)
                .Select(x => new InventoryItemViewModel() {
                    Id = x.SparepartId.ToString(),
                    Name = x.SparepartName,
                    Available = x.ROB,
                    Units = x.Units,
                    Used = x.ROB,
                    Price = x.Price.ToString(),
                    EditedOn = x.EditedOn
                })
                .ToListAsync();  
            model.Spares = sparesList;
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateSparesInventory(SparesInventoryViewModel model)
        {
            var mySpares = await context
                .Spareparts
                .Where(x => !x.IsDeleted)
                .ToListAsync();
            foreach (var item in model.Spares)
            {
                var spare = mySpares.FirstOrDefault(x => x.SparepartId.ToString().ToLower() == item.Id.ToLower());
                if (item.Used < 0)  // in this case real stock
                {
                    //do nothing
                    //spare.ROB -= item.Used;  // for testing only 
                }
                else 
                {
                    spare.ROB = item.Used;  // item.RealStock
                }
                
            }
            await context.SaveChangesAsync();
            return RedirectToAction(nameof(SparesInventory));
        }

        [HttpGet]
        public async Task<IActionResult> ConsumablesInventory()
        {
            var model = new ConsumablesInventoryViewModel();
            model.Name = "Consumables Inventory";

            var consumablesList = await context
                .Consumables
                .Where(x => !x.IsDeleted)
                .AsNoTracking()
                .OrderByDescending(x=>x.EditedOn)
                .Select(x => new InventoryItemViewModel()
                {
                    Id = x.ConsumableId.ToString(),
                    Name = x.Name,
                    Available = x.ROB,
                    Units = x.Units,
                    Used = x.ROB,
                    Price = x.Price.ToString(),
                    EditedOn = x.EditedOn
                })
                .ToListAsync();
            model.Consumables = consumablesList;
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateConsumablesInventory(ConsumablesInventoryViewModel model)
        {
            var myConsumables = await context
               .Consumables
               .Where(x => !x.IsDeleted)
               .ToListAsync();

            foreach (var item in model.Consumables)
            {
                var consumable = myConsumables.FirstOrDefault(x => x.ConsumableId.ToString().ToLower() == item.Id.ToLower());
                if (item.Used < 0)  // in this case real stock
                {
                    //do nothing
                    //spare.ROB -= item.Used;  // for testing only 
                }
                else
                {
                    consumable.ROB = item.Used;  // item.RealStock
                }
            }
            await context.SaveChangesAsync();
            return RedirectToAction(nameof(ConsumablesInventory));
        }
    }
}
