using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PMS.Data;
using PMS.Data.Models;
using PMSWeb.ViewModels.CommonVM;
using PMSWeb.ViewModels.SparepartVM;
using System.Security.Claims;
using static PMS.Common.EntityValidationConstants;

namespace PMSWeb.Controllers
{
    [Authorize]
    public class SparepartController(PMSDbContext context) : Controller
    {

            
        public async Task<IActionResult> Select()
            {
                var models = await context
                .Spareparts
                .Where(x => x.IsDeleted == false)
                .AsNoTracking()
                .OrderByDescending(x => x.EditedOn)
                .ThenBy(x => x.SparepartName)
                .Select(x => new SparepartDisplayViewModel()
                {
                   SparepartId = x.SparepartId.ToString(),
                   Name = x.SparepartName,
                   Units = x.Units,
                   Description = x.Description,
                   Equipment = x.Equipment.Name,
                   Price = x.Price.ToString("C"),
                   ROB = x.ROB.ToString(),
                })
               .ToListAsync();

                return View(models);
            }

            
        [HttpGet]
            
        public async Task<IActionResult> Create()
        {
            var model = new SparepartCreateViewModel();
            var equipments = await context
               .Equipments
               .Where(x => !x.IsDeleted)
               .AsNoTracking()
               .Select(x => new PairViewModel()
               {
                   Name = x.Name,
                   Id = x.EquipmentId.ToString()
               })
               .ToListAsync();
            if (equipments != null)
                
            {
                    model.Equipments = equipments;  
                
            }
               return View(model);
        }

            
        [HttpPost]
        public async Task<IActionResult> Create(SparepartCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                    return View(model);
            }
            if (GetUserId() == null)
            {
                    return View(model);
            }
            Sparepart spare = new() {
                    SparepartName = model.Name, 
                    Description = model.Description ?? string.Empty,
                    ROB = model.ROB,
                    Price = model.Price,
                    Units = model.Units,    
                    EquipmentId = Guid.Parse(model.EquipmentId),
                    CreatedOn = DateTime.Now,
                    EditedOn = DateTime.Now,
                    ImageURL = model.ImageUrl,
                    CreatorId = GetUserId()!,
                    IsDeleted = false
            };
            await context.Spareparts.AddAsync(spare);
            await context.SaveChangesAsync(); 
            return RedirectToAction(nameof(Select));
        }

            
        [HttpGet]
        
        public async Task<IActionResult> Edit(string id)
        {
            var editModel = await context
                .Spareparts
                .Where(x=>!x.IsDeleted)
                .Where(x=>x.SparepartId.ToString().ToLower() == id.ToLower())
                .Select(x=> new SparepartEditViewModel() {
                    SparepartId = x.SparepartId.ToString(),
                    Name = x.SparepartName,
                    Description= x.Description ?? string.Empty,
                    Price= x.Price,
                    Units = x.Units,
                    ROB= x.ROB,
                    ImageUrl = x.ImageURL
                })
                .AsNoTracking()
                .FirstOrDefaultAsync(); 
            return View(editModel);
        }
        
        [HttpPost]
        public async Task<IActionResult> Edit(SparepartEditViewModel model)
        {
            if (!ModelState.IsValid || model == null || model.SparepartId == null)
            {
                    return View(model);
            }
            if (GetUserId() == null)
            {
                return View(model);
            }
            var editModel = await context
                .Spareparts
                .Where(x => !x.IsDeleted)
                .Where(x => x.SparepartId.ToString().ToLower() == model.SparepartId.ToLower())
                .FirstOrDefaultAsync();
            if (editModel == null)
            {
                return RedirectToAction(nameof(Select));
            }
            
                editModel.SparepartName = model.Name;
                editModel.Description = model.Description ?? string.Empty;
                editModel.Price = model.Price;
                editModel.Units = model.Units;
                editModel.ROB = model.ROB;
                editModel.ImageURL = model.ImageUrl;
                editModel.EditedOn = DateTime.Now;
            await context.SaveChangesAsync(); 
            return RedirectToAction(nameof(Select));
        }

            
        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            var model = await context
               .Spareparts
               .Where(x => !x.IsDeleted)
               .Where(x => x.SparepartId.ToString().ToLower() == id.ToLower())
               .Select(x => new SparepartDetailsViewModel()
               {
                   SparepartId = x.SparepartId.ToString(),
                   Name = x.SparepartName,
                   Description = x.Description ?? string.Empty,
                   Price = x.Price.ToString("C"),
                   Units = x.Units,
                   ROB = x.ROB.ToString(),
                   CreatedOn = x.CreatedOn.ToString(PMSRequiredDateFormat),
                   CreatorName = x.Creator.UserName,
                   ImageURL = x.ImageURL
               })
               .AsNoTracking()
               .FirstOrDefaultAsync();
            return View(model);
        }
        
        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            var model = await context
                .Spareparts
                .AsNoTracking()
                .Where(x=>!x.IsDeleted)
                .Where(x=>x.SparepartId.ToString().ToLower() == id.ToLower())
                .Select(x=> new SparepartDeleteViewModel() {
                    SparepartId = x.SparepartId.ToString(),
                    Name = x.SparepartName,
                    Description = x.Description,
                    CreatedOn = x.CreatedOn.ToString(PMSRequiredDateFormat) 
                })
                .FirstOrDefaultAsync();
                return View(model);
        }
        
        [HttpPost]
        public async Task<IActionResult> Delete(SparepartDeleteViewModel model)
        {
            if (model == null || model.SparepartId == null)
            {
               return RedirectToAction(nameof(Select));
            }
            var deleteModel = await context
                .Spareparts
                .Where(x => !x.IsDeleted)
                .Where(x => x.SparepartId.ToString().ToLower() == model.SparepartId.ToLower())
                .FirstOrDefaultAsync();

            if (deleteModel != null) 
                deleteModel.IsDeleted = true;   
            await context.SaveChangesAsync(); 

            return RedirectToAction(nameof(Select));
        }

          
        private string? GetUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier);
        }
        
    }
}
