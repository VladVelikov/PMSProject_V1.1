using Microsoft.EntityFrameworkCore;
using PMS.Data.Models.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static PMS.Common.EntityValidationConstants.SparePartConstants;

namespace PMS.Data.Models
{
    public class Sparepart
    {
        public Sparepart()
        {
            this.SparepartId = Guid.NewGuid();
        }

        [Key]
        [Comment("UniqueIdentifier of the spare part")]
        public Guid SparepartId { get; set; }

        [Required]
        [MaxLength(SparePartNameMaxLength)]
        [MinLength(SparePartNameMinLength)]
        [Comment("The name of the spare part")]
        public string SparepartName { get; set; } = null!;

        [Required]
        [MaxLength(SparePartDescriptionMaxLength)]
        [MinLength(SparePartDescriptionMinLength)]
        [Comment("Description of the spare part")]
        public string Description { get; set; } = null!;

        [Required]
        [Range(0,SparePartROBMaxValue)]
        [Comment("Remaining stock")]
        public double ROB { get; set; }

        [Required]
        [Range(0,SparePartPriceMaxValue)]
        [Comment("The price for one unit of the spare")]
        [Column("Price",TypeName = "DECIMAL(18,2)")]
        public decimal Price { get; set; }

        
        [Required]
        [MaxLength(SparePartUnitsMaxLength)]
        [MinLength(SparePartUnitsMinLength)]
        [Comment("Teh measuring units")]
        public string Units { get; set; } = null!;

        [Required]
        [Comment("Unique identifier of the related equipment")]
        public Guid EquipmentId { get; set; }

        [ForeignKey(nameof(EquipmentId))]
        public Equipment Equipment { get; set; } = null!;

        [Required]
        [Comment("Date when spare created on")]
        public DateTime CreatedOn { get; set; }

        [Required]
        [Comment("Date when spare last edited")]
        public DateTime EditedOn { get; set; }

        [Required]
        [Comment("Unique  identifier of the creator of the spare")]
        public string CreatorId { get; set; } = null!;

        [ForeignKey(nameof(CreatorId))]
        public PMSUser Creator { get; set; } = null!;


        public ICollection<SparepartSupplier> SparepartsSuppliers { get; set; } = new HashSet<SparepartSupplier>();

        public ICollection<Manual> Manuals { get; set; } = new List<Manual>();

        [Comment("Soft delete fpr spare part")]
        public bool IsDeleted { get; set; }     
 
    }
}