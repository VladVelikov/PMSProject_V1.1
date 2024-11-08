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
                    RoutMaintId = x.RoutMaintId.ToString(),
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
            return View(new RMCreateViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(RMCreateViewModel model)
        {
            if (GetUserId == null)
            {
                return RedirectToAction(nameof(Select));
            }

            if (!PMSPositions.Contains(model.ResponsiblePosition))
            {
                ModelState.AddModelError("ResponsiblePosition", $"The Responsible positions supported are: {string.Join(", ",PMSPositions)}");
                return View(model);
            }

            if (!ModelState.IsValid)
            {
                return View(model);
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
            var model = await context
                .RoutineMaintenances
                .Where(x => !x.IsDeleted)
                .Where(x => x.RoutMaintId.ToString().ToLower() == id.ToLower())
                .AsNoTracking()
                .Select(x=> new RMEditViewModel() { 
                    Name = x.Name,
                    Description = x.Description,
                    Interval = x.Interval,
                    ResponsiblePosition = x.ResponsiblePosition,
                    RMId = x.RoutMaintId.ToString(),
                })
                .FirstOrDefaultAsync();
            if (model == null)
            {
                return RedirectToAction(nameof(Select));  
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(RMEditViewModel model)
        {
            if (GetUserId == null)
            {
                return RedirectToAction(nameof(Select));
            }

            if (!PMSPositions.Contains(model.ResponsiblePosition))
            {
                ModelState.AddModelError("ResponsiblePosition", $"The Responsible positions supported are: {string.Join(", ", PMSPositions)}");
                return View(model);
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var rm = await context
                .RoutineMaintenances
                .Where(x => !x.IsDeleted)
                .Where(x => x.RoutMaintId.ToString().ToLower() == model.RMId.ToLower())
                .FirstOrDefaultAsync();
            if (rm == null)
            {
                // Don't edit the record
                return RedirectToAction(nameof(Select));
            }
            // Edit the RM record
            rm.Name = model.Name;
            rm.Description = model.Description;
            rm.ResponsiblePosition = model.ResponsiblePosition;
            rm.Interval = model.Interval;
            rm.EditedOn = DateTime.Now;
            await context.SaveChangesAsync();

            return RedirectToAction(nameof(Select));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            var model = await context
                .RoutineMaintenances
                .Where (x => !x.IsDeleted)  
                .Where(x=>x.RoutMaintId.ToString().ToLower() == id.ToLower())
                .AsNoTracking()
                .Select(x=> new RMDeleteViewModel() {
                    Name = x.Name,
                    Description = x.Description,
                    CreatedOn = x.CreatedOn.ToString(PMSRequiredDateFormat),
                    RmId = x.RoutMaintId.ToString()
                })
                .FirstOrDefaultAsync();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(RMDeleteViewModel model)
        {
            if (model==null || !ModelState.IsValid || model.RmId == null)
            {
                //Don't delete
                return RedirectToAction(nameof(Select));
            }
            var delModel = await context
                .RoutineMaintenances
                .Where(x => !x.IsDeleted)
                .Where(x => x.RoutMaintId.ToString().ToLower() == model.RmId.ToLower())
                .FirstOrDefaultAsync();
            if (delModel != null)
            {
                // Execute soft delete
                delModel.IsDeleted = true;  
                await context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Select));
        }

        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            var model = await context
                .RoutineMaintenances
                .Where (x => !x.IsDeleted)
                .Where(x => x.RoutMaintId.ToString().ToLower() == id.ToLower())
                .AsNoTracking()
                .Select(x=> new RMDetailsViewModel() {
                    RoutMaintId = x.RoutMaintId.ToString(),
                    Name = x.Name,
                    Description = x.Description,
                    LastCompletedDate = x.LastCompletedDate.ToString(PMSRequiredDateFormat),
                    Interval = x.Interval.ToString(),
                    ResponsiblePosition = x.ResponsiblePosition,
                    CreatorName = x.Creator.UserName ?? string.Empty,
                    CreatedOn = x.CreatedOn.ToString(PMSRequiredDateFormat),
                    EditedOn = x.EditedOn.ToString(PMSRequiredDateFormat),
                })
                .FirstOrDefaultAsync();
            
            if (model == null)
            {
                return RedirectToAction(nameof(Select));
            }

            var equipments = await context
                .RoutineMaintenancesEquipments
                .Where(x=>x.RoutineMaintenanceId.ToString().ToLower() == id.ToLower())
                .AsNoTracking()
                .Select(x=>x.Equipment.Name)
                .ToListAsync();
            if (equipments.Any())
            {
                model.Equipments = equipments;
            }

            return View(model);
        }

        public string? GetUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier);
        }

    }
}
