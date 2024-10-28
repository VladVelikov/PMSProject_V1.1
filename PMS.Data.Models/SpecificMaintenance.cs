using Microsoft.EntityFrameworkCore;
using PMS.Data.Models.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static PMS.Common.EntityValidationConstants.MaintenanceConstants;
using PMS.Data.Models.Enums;

namespace PMS.Data.Models
{
    public class SpecificMaintenance
    {
        public SpecificMaintenance()
        {
            this.SpecMaintId = Guid.NewGuid();
        }

        [Key]
        [Comment("Unique identifier of the RoutineMaintenance")]
        public Guid SpecMaintId { get; set; }

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

        [Comment("Is it postponed - the maintenance")]
        public bool IsPostponed { get; set; }

        [Required]
        [Comment("Unique identifier of the equipment maintained")]
        public Guid EquipmentId { get; set; }

        [ForeignKey(nameof(EquipmentId))]
        public Equipment Equipment { get; set; } = null!;

        [Required]
        [MaxLength(MaintenancePositionMaxLength)]
        [MinLength(MaintenancePositionMinLength)]
        [Comment("Position of the person responsible for the maintenance")]
        public string ResponsiblePosition { get; set; }


        [Required]
        [Comment("Unique identifier of the creator of the maintenance")]
        public string CReatorId { get; set; } = null!;

        [ForeignKey(nameof(CReatorId))]
        public PMSUser Creator { get; set; } = null!;

        [Required]
        [Comment("Date when created on")]
        public DateTime CreatedOn { get; set; }

        [Required]
        [Comment("Date when last edited")]
        public DateTime EditedOn { get; set; }

        public bool IsDeleted { get; set; }

    }
   

}

