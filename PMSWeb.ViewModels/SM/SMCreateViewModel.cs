using Microsoft.AspNetCore.Mvc.Rendering;
using PMSWeb.ViewModels.CommonVM;
using System.ComponentModel.DataAnnotations;
using static PMS.Common.EntityValidationConstants;
using static PMS.Common.EntityValidationConstants.MaintenanceConstants;

namespace PMSWeb.ViewModels.SM
{
    public class SMCreateViewModel
    {
        public SMCreateViewModel() 
        {
            Positions = PMSPositions.ToList();
        } 

        [Required]
        [MaxLength(MaintenanceNameMaxLength)]
        [MinLength(MaintenanceNameMinLength)]
        public string Name { get; set; } = null!;

        [MaxLength(MaintenanceDescriptionMaxLength)]
        [MinLength(MaintenanceDescriptionMinLength)]
        public string? Description { get; set; }

        [Required]
        public DateTime LastCompletedDate { get; set; }

        [Required]
        public int Interval { get; set; }

        [Required]
        [MaxLength(MaintenancePositionMaxLength)]
        [MinLength(MaintenancePositionMinLength)]
        public string ResponsiblePosition { get; set; } = null!;

        public string EquipmentId { get; set; } = null!;

        public List<string> Positions { get; set; }

        public List<PairViewModel> Equipments { get; set; } = new List<PairViewModel>();

    }
}
