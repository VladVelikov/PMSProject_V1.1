using PMSWeb.ViewModels.CommonVM;
using System.ComponentModel.DataAnnotations;
using static PMS.Common.EntityValidationConstants.EquipmentConstants;

namespace PMSWeb.ViewModels.Equipment
{
    public class EquipmentCreateViewModel
    {
        

        [Required]
        [MinLength(EquipmentNameMinLength)]
        [MaxLength(EquipmentNameMaxLength)]
        public string Name { get; set; } = null!;

        [Required]
        [MaxLength(EquipmentDescriptionMaxLength)]
        [MinLength(EquipmentDescriptionMinLength)]
        public string Description { get; set; } = null!;

        [Required(ErrorMessage ="Maker field is required. *If the list is empty, please Create at least one maker first!")]
        public Guid MakerId { get; set; }

        public List<PairGuidViewModel> RoutineMaintenances { get; set; } = new List<PairGuidViewModel>();
        public List<PairGuidViewModel> Consumables { get; set; } = new List<PairGuidViewModel>();
        public List<PairGuidViewModel> Makers { get; set; } = new List<PairGuidViewModel>();



    }
}
