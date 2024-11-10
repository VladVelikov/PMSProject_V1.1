using PMS.Data.Models;
using PMSWeb.ViewModels.Equipment;

namespace PMS.Services.Data.Interfaces
{
    public interface IEquipmentService
    {
        Task<IEnumerable<EquipmentDisplayViewModel>> GetListOfViewModelsAsync();

        Task<EquipmentCreateViewModel> GetCreateModelAsync();

        Task<bool> CreateEquipmentAsync(EquipmentCreateViewModel model, string userId, List<Guid> RoutineMaintenances, List<Guid> consumables);


        Task<EquipmentDetailsViewModel> GetDetailsAsync(string id);


        Task<EquipmentEditViewModel> GetItemForEditAsync(string id);


        Task<bool> SaveItemToEditAsync(EquipmentEditViewModel model, string userId,
            List<Guid> RoutineMaintenances,
            List<Guid> Consumables,
            List<Guid> AvailableRoutineMaintenances,
            List<Guid> AvailableConsumables);


        Task<EquipmentDeleteViewModel> GetItemToDeleteAsync(string id);


        Task<bool> ConfirmDeleteAsync(EquipmentDeleteViewModel model);
    }
}
