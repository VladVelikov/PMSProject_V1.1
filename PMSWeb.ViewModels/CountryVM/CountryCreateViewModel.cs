using System.ComponentModel.DataAnnotations;
using static PMS.Common.EntityValidationConstants.CityAndCountryConstants;

namespace PMSWeb.ViewModels.CountryVM
{
    public class CountryCreateViewModel
    {
        [Required]
        [MinLength(CityCountryNameMinLength)]
        [MaxLength(CityCountryNameMaxLength)]
        public string Name { get; set; } = null!;

    }
}
