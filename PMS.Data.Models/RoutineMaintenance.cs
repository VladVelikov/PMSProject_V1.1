using Microsoft.EntityFrameworkCore;
using PMS.Data.Models.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static PMS.Common.EntityValidationConstants.MaintenanceConstants;
using PMS.Data.Models.Enums;

namespace PMS.Data.Models
{
    public class RoutineMaintenance
    {
        public RoutineMaintenance() 
        {
            this.RoutMaintId = Guid.NewGuid();
        }

        [Key]
        [Comment("Unique identifier of the RoutineMaintenance")]
        public Guid RoutMaintId { get; set; }

        [Required]
        [MaxLength(MaintenanceNameMaxLength)]
        [MinLength(MaintenanceNameMinLength)]
        [Comment("Name of the maintanance")]
        public string Name { get; set; } = null!;

        [MaxLength(MaintenanceDescriptionMaxLength)]
        [MinLength(MaintenanceDescriptionMinLength)]
        [Comment("Description of the maintenance")]
        public string? Description { get; set; }
        
        [Required]
        [Comment("Date when maintanance is completed")]
        public DateTime LastCompletedDate { get; set; }

        [Comment("Interval to do the maintanance")]
        public int Interval { get; set; }

        [Required]
        [MaxLength(MaintenancePositionMaxLength)]
        [MinLength(MaintenancePositionMinLength)]
        [Comment("Position of the person responsible for the maintenance")]
        public string ResponsiblePosition { get; set; } = null!;

        [Required]
        [Comment("Unique identifier of the creator of the maintenance")]
        public string CReatorId { get; set; } = null!;

        [ForeignKey(nameof(CReatorId))]
        public virtual PMSUser Creator { get; set; } = null!;
        
        [Required]
        [Comment("Date when created on")]
        public DateTime CreatedOn { get; set; }

        [Required]
        [Comment("Date when last edited")]
        public DateTime EditedOn { get; set; }

        public virtual ICollection<RoutineMaintenanceEquipment> RoutineMaintenancesEquipments { get; set; } 
            = new HashSet<RoutineMaintenanceEquipment>();  
        
        public virtual ICollection<JobOrder> JobOrders { get; set; } = new List<JobOrder>();

        public bool IsDeleted { get; set; }
    }

}
