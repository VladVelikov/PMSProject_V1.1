using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Security.Claims;
using static PMS.Common.EntityValidationConstants;

namespace PMSWeb.Controllers
{
    public class BasicController : Controller
    {
        internal string? GetUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        protected bool IsValidGuid(string id)
        {
            if(string.IsNullOrEmpty(id)) return false;

            if (Guid.TryParse(id, out var idGuid))
            {
                return true;
            }
                return false;
        }

        protected bool IsValidDecimal(string number)
        {
            if (string.IsNullOrEmpty(number)) return false;

            if (decimal.TryParse(number, out var numberDecimal))
            {
                return true;
            }
            return false;
        }

        protected bool IsValidDouble(string number)
        {
            if (string.IsNullOrEmpty(number)) return false;

            if (double.TryParse(number, out var numberDouble))
            {
                return true;
            }
            return false;
        }

        protected bool IsValidDate(string dateTime)
        {
            if (string.IsNullOrEmpty(dateTime)) return false;

            DateTime correctDate;
            bool result = DateTime.TryParse(dateTime, out correctDate);
            if (result)
            {
                return true;
            }
            return false;
        }

        protected bool IsDateTimeValid(string dateTime)
        {
            if (string.IsNullOrEmpty(dateTime)) return false;

            DateTime correctDateTime;
            bool result = DateTime.TryParseExact(dateTime, PMSRequiredDateTimeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out correctDateTime);
            if (result)
            {
                return true;
            }
            return false;
        }

    }
}
