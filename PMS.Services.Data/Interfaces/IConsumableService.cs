using PMSWeb.ViewModels.Consumable;

namespace PMS.Services.Data.Interfaces
{
    public interface IConsumableService
    {
        Task<IEnumerable<ConsumableDisplayViewModel>> GetListOfViewModelsAsync();


        Task<bool> CreateConsumableAsync(ConsumableCreateViewModel model);


        Task<bool> GetDetailsAsync(string Id);


        Task<bool> GetItemForEditAsync(string Id);


        Task<bool> SaveItemToEditAsync(ConsumableEditViewModel model);


        Task<bool> GetItemToDeleteAsync(string Id);


        Task<bool> ConfirmDeleteAsync(ConsumableDeleteViewModel model);

    }
}
