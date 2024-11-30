using PMSWeb.ViewModels.RM;

namespace PMS.Services.Data.Interfaces
{
    public interface IRMService
    {
        Task<IEnumerable<RMDisplayViewModel>> GetListOfViewModelsAsync();


        Task<bool> CreateRMAsync(RMCreateViewModel model, string userId);


        Task<RMDetailsViewModel> GetDetailsAsync(string id);


        Task<RMEditViewModel> GetItemForEditAsync(string id);


        Task<bool> SaveItemToEditAsync(RMEditViewModel model, string userId);


        Task<RMDeleteViewModel> GetItemToDeleteAsync(string id);


        Task<bool> ConfirmDeleteAsync(RMDeleteViewModel model);
    }
}
