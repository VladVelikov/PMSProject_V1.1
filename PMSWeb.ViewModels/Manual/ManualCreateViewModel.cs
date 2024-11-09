using System.ComponentModel.DataAnnotations;
using PMSWeb.ViewModels.CommonVM;
using static PMS.Common.EntityValidationConstants.ManualClassConstants;

namespace PMSWeb.ViewModels.Manual
{
    public class ManualCreateViewModel
    {
        [Required]
        [MaxLength(ManualNameMaxLength)]
        [MinLength(ManualNameMinLength)]
        public string ManualName { get; set; } = null!;

        [MaxLength(ManualURLMaxLength)]
        [MinLength(ManualURLMinLength)]
        public string? ContentURL { get; set; }

        [Required]
        public string MakerId { get; set; } = null!;

        [Required]
        public string EquipmentId { get; set; } = null!;

        public ICollection<PairViewModel> Equipments { get; set; } 
            = new List<PairViewModel>();

        public ICollection<PairViewModel> Makers { get; set; } 
            = new List<PairViewModel>();

    }
}
