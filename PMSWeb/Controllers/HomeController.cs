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
    public class HomeController(PMSDbContext context) : Controller
    {

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        
        public async Task<IActionResult> Dashboard()
        {

            return View();
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
