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
        [Comment("Unique Identifier Of The Supplier")]
        public Guid SupplierId { get; set; }

        [Required]
        [MinLength(SupplierNameMinLength)]
        [MaxLength(SupplierNameMaxLength)]
        [Comment("The name of supplier")]
        public string Name { get; set; } = null!;

        [Required]
        [MinLength(SupplierAddressMinLength)]
        [MaxLength(SupplierAddressMaxLength)]
        [Comment("The address of supplier")]
        public string Address { get; set; } = null!;

        [Required]
        [MinLength(SupplierEmailMinLength)]
        [MaxLength(SupplierEmailMaxLength)]
        [Comment("The email of supplier")]
        public string Email { get; set; } = null!;

        [Required]
        [MinLength(SupplierPhoneMinLength)]
        [MaxLength(SupplierPhoneMaxLength)]
        [Comment("The phone number of supplier")]
        public string PhoneNumber { get; set; } = null!;

        [Required]
        [Comment("Unique Identifier Of The City")]
        public Guid CityId { get; set; } 

        [ForeignKey(nameof(CityId))]
        public virtual City City { get; set; } = null!;


        [Required]
        [Comment("Unique Identifier Of The Country")]
        public Guid CountryId { get; set; } 

        [ForeignKey(nameof(CountryId))]
        public virtual Country Country { get; set; } = null!;

        [Required]
        [Comment("Unique Identifier Of The Creator")]
        public string CreatorId { get; set; } = null!;


        [ForeignKey(nameof(CreatorId))]
        public virtual PMSUser Creator { get; set; } = null!;

        [Required]
        [Comment("Date when created on")]
        public DateTime CreatedOn { get; set; }

        [Required]
        [Comment("Date when last edited")]
        public DateTime EditedOn { get; set; }


        public virtual ICollection<ConsumableSupplier> ConsumablesSuppliers { get; set; } = new HashSet<ConsumableSupplier>();
        public virtual ICollection<SparepartSupplier> SparepartsSuppliers { get; set; } = new HashSet<SparepartSupplier>();

        public bool IsDleted { get; set; }
    }
}
