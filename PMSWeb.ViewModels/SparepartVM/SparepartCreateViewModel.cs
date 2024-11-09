using PMSWeb.ViewModels.CommonVM;
using System.ComponentModel.DataAnnotations;
using static PMS.Common.EntityValidationConstants.SparePartConstants;

namespace PMSWeb.ViewModels.SparepartVM
{
    public class SparepartCreateViewModel
    {

        [Required]
        [MaxLength(SparePartNameMaxLength)]
        [MinLength(SparePartNameMinLength)]
        public string Name { get; set; } = null!;

        [Required]
        [MaxLength(SparePartUnitsMaxLength)]
        [MinLength(SparePartUnitsMinLength)]
        public string Units { get; set; } = null!;

        [MaxLength(SparePartDescriptionMaxLength)]
        [MinLength(SparePartDescriptionMinLength)]
        public string? Description { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        [Range(0, SparePartROBMaxValue)]
        public double ROB { get; set; }

        [Required]
        public string EquipmentId { get; set; } = null!;

        [MaxLength(SparePartImageURLMaxLength)]
        public string? ImageUrl { get; set; } 

        public List<PairViewModel> Equipments { get; set; } = new List<PairViewModel>();
    }
}
