using Microsoft.EntityFrameworkCore;
using PMS.Data.Models.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static PMS.Common.EntityValidationConstants.EquipmentConstants;

namespace PMS.Data.Models
{
    public class Equipment
    {
        public Equipment()
        {
            this.EquipmentId = Guid.NewGuid();
        }
       
        [Key]
        [Comment("Unique identifier of the Equipment")]
        public Guid EquipmentId { get; set; }

        [Required]
        [MinLength(EquipmentNameMinLength)]
        [MaxLength(EquipmentNameMaxLength)]
        [Comment("The name of equipment")]
        public string Name { get; set; } = null!;
        
        [Required]
        [MaxLength(EquipmentDescriptionMaxLength)]
        [MinLength(EquipmentDescriptionMinLength)]
        [Comment("Description of equipment")]
        public string Description { get; set; } = null!;

        [Required]
        [Comment("Date when created on")]
        public DateTime CreatedOn { get; set; }

        [Required]
        [Comment("Date when last edited")]
        public DateTime EditedOn { get; set; }

        [Required]
        [Comment("Unique identifier of the Creator")]
        public string CreatorId { get; set; } = null!;

        [ForeignKey(nameof(CreatorId))]
        public virtual PMSUser Creator { get; set; } = null!;

        [Required]
        [Comment("Unique identifier of the Maker")]
        public Guid MakerId { get; set; }

        [ForeignKey(nameof(MakerId))]
        public virtual Maker Maker { get; set; } = null!;


        public virtual ICollection<RoutineMaintenanceEquipment> RoutineMaintenancesEquipments { get; set; } 
                                                       = new HashSet<RoutineMaintenanceEquipment>();
        public virtual ICollection<SpecificMaintenance> SpecificMaintenances { get; set; } = new List<SpecificMaintenance>();  
                                                      
        public virtual ICollection<Sparepart> SpareParts { get; set; } = new List<Sparepart>();

        public virtual ICollection<Manual> Manuals { get; set; } = new List<Manual>();    

        public virtual ICollection<ConsumableEquipment> ConsumablesEquipments { get; set; } = new HashSet<ConsumableEquipment>();

        public virtual ICollection<JobOrder> JobOrders { get; set; } = new List<JobOrder>();   

        public bool IsDeleted { get; set; }

    }
}
