using PMSWeb.ViewModels.Consumable;

namespace PMS.Services.Data.Interfaces
{
    public interface IConsumableService
    {
        Task<IEnumerable<ConsumableDisplayViewModel>> GetListOfViewModelsAsync();


        Task<bool> CreateConsumableAsync(ConsumableCreateViewModel model, string userId);


        Task<ConsumableDetailsViewModel> GetDetailsAsync(string id);


        Task<ConsumableEditViewModel> GetItemForEditAsync(string id);


        Task<bool> SaveItemToEditAsync(ConsumableEditViewModel model, string userId);


        Task<ConsumableDeleteViewModel> GetItemToDeleteAsync(string id);


        Task<bool> ConfirmDeleteAsync(ConsumableDeleteViewModel model);

    }
}
