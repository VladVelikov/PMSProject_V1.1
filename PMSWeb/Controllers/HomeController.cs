using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PMS.Data;
using PMS.Services.Data.Interfaces;
using PMSWeb.ViewModels;
using PMSWeb.ViewModels.CommonVM;
using System.Diagnostics;

namespace PMSWeb.Controllers
{
    public class HomeController(IStatisticService statisticService) : BasicController
    {
        [HttpGet]
        public IActionResult Index()
        {
            if (User.Identity!.IsAuthenticated)
            {
                return RedirectToAction(nameof(Dashboard));
            }
            return View();
        }

        [HttpGet]
        public IActionResult Dashboard()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Statistics()
        {
            var model = await statisticService.GetStatisticVieModelAsync();
            if (model == null)
            {
                return RedirectToAction("NotFound", "Crushes");
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult UserGuide()
        {
            return View();
        }

        [HttpGet]
        public IActionResult CreatorPage()
        {
            return View();
        }

        [HttpGet]
        public IActionResult CreatorHelpNotes()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Select()  // TO DO - erase rerouting after error handling module completed
        {
            return RedirectToAction(nameof(CreatorPage));
        }

        [HttpGet]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        public IActionResult StatusPageHandler(int code)
        {
            if(code == 404)
               return View("View404");

            if (code == 500)
                return View("View500");

            ViewBag.Code = code;    
            return View();
        }

    }
}
