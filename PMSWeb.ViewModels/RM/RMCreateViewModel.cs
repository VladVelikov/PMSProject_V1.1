using Microsoft.EntityFrameworkCore;
using PMS.Data.Models;
using PMS.Data.Models.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static PMS.Common.EntityValidationConstants.MaintenanceConstants;
using static PMS.Common.EntityValidationConstants;

namespace PMSWeb.ViewModels.RM
{
    public class RMCreateViewModel
    {
        public RMCreateViewModel() 
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

        public List<string> Positions { get; set; }
        

    }
}
