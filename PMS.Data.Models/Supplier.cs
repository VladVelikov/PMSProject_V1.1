using Microsoft.EntityFrameworkCore;
using PMS.Data.Models.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static PMS.Common.EntityValidationConstants.SupplierConstatnts;

namespace PMS.Data.Models
{
    public class Supplier
    {
        public Supplier() 
        {
            this.SupplierId = Guid.NewGuid();
        }   

        [Key]
        [Comment("UniqueIdentifierOf The Supplier")]
        public Guid SupplierId { get; set; }

        [Required]
        [MinLength(SupplierNameMinLength)]
        [MaxLength(SupplierNameMaxLength)]
        [Comment("The name of equipment")]
        public string Name { get; set; } = null!;

        [Required]
        [MinLength(SupplierAddressMinLength)]
        [MaxLength(SupplierAddressMaxLength)]
        [Comment("The name of equipment")]
        public string Address { get; set; } = null!;

        [Required]
        [MinLength(SupplierEmailMinLength)]
        [MaxLength(SupplierEmailMaxLength)]
        [Comment("The name of equipment")]
        public string Email { get; set; } = null!;

        [Required]
        [MinLength(SupplierPhoneMinLength)]
        [MaxLength(SupplierPhoneMaxLength)]
        [Comment("The name of equipment")]
        public string PhoneNumber { get; set; } = null!;

        [Required]
        [Comment("UniqueIdentifierOf The City")]
        public Guid CityId { get; set; } 

        [ForeignKey(nameof(CityId))]
        public City City { get; set; } = null!;


        [Required]
        [Comment("UniqueIdentifierOf The Country")]
        public Guid CountryId { get; set; } 

        [ForeignKey(nameof(CountryId))]
        public Country Country { get; set; } = null!;

        [Required]
        [Comment("UniqueIdentifierOf The Creator")]
        public string CreatorId { get; set; } = null!;


        [ForeignKey(nameof(CreatorId))]
        public PMSUser Creator { get; set; } = null!;

        [Required]
        [Comment("Date when created on")]
        public DateTime CreatedOn { get; set; }

        [Required]
        [Comment("Date when last edited")]
        public DateTime EditedOn { get; set; }


        public ICollection<ConsumableSupplier> ConsumablesSuppliers { get; set; } = new HashSet<ConsumableSupplier>();
        public ICollection<SparepartSupplier> SparepartsSuppliers = new HashSet<SparepartSupplier>();

        public bool IsDleted { get; set; }
    }
}
