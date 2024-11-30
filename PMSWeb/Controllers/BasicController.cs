using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Security.Claims;
using System.Text.RegularExpressions;
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

        protected bool IsValidInteger(string number)
        {
            if (string.IsNullOrEmpty(number)) return false;

            if (int.TryParse(number, out var numberDouble))
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

        public static bool IsSafeUrl(string url)   // method to examin possible bridges through URL fields
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                return false;
            }
            try
            {
                var uri = new Uri(url, UriKind.RelativeOrAbsolute);
                if (uri.IsAbsoluteUri)
                {
                    if (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps)
                    {
                        return false;
                    }
                }

                var unsafePatterns = new[]
                {
                    @"^javascript:", // JavaScript 
                    @"^data:",       // Data 
                    @"<script>",     // Tag for script
                    @"[\s<>{}]"      // Some dangerous signs
                };

                foreach (var pattern in unsafePatterns)
                {
                    if (Regex.IsMatch(url, pattern, RegexOptions.IgnoreCase))
                    {
                        return false;
                    }
                }
                return true; 
            }
            catch
            {
                // If Uri Parse Failed => the URL is not ok.
                return false;
            }
        }

    }
}
