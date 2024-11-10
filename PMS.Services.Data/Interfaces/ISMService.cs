using PMSWeb.ViewModels.SM;

namespace PMS.Services.Data.Interfaces
{
    public interface ISMService
    {
        Task<IEnumerable<SMDisplayViewModel>> GetListOfViewModelsAsync();

        Task<SMCreateViewModel> GetItemForCreateAsync();

        Task<bool> CreateSpecificMaintenanceAsync(SMCreateViewModel model, string userId);


        Task<SMDetailsViewModel> GetDetailsAsync(string id);


        Task<SMEditViewModel> GetItemForEditAsync(string id);


        Task<bool> SaveItemToEditAsync(SMEditViewModel model, string userId);


        Task<SMDeleteViewModel> GetItemToDeleteAsync(string id);


        Task<bool> ConfirmDeleteAsync(SMDeleteViewModel model);
    }
}
