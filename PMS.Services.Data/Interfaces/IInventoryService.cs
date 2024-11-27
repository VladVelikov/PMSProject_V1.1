using PMSWeb.ViewModels.InventoryVM;

namespace PMS.Services.Data.Interfaces
{
    public interface IInventoryService
    {
        public Task<SparesInventoryViewModel> GetSparesInventoryViewModelAsync();

        public Task<bool> UpdateSparesInventoryAsync(SparesInventoryViewModel model);

        public Task<ConsumablesInventoryViewModel> GetConsumablesInventoryViewModelAsync();

        public Task<bool> UpdateConsumablesInventoryAsync(ConsumablesInventoryViewModel model);


    }
}
