using PMSWeb.ViewModels.Manual;

namespace PMS.Services.Data.Interfaces
{
    public interface IManualService
    {
        Task<IEnumerable<ManualDisplayViewModel>> GetListOfViewModelsAsync();

        Task<ManualCreateViewModel> GetCreateViewModelAsync(string URL);

        Task<bool> CreateManualAsync(ManualCreateViewModel model, string userId);

        Task<ManualDetailsViewModel> GetDetailsAsync(string id);

        Task<ManualDeleteViewModel> GetItemToDeleteAsync(string id);

        Task<bool> ConfirmDeleteAsync(ManualDeleteViewModel model);
    }
}
