using PMSWeb.ViewModels.SparepartVM;

namespace PMS.Services.Data.Interfaces
{
    public interface ISparepartService
    {
        Task<IEnumerable<SparepartDisplayViewModel>> GetListOfViewModelsAsync();

        Task<SparepartCreateViewModel> GetItemForCreateAsync();

        Task<bool> CreateSparepartAsync(SparepartCreateViewModel model, string userId);


        Task<SparepartDetailsViewModel> GetDetailsAsync(string id);


        Task<SparepartEditViewModel> GetItemForEditAsync(string id);


        Task<bool> SaveItemToEditAsync(SparepartEditViewModel model, string userId);


        Task<SparepartDeleteViewModel> GetItemToDeleteAsync(string id);


        Task<bool> ConfirmDeleteAsync(SparepartDeleteViewModel model);
    }
}
