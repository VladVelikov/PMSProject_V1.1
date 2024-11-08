using System.ComponentModel.DataAnnotations;
using static PMS.Common.EntityValidationConstants.CityAndCountryConstants;

namespace PMSWeb.ViewModels.CityVM
{
    public class CityDisplayViewModel
    {
        public string? Name { get; set; } = null!;

        public string? CreatedOn { get; set; }

        public string? CityId { get; set; }  
    }
}
