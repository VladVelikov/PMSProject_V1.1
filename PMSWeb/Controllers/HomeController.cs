using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PMS.Data;
using PMSWeb.ViewModels;
using System.Diagnostics;

namespace PMSWeb.Controllers
{
    public class HomeController(PMSDbContext context) : BasicController
    {
        public IActionResult Index()
        {
            if (User.Identity!.IsAuthenticated)
            {
                return RedirectToAction(nameof(Dashboard));
            }
            return View();
        }

        public async Task<IActionResult> Dashboard()
        {
            var completedReq = await context
                .Requisitions
                .Where(x => !x.IsDeleted)
                .Where(x => x.IsApproved)
                .CountAsync();
            ViewBag.CompletedRequisitions = completedReq;

            var budget = await context
                .Budget
                .OrderByDescending(x => x.LastChangeDate)
                .Select(x=>x.Ballance)
                .FirstAsync();
            ViewBag.Budget = budget;

            return View();
        }

        public IActionResult CreatorPage()
        {
            return View();
        }

        public IActionResult CreatorHelpNotes()
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


        //public string? GetUserId()
        //{
        //    return User.FindFirstValue(ClaimTypes.NameIdentifier)!.ToString();
        //}
    }
}
