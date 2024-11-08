using System.ComponentModel.DataAnnotations;
using static PMS.Common.EntityValidationConstants;
using static PMS.Common.EntityValidationConstants.MaintenanceConstants;


namespace PMSWeb.ViewModels.SM
{
    public class SMEditViewModel
    {
        public SMEditViewModel()
        {
            Positions = PMSPositions.ToList();
        }

        [Required]
        public string SMId { get; set; } = null!;

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

        public List<string> Positions { get; set; }


    }
}

