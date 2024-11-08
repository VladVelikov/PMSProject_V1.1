using System.ComponentModel.DataAnnotations;
using static PMS.Common.EntityValidationConstants.CityAndCountryConstants;

namespace PMSWeb.ViewModels.CityVM
{
    public class CityCreateViewModel
    {
        [Required]
        [MinLength(CityCountryNameMinLength)]
        [MaxLength(CityCountryNameMaxLength)]
        public string Name { get; set; } = null!;

    }
}
