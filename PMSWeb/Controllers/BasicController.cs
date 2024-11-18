using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace PMSWeb.Controllers
{
    public class BasicController : Controller
    {
        internal string? GetUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier);
        }

    }
}
