using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration.UserSecrets;
using PMS.Data;
using PMS.Data.Models;
using PMS.Data.Repository.Interfaces;
using PMS.Services.Data.Interfaces;
using PMSWeb.ViewModels.Consumable;
using System.Security.Claims;

namespace PMSWeb.Controllers
{

    public class ConsumableController(IRepository<Consumable, Guid> consumables, IConsumableService consumableService) : Controller
    {

        public async Task<IActionResult> Index()
        {
            return View();
        }

        public async Task<IActionResult> Select()
        {
            //var models = await consumables.GetAllAsQueryable()
            //    .Where(x => x.IsDeleted == false)
            //    .AsNoTracking()
            //    .Select(x => new ConsumableDisplayViewModel() {
            //        ConsumableId = x.ConsumableId.ToString(),
            //        Name = x.Name,
            //        Units = x.Units,
            //        Description = x.Description,
            //        Price = x.Price,
            //        ROB = x.ROB,
            //        EditedOn = x.EditedOn
            //    })
            //    .OrderByDescending(x => x.EditedOn)
            //    .ThenBy(x => x.Name)
            //    .ToListAsync();
            var models = await consumableService.GetListOfViewModelsAsync();
            return View(models);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var model = new ConsumableCreateViewModel();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ConsumableCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            if (GetUserId() == null)
            {
                return View(model);
            }

            Consumable consumable = new Consumable()
            {
                Name = model.Name,
                Units = model.Units,
                Description = model.Description,
                Price = model.Price,
                ROB = model.ROB,
                CreatorId = GetUserId() ?? string.Empty,
                CreatedOn = DateTime.Now,
                EditedOn = DateTime.Now,
                IsDeleted = false
            };
            await consumables.AddAsync(consumable);
            return RedirectToAction(nameof(Select));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var model = await consumables.GetAllAsQueryable() 
                .AsNoTracking()
                .Where(x => x.IsDeleted == false)
                .Where(x => x.ConsumableId.ToString() == id)
                .Select(x => new ConsumableEditViewModel() {
                    ConsumableId = x.ConsumableId.ToString(),
                    Name = x.Name,
                    Units = x.Units,
                    Description = x.Description,
                    Price = x.Price,
                    ROB = x.ROB
                })
                .FirstOrDefaultAsync();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ConsumableEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            if (GetUserId() == null)
            {
                return View(model);
            }
            Consumable? consToEdit = await consumables.GetAllAsQueryable()
               .Where(x => x.IsDeleted == false)
               .Where(x => x.ConsumableId.ToString() == model.ConsumableId)
               .FirstOrDefaultAsync();

            if (consToEdit == null)
            {
                return RedirectToAction(nameof(Index));
            }

            consToEdit.Name = model.Name;
            consToEdit.Units = model.Units;
            consToEdit.Description = model.Description;
            consToEdit.Price = model.Price;
            consToEdit.ROB = model.ROB;
            consToEdit.CreatorId = GetUserId() ?? string.Empty;
            consToEdit.IsDeleted = false;
            consToEdit.EditedOn = DateTime.Now;

            await consumables.UpdateAsync(consToEdit);

            return RedirectToAction(nameof(Select));
        }

        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            var model = await consumables.GetAllAsQueryable()
                .Where(x => x.IsDeleted == false)
                .Where(x => x.ConsumableId.ToString() == id)
                .AsNoTracking()
                .Select(x => new ConsumableDetailsViewModel() {
                    ConsumableId = x.ConsumableId.ToString(),
                    Name = x.Name,
                    Units = x.Units,
                    Description = x.Description,
                    Price = x.Price.ToString("C"),
                    ROB = x.ROB,
                    CreatedOn = x.CreatedOn,
                    EditedOn = x.EditedOn,
                    CreatorId = x.CreatorId,
                    CreatorName = x.Creator.UserName ?? string.Empty,
                })
                .FirstOrDefaultAsync();

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            var model = await consumables.GetAllAsQueryable()
                .Where(x => x.IsDeleted == false)
                .Where(x => x.ConsumableId.ToString() == id)
                .AsNoTracking()
                .Select(x => new ConsumableDeleteViewModel() {
                    ConsumableId = x.ConsumableId.ToString(),
                    Name = x.Name,
                    CreatedOn = x.CreatedOn,
                    EditedOn = x.EditedOn,
                    CreatorId = x.CreatorId,
                    CreatorName = x.Creator.UserName ?? string.Empty
                })
                .FirstOrDefaultAsync();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(ConsumableDeleteViewModel model)
        {
            Consumable? consToDelete = await consumables.GetAllAsQueryable()
                .Where(x => x.IsDeleted == false)
                .Where(x => x.ConsumableId.ToString() == model.ConsumableId)
                .FirstOrDefaultAsync();
            if (consToDelete != null)
            {
                consToDelete.IsDeleted = true;
                await consumables.UpdateAsync(consToDelete);  
            }
            return RedirectToAction(nameof(Select));
        }

        private string? GetUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier);
        }

    }
}
