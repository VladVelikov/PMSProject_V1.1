using Microsoft.EntityFrameworkCore;
using PMS.Data.Models;
using PMS.Data.Models.Identity;
using PMSWeb.ViewModels.CommonVM;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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

        [Required]
        public Guid MakerId { get; set; }

        public List<PairGuidViewModel> RoutineMaintenances { get; set; } = new List<PairGuidViewModel>();
        public List<PairGuidViewModel> Consumables { get; set; } = new List<PairGuidViewModel>();
        public List<PairGuidViewModel> Makers { get; set; } = new List<PairGuidViewModel>();



    }
}
