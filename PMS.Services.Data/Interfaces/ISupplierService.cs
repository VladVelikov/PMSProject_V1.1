using PMSWeb.ViewModels.SupplierVM;

namespace PMS.Services.Data.Interfaces
{
    public interface ISupplierService
    {
        Task<IEnumerable<SupplierDisplayViewModel>> GetListOfViewModelsAsync();

        Task<SupplierCreateViewModel> GetItemForCreateAsync();

        Task<bool> CreateSparepartAsync(SupplierCreateViewModel model, string userId, List<Guid> Spareparts, List<Guid> Consumables);


        Task<SupplierDetailsViewModel> GetDetailsAsync(string id);


        Task<SupplierEditViewModel> GetItemForEditAsync(string id);


        Task<bool> SaveItemToEditAsync(SupplierEditViewModel model, string userId,
                                                               List<Guid> Spareparts,
                                                               List<Guid> Consumables,
                                                               List<Guid> AvailableSpareparts,
                                                               List<Guid> AvailableConsumables);


        Task<SupplierDeleteViewModel> GetItemToDeleteAsync(string id);


        Task<bool> ConfirmDeleteAsync(SupplierDeleteViewModel model);
    }
}
