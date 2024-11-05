
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
    }
}
