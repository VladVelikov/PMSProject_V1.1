using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration.UserSecrets;
using PMS.Data;
using PMS.Data.Models;
using PMS.Data.Models.Identity;
using PMS.Data.Seeders;
using PMSWeb.ViewModels;
using System.Diagnostics;
using System.Security.Claims;

namespace PMSWeb.Controllers
{
    public class HomeController : Controller
    {

        public IActionResult Index()
        {
            if (User.Identity!.IsAuthenticated)
            {
                return RedirectToAction(nameof(Dashboard));
            }
            return View();
        }

        public IActionResult Dashboard()
        {
            return View();
        }

        public IActionResult CreatorPage()
        {
            return View();
        }
        public IActionResult Select()  // TO DO - erase rerouting after error handling module completed
        {
            return RedirectToAction(nameof(CreatorPage));
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        public string? GetUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier)!.ToString();
        }
    }
}
