using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PMS.Services.Data.Interfaces;
using PMSWeb.ViewModels.SparepartVM;
using System.Security.Claims;

namespace PMSWeb.Controllers
{
    [Authorize]
    public class SparepartController(ISparepartService sparesService) : Controller
    {
        [HttpGet]    
        public async Task<IActionResult> Select()
        {
            var sparesList = await sparesService.GetListOfViewModelsAsync();
            return View(sparesList);
        }
            
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var model = await sparesService.GetItemForCreateAsync();
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
            bool isCreated = await sparesService.CreateSparepartAsync(model, GetUserId()!);
            return RedirectToAction(nameof(Select));
        }
            
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var editModel = await sparesService.GetItemForEditAsync(id);
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
            bool isEdited = await sparesService.SaveItemToEditAsync(model, GetUserId()!);
            return RedirectToAction(nameof(Select));
        }

            
        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            var model = await sparesService.GetDetailsAsync(id);
            return View(model);
        }
        
        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            var model = await sparesService.GetItemToDeleteAsync(id);
            return View(model);
        }
        
        [HttpPost]
        public async Task<IActionResult> Delete(SparepartDeleteViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("ModelIsNotValid","Crushes");
            }
            bool isDeleted = await sparesService.ConfirmDeleteAsync(model);
            return RedirectToAction(nameof(Select));
        }
          
        private string? GetUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier);
        }
    }
}
