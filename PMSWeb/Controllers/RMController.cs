using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using Microsoft.EntityFrameworkCore;
using PMS.Data;
using PMS.Data.Models;
using PMS.Data.Models.Enums;
using PMSWeb.ViewModels.RM;
using System.Security.Claims;
using static PMS.Common.EntityValidationConstants;

namespace PMSWeb.Controllers
{
    [Authorize]
    public class RMController(PMSDbContext context) : Controller
    {
        public async Task<IActionResult> Select()
        {
            var rmss = await context
                .RoutineMaintenances
                .Where(x => !x.IsDeleted)
                .OrderByDescending(x=>x.EditedOn)
                .AsNoTracking()
                .Select(x => new RMDisplayViewModel() {
                    Name = x.Name,  
                    Description = x.Description,
                    LastCompletedDate = x.LastCompletedDate.ToString(PMSRequiredDateFormat),
                    Interval = x.Interval.ToString(),
                    ResponsiblePosition = x.ResponsiblePosition.ToString()
                })
                .ToListAsync();
            return View(rmss);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Positions = PMSPositions.ToList();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(RMCreateViewModel model)
        {
            if (GetUserId == null)
            {
                return RedirectToAction(nameof(Select));
            }
            RoutineMaintenance rm = new()
            {
                Name = model.Name,
                Description = model.Description,
                LastCompletedDate = DateTime.Now,
                Interval = model.Interval,
                ResponsiblePosition= model.ResponsiblePosition,
                CReatorId = GetUserId()!,
                CreatedOn = DateTime.Now,
                EditedOn = DateTime.Now,
                IsDeleted = false
            };
            await context.RoutineMaintenances.AddAsync(rm);
            await context.SaveChangesAsync();

            return RedirectToAction(nameof(Select));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(RMEditViewModel model)
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Delete(RMDeleteViewModel model)
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            return View();
        }

        public string? GetUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier);
        }

    }
}
