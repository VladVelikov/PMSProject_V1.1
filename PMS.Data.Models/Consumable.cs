using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using PMS.Data.Models.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static PMS.Common.EntityValidationConstants.ConsumableConstants;

namespace PMS.Data.Models
{
    public class Consumable
    {
        public Consumable() 
        {
            this.ConsumableId = Guid.NewGuid();
        }

        [Key]
        [Comment("Unique identifier of the Consumable")]
        public Guid ConsumableId { get; set; }

        [Required]
        [MaxLength(ConsumableNameMaxLength)]
        [MinLength(ConsumableNameMinLength)]
        [Comment("The name of the consumable")]
        public string Name { get; set; } = null!;

        [Required]
        [MaxLength(ConsumableUnitStringMaxLength)]
        [MinLength(ConsumableUnitStringMinLength)]
        [Comment("The measuring unit of the consumable")]
        public string Units { get; set; } = null!;

        [MaxLength(ConsumableDescriptionMaxLength)]
        [MinLength(ConsumableDescriptionMinLength)]
        [Comment("Description of the consumable")]
        public string? Description { get; set; }

        [Required] 
        [Comment("Unique identifier of the Creator")]
        public string CreatorId { get; set; } = null!;

        [ForeignKey(nameof(CreatorId))]
        public PMSUser Creator { get; set; } = null!;

        [Required]
        [Comment("Date when created on")]
        public DateTime CreatedOn { get; set; }

        [Required]
        [Comment("Date when last edited")]
        public DateTime EditedOn { get; set; }

        [Required]
        [Column("Price", TypeName = "DECIMAL(18,2)")]
        [Comment("The price of the consumable per unit")]
        public decimal Price { get; set; }
        
        [Required]
        [Range(0,ConsumableROBMaxValue)]
        [Comment("The remaining quantity on stock")]
        public double  ROB { get; set; }

        public ICollection<ConsumableEquipment> ConsumablesEquipments = new HashSet<ConsumableEquipment>();
        
        public ICollection<ConsumableSupplier> ConsumablesSuppliers = new HashSet<ConsumableSupplier>(); 

        public bool IsDeleted { get; set; } 
    }
}