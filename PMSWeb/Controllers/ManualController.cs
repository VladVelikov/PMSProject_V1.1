using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PMS.Data;
using PMS.Data.Models;
using PMSWeb.ViewModels.Manual;
using PMSWeb.ViewModels.CommonVM;
using System.Security.Claims;
using static PMS.Common.EntityValidationConstants;

namespace PMSWeb.Controllers
{
    [Authorize]
    public class ManualController(PMSDbContext context) : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Select()
        {
            var manuals = await context
                .Manuals
                .Where(x => !x.IsDeleted)
                .Include(x=>x.Maker)
                .Include(x=>x.Equipment)
                .AsNoTracking()
                .Select(x => new ManualDisplayViewModel()
                {
                    ManualId = x.ManualId.ToString(),
                    ManualName = x.ManualName,
                    Maker = x.Maker.MakerName,
                    Equipment = x.Equipment!.Name ?? string.Empty
                })
                .ToListAsync();

            if (!manuals.Any())
            {
                return View(new List<ManualDisplayViewModel>());
            }

            return View(manuals);
        }

        [HttpGet]
        public async Task<IActionResult> Create(string URL)
        {
            var model = new ManualCreateViewModel();
            if (URL != null)
            {
                model.ContentURL = URL; 
            }
            model.Makers = await context
                .Makers
                .Where(x=>!x.IsDeleted)
                .AsNoTracking()
                .Select(x => new PairViewModel() 
                { 
                  Name = x.MakerName,
                  Id = x.MakerId.ToString()    
                })
                .ToListAsync();

            model.Equipments = await context
                .Equipments
                .Where(x => !x.IsDeleted)
                .AsNoTracking()
                .Select(x => new PairViewModel()
                {
                    Name= x.Name,
                    Id = x.EquipmentId.ToString()
                })
                .ToListAsync();

            return View(model);  
        }

        [HttpPost]
        public async Task<IActionResult> Create(ManualCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            string? userId = GetUserId();
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction(nameof(Select));    
            }

            Manual manual = new Manual()
            {
                ManualName = model.ManualName,
                MakerId = Guid.Parse(model.MakerId),
                EquipmentId = Guid.Parse(model.EquipmentId),
                CreatorId = userId!,
                CreatedOn = DateTime.Now,
                EditedOn = DateTime.Now,
                IsDeleted = false,
                ContentURL = model.ContentURL  
            };
            await context.Manuals.AddAsync(manual);
            await context.SaveChangesAsync();

            return RedirectToAction(nameof(Select));
        }

        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            var model = await context
                .Manuals
                .AsNoTracking()
                .Where(x => !x.IsDeleted)
                .Where(x => x.ManualId.ToString().ToLower() == id.ToLower())
                .Select(x => new ManualDetailsViewModel() {
                    URL = x.ContentURL,
                    Name = x.ManualName,
                    MakerName = x.Maker.MakerName,
                    EquipmentName = x.Equipment!.Name ?? string.Empty,
                })
                .FirstOrDefaultAsync();
            if (model == null)
            {
                return RedirectToAction(nameof(Select));
            }
           

            return View(model);
        }

        [HttpGet]
        public IActionResult Upload()
        {
            return View();
        }
       
        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile myFile)
        {
            if (myFile != null && myFile.Length > 0)
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "UploadedFiles", myFile.FileName);
                Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);
                
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await myFile.CopyToAsync(stream);
                }
                string shortPath = $"\\UploadedFiles\\{myFile.FileName}";
                return RedirectToAction("Create", new { URL = shortPath });
            }
            else
            {
                return RedirectToAction(nameof(Upload));
            }
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            var model = await context
                .Manuals
                .Where(x => !x.IsDeleted)
                .Where(x => x.ManualId.ToString().ToLower() == id.ToLower())
                .AsNoTracking()
                .Select(x => new ManualDeleteViewModel() {
                    ManualName = x.ManualName,
                    MakerName = x.Maker.MakerName,
                    EquipmentName = x.Equipment.Name,
                    CreatedOn = x.CreatedOn.ToString(PMSRequiredDateFormat),
                    ManualId = x.ManualId.ToString()
                })
                .FirstOrDefaultAsync();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(ManualDeleteViewModel model)
        {
            if (model != null && model.ManualId != null) 
            {
                var deleteModel = await context
                    .Manuals
                    .Where (x => !x.IsDeleted)  
                    .Where(x=>x.ManualId.ToString().ToLower() == model.ManualId.ToLower())
                    .FirstOrDefaultAsync();
                if (deleteModel != null)
                {
                       deleteModel.IsDeleted = true;
                }
                await context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Select));
        }

        public string? GetUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier)!.ToString();
        }

    }
}
