using Microsoft.EntityFrameworkCore;
using PMS.Data.Models.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static PMS.Common.EntityValidationConstants.JobOrderConstants;

namespace PMS.Data.Models
{
    public class JobOrder
    {
        [Key]
        [Comment("Unique identifier of the Job Order.")]
        public Guid JobId { get; set; }

        [Required]
        [MaxLength(JobOrderNameMaxLength)]
        [MinLength(JobOrderNameMinLength)]
        [Comment("The name of JobOrder")]
        public string JobName { get; set; } = null!;

        [MaxLength(JobOrderDescriptionMaxLength)]
        [MinLength(JobOrderDescriptionMinLength)]
        [Comment("Standard or modified description of JobOrder")]
        public string JobDescription { get; set; } = null!;

        [Required]
        [Comment("The next due date when the job should be completed")]
        public DateTime DueDate { get; set; }

        [Required]
        [Comment("The last date when this job was done.")]
        public DateTime LastDoneDate { get; set; }

        [Required]
        [Comment("Interval betweed LastDone and DueDate.")]
        public int Interval { get; set; }

        [Required]
        [MaxLength(JobOrderTypeMaxLength)]
        [MinLength(JobOrderTypeMinLength)]
        [Comment("Type of maintenance: Routine or Specific")]
        public string Type { get; set; } = null!;

        [Required]
        [MaxLength(JobOrderResponsiblePositionMaxLength)]
        [MinLength(JobOrderResponsiblePositionMinLength)]
        [Comment("Default of modified Position responsible for the job.")]
        public string ResponsiblePosition { get; set; } = null!;

        [Required]
        [Comment("Unique identifier of the creator of the JobOrder.")]
        public string CreatorId { get; set; } = null!;

        [ForeignKey(nameof(CreatorId))]
        public virtual PMSUser Creator { get; set; } = null!;

        [Required]
        [Comment("Unique identifier of the equipment that will be maintaind.")]
        public Guid EquipmentId { get; set; }

        [ForeignKey(nameof(EquipmentId))]
        public virtual Equipment Equipment { get; set; } = null!;

        [Comment("Unique identifier of the routine maintenance selected if any")]
        public Guid RoutineMaintenanceId { get; set; }

        [ForeignKey(nameof(RoutineMaintenanceId))]
        public RoutineMaintenance? RoutineMaintenance { get; set; }

        [Comment("Unique identifier of the specific maintenance selected if any")]
        public Guid SpecificMaintenanceId { get; set; }
        
        [ForeignKey(nameof(SpecificMaintenanceId))]
        public SpecificMaintenance? SpecificMaintenance { get; set; }

        bool IsDeleted { get; set; }
    }
}
