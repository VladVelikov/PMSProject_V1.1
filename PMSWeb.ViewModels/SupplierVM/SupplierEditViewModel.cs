using PMSWeb.ViewModels.CommonVM;
using System.ComponentModel.DataAnnotations;
using static PMS.Common.EntityValidationConstants.SupplierConstatnts;

namespace PMSWeb.ViewModels.SupplierVM
{
    public class SupplierEditViewModel
    {
        public string SupplierId { get; set; }  

        [Required]
        [MinLength(SupplierNameMinLength)]
        [MaxLength(SupplierNameMaxLength)]
        public string Name { get; set; } = null!;

        [Required]
        [MinLength(SupplierAddressMinLength)]
        [MaxLength(SupplierAddressMaxLength)]
        public string Address { get; set; } = null!;

        [Required]
        [MinLength(SupplierEmailMinLength)]
        [MaxLength(SupplierEmailMaxLength)]
        public string Email { get; set; } = null!;

        [Required]
        [MinLength(SupplierPhoneMinLength)]
        [MaxLength(SupplierPhoneMaxLength)]
        public string PhoneNumber { get; set; } = null!;

        [Required]
        public string CityId { get; set; } = null!;

        [Required]
        public string CountryId { get; set; } = null!;

        public List<PairViewModel> Cities { get; set; } = new List<PairViewModel>();

        public List<PairViewModel> Countries { get; set; } = new List<PairViewModel>();
    }
}
