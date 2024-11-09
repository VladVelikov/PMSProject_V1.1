
using PMSWeb.ViewModels.CommonVM;
using System.ComponentModel.DataAnnotations;

namespace PMSWeb.ViewModels.Equipment
{
    using static PMS.Common.EntityValidationConstants.EquipmentConstants;

    public class EquipmentEditViewModel
    {
        [Required]
        public string EquipmentId { get; set; } = null!;

        [Required]
        [MinLength(EquipmentNameMinLength)]
        [MaxLength(EquipmentNameMaxLength)]
        public string Name { get; set; } = null!;

        [Required]
        [MaxLength(EquipmentDescriptionMaxLength)]
        [MinLength(EquipmentDescriptionMinLength)]
        public string Description { get; set; } = null!;

        [Required]
        public Guid MakerId { get; set; }

        public List<PairGuidViewModel> Makers { get; set; } = new List<PairGuidViewModel>();
        public List<PairGuidViewModel> Consumables { get; set; } = new List<PairGuidViewModel>();
        public List<PairGuidViewModel> RoutineMaintenances { get; set; } = new List<PairGuidViewModel>();
        public List<PairGuidViewModel> AvailableRoutineMaintenances { get; set; } = new List<PairGuidViewModel>();
        public List<PairGuidViewModel> AvailableConsumables { get; set; } = new List<PairGuidViewModel>();
    }
}
