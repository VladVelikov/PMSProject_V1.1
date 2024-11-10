using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PMS.Data;
using PMS.Services.Data.Interfaces;
using PMSWeb.ViewModels.Maker;
using System.Security.Claims;

namespace PMSWeb.Controllers
{
    [Authorize]
    public class MakerController(PMSDbContext context, IMakerService makerService) : Controller
    {
        public async Task<IActionResult> Select()
        {
            var makers = await makerService.GetListOfViewModelsAsync();
            return View(makers);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new MakerCreateViewModel());
        }
       
        [HttpPost]
        public async Task<IActionResult> Create(MakerCreateViewModel model)
        {
            bool isCreated = await makerService.CreateMakerAsync(model, GetUserId());
            return RedirectToAction(nameof(Select));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var model = await makerService.GetItemForEditAsync(id);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(MakerEditViewModel model)
        {
            bool isEdited = await makerService.SaveItemToEditAsync(model, GetUserId());
            return RedirectToAction(nameof(Select));
        }

        public async Task<IActionResult> Details(string id)
        {
            var model = await makerService.GetDetailsAsync(id);   
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            var model = await makerService.GetItemToDeleteAsync(id);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(MakerDeleteViewModel model)
        {
            bool isDeleted = await makerService.ConfirmDeleteAsync(model);    
            return RedirectToAction(nameof(Select));
        }    

        public string? GetUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier);
        }

       
    }
}

