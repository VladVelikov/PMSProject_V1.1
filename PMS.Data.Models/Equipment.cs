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
        
        public ICollection<SpecificMaintenance> SpecificMaintenances = new List<SpecificMaintenance>();  
                                                      

        public ICollection<Sparepart> SpareParts = new List<Sparepart>();

        public ICollection<Manual> Manuals = new List<Manual>();    

        public ICollection<ConsumableEquipment> ConsumablesEquipments = new HashSet<ConsumableEquipment>();

        public bool IsDeleted { get; set; }

    }
}
