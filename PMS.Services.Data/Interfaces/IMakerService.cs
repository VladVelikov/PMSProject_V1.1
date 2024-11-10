using PMSWeb.ViewModels.Maker;

namespace PMS.Services.Data.Interfaces
{
    public interface IMakerService
    {

        Task<IEnumerable<MakerDisplayViewModel>> GetListOfViewModelsAsync();


        Task<bool> CreateMakerAsync(MakerCreateViewModel model, string userId);


        Task<MakerDetailsViewModel> GetDetailsAsync(string id);


        Task<MakerEditViewModel> GetItemForEditAsync(string id);


        Task<bool> SaveItemToEditAsync(MakerEditViewModel model, string userId);


        Task<MakerDeleteViewModel> GetItemToDeleteAsync(string id);


        Task<bool> ConfirmDeleteAsync(MakerDeleteViewModel model);
    }
}
