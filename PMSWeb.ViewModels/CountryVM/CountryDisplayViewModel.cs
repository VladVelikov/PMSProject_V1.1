using System.ComponentModel.DataAnnotations;
using static PMS.Common.EntityValidationConstants.CityAndCountryConstants;

namespace PMSWeb.ViewModels.CountryVM
{
    public class CountryDisplayViewModel
    {
        public string? Name { get; set; } = null!;

        public string? CreatedOn { get; set; }

        public string? CountryId { get; set; }  
    }
}
