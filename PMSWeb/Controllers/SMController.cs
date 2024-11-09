using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PMS.Data;
using PMS.Data.Models;
using PMSWeb.ViewModels.CommonVM;
using PMSWeb.ViewModels.SM;
using System.Security.Claims;
using static PMS.Common.EntityValidationConstants;
using static PMS.Common.EntityValidationConstants.MaintenanceConstants;

namespace PMSWeb.Controllers
{
    public class SMController(PMSDbContext context) : Controller
    {
        public async Task<IActionResult> Select()
        {
            var smss = await context
                .SpecificMaintenances
                .Where(x => !x.IsDeleted)
                .Include(x=>x.Equipment)
                .OrderByDescending(x => x.EditedOn)
                .AsNoTracking()
                .Select(x => new SMDisplayViewModel()
                {
                    SpecMaintId = x.SpecMaintId.ToString(),
                    Name = x.Name,
                    Description = x.Description,
                    Equipment = x.Equipment.Name,
                    LastCompletedDate = x.LastCompletedDate.ToString(PMSRequiredDateFormat),
                    Interval = x.Interval.ToString(),
                    ResponsiblePosition = x.ResponsiblePosition.ToString()
                })
                .ToListAsync();

            return View(smss);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var model = new SMCreateViewModel();
            var equipments = await context
                .Equipments
                .Where(x=>!x.IsDeleted)
                .AsNoTracking()
                .Select(x=> new PairViewModel() { 
                    Name = x.Name,  
                    Id = x.EquipmentId.ToString()
                })
                .ToListAsync();  
            model.Equipments = equipments;  
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(SMCreateViewModel model)
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

            SpecificMaintenance sm = new()
            {
                Name = model.Name,
                Description = model.Description,
                EquipmentId = Guid.Parse(model.EquipmentId),    
                LastCompletedDate = DateTime.Now,
                Interval = model.Interval,
                ResponsiblePosition = model.ResponsiblePosition,
                CReatorId = GetUserId()!,
                CreatedOn = DateTime.Now,
                EditedOn = DateTime.Now,
                IsDeleted = false
            };
            await context.SpecificMaintenances.AddAsync(sm);
            await context.SaveChangesAsync();

            return RedirectToAction(nameof(Select));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var model = await context
                .SpecificMaintenances
                .Where(x => !x.IsDeleted)
                .Where(x => x.SpecMaintId.ToString().ToLower() == id.ToLower())
                .AsNoTracking()
                .Select(x => new SMEditViewModel()
                {
                    Name = x.Name,
                    Description = x.Description,
                    Interval = x.Interval,
                    ResponsiblePosition = x.ResponsiblePosition,
                    SMId = x.SpecMaintId.ToString(),
                })
                .FirstOrDefaultAsync();
            if (model == null)
            {
                return RedirectToAction(nameof(Select));
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(SMEditViewModel model)
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

            var sm = await context
                .SpecificMaintenances
                .Where(x => !x.IsDeleted)
                .Where(x => x.SpecMaintId.ToString().ToLower() == model.SMId.ToLower())
                .FirstOrDefaultAsync();
            if (sm == null)
            {
                // Don't edit the record
                return RedirectToAction(nameof(Select));
            }
            // Edit the RM record
            sm.Name = model.Name;
            sm.Description = model.Description;
            sm.ResponsiblePosition = model.ResponsiblePosition;
            sm.Interval = model.Interval;
            sm.EditedOn = DateTime.Now;
            await context.SaveChangesAsync();

            return RedirectToAction(nameof(Select));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            var model = await context
                .SpecificMaintenances
                .Where(x => !x.IsDeleted)
                .Where(x => x.SpecMaintId.ToString().ToLower() == id.ToLower())
                .AsNoTracking()
                .Select(x => new SMDeleteViewModel()
                {
                    Name = x.Name,
                    Description = x.Description,
                    CreatedOn = x.CreatedOn.ToString(PMSRequiredDateFormat),
                    SmId = x.SpecMaintId.ToString()
                })
                .FirstOrDefaultAsync();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(SMDeleteViewModel model)
        {
            if (model == null || !ModelState.IsValid || model.SmId == null)
            {
                //Don't delete
                return RedirectToAction(nameof(Select));
            }
            var delModel = await context
                .SpecificMaintenances
                .Where(x => !x.IsDeleted)
                .Where(x => x.SpecMaintId.ToString().ToLower() == model.SmId.ToLower())
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
                .SpecificMaintenances
                .Where(x => !x.IsDeleted)
                .Where(x => x.SpecMaintId.ToString().ToLower() == id.ToLower())
                .AsNoTracking()
                .Select(x => new SMDetailsViewModel()
                {
                    SpecMaintId = x.SpecMaintId.ToString(),
                    Name = x.Name,
                    Description = x.Description,
                    Equipment = x.Equipment.Name,
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

            return View(model);
        }

        public string? GetUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier);
        }
    }
}
