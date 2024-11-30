using Microsoft.EntityFrameworkCore;
using PMS.Data.Models;
using PMS.Data.Repository.Interfaces;
using PMS.Services.Data.Interfaces;
using PMSWeb.ViewModels.InventoryVM;

namespace PMS.Services.Data
{
    public class InventoryService(IRepository<Sparepart,  Guid> sparesRepo,
                                  IRepository<Consumable, Guid> consumablesRepo)
                                : IInventoryService
    {
        public async Task<SparesInventoryViewModel> GetSparesInventoryViewModelAsync()
        {
            var model = new SparesInventoryViewModel();
            model.Name = "Spare Parts Inventory";

            var sparesList = await sparesRepo
                .GetAllAsQueryable()
                .Where(x => !x.IsDeleted)
                .AsNoTracking()
                .OrderByDescending(x => x.EditedOn)
                .Select(x => new InventoryItemViewModel()
                {
                    Id = x.SparepartId.ToString(),
                    Name = x.SparepartName,
                    Available = x.ROB,
                    Units = x.Units,
                    Used = x.ROB,
                    Price = x.Price.ToString(),
                    EditedOn = x.EditedOn
                })
                .ToListAsync();
            if (sparesList != null && sparesList.Count > 0)
            model.Spares = sparesList;

            return model;
        }

        public async Task<ConsumablesInventoryViewModel> GetConsumablesInventoryViewModelAsync()
        {
            var model = new ConsumablesInventoryViewModel();
            model.Name = "Consumables Inventory";

            var consumablesList = await consumablesRepo
                .GetAllAsQueryable()
                .Where(x => !x.IsDeleted)
                .AsNoTracking()
                .OrderByDescending(x => x.EditedOn)
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

            return model;
        }

        public async Task<bool> UpdateSparesInventoryAsync(SparesInventoryViewModel model)
        {
            var mySpares = await sparesRepo
                .GetAllAsQueryable()
                .Where(x => !x.IsDeleted)
                .AsNoTracking()
                .ToListAsync();
            foreach (var item in model.Spares)
            {
                var spare = mySpares.FirstOrDefault(x => x.SparepartId.ToString().ToLower() == item.Id!.ToLower());
                if (item.Used < 0)  // in this case real stock
                {
                    //do nothing
                    //spare.ROB -= item.Used;  // for testing only 
                }
                else
                {
                    if (spare != null)  
                    spare.ROB = item.Used;  // item.RealStock
                }
            }
            await sparesRepo.UpdateRange(mySpares);
            return true;
        }

        public async Task<bool> UpdateConsumablesInventoryAsync(ConsumablesInventoryViewModel model)
        {
            var myConsumables = await consumablesRepo
              .GetAllAsQueryable()
              .Where(x => !x.IsDeleted)
              .AsNoTracking()
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
            await consumablesRepo.UpdateRange(myConsumables);
            return true;
        }
    }
}
