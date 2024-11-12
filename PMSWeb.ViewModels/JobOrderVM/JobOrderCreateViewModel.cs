using Microsoft.EntityFrameworkCore;
using PMS.Data.Models;
using PMS.Data.Models.Identity;
using PMSWeb.ViewModels.CommonVM;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static PMS.Common.EntityValidationConstants.JobOrderConstants;

namespace PMSWeb.ViewModels.JobOrderVM
{
    public class JobOrderCreateViewModel
    {
        

        [Required]
        [MaxLength(JobOrderNameMaxLength)]
        [MinLength(JobOrderNameMinLength)]
        public string JobName { get; set; } = null!;

        [MaxLength(JobOrderDescriptionMaxLength)]
        [MinLength(JobOrderDescriptionMinLength)]
        public string JobDescription { get; set; } = null!;

        [Required]
        public DateTime DueDate { get; set; } = DateTime.UtcNow;

        [Required]
        public DateTime LastDoneDate { get; set; }

        [Required]
        public int Interval { get; set; }

        [Required]
        [MaxLength(JobOrderTypeMaxLength)]
        [MinLength(JobOrderTypeMinLength)]
        public string Type { get; set; } = null!;

        [Required]
        [MaxLength(JobOrderResponsiblePositionMaxLength)]
        [MinLength(JobOrderResponsiblePositionMinLength)]
        public string ResponsiblePosition { get; set; } = null!;

        [Required]
        public Guid EquipmentId { get; set; }

        [Required]
        public Guid RoutineMaintenanceId { get; set; }

        [Required]
        public Guid SpecificMaintenanceId { get; set; }

        public string MaintenanceName { get; set; }

        public string EquipmentName { get; set; }

        public string MaintenanceType { get; set; }

        
    }
}
