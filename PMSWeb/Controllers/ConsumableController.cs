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
    [Authorize]
    public class ConsumableController(IRepository<Consumable, Guid> consumables, IConsumableService consumableService) : Controller
    {

        public async Task<IActionResult> Index()
        {
            return View();
        }
        
        public async Task<IActionResult> Select()
        {
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
           
            bool result = await consumableService.CreateConsumableAsync(model, GetUserId()!);
            if (!result)
            {
                return View(model);
            }
            return RedirectToAction(nameof(Select));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            ConsumableEditViewModel model = await consumableService.GetItemForEditAsync(id); 
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
           
            bool isEdited = await consumableService.SaveItemToEditAsync(model, GetUserId()!);
            if (!isEdited)
            {
                return View(model);
            }
            return RedirectToAction(nameof(Select));
        }

        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            var model = await consumableService.GetDetailsAsync(id);
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            ConsumableDeleteViewModel model = await consumableService.GetItemToDeleteAsync(id);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(ConsumableDeleteViewModel model)
        {
            bool result = await consumableService.ConfirmDeleteAsync(model);
            if (!result)
            {
                return View(model);
            }
            return RedirectToAction(nameof(Select));
        }

        private string? GetUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier);
        }

    }
}
