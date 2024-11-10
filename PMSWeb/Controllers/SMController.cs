using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PMS.Data;
using PMS.Data.Models;
using PMS.Services.Data.Interfaces;
using PMSWeb.ViewModels.CommonVM;
using PMSWeb.ViewModels.SM;
using System.Security.Claims;
using static PMS.Common.EntityValidationConstants;
using static PMS.Common.EntityValidationConstants.MaintenanceConstants;

namespace PMSWeb.Controllers
{
    public class SMController(PMSDbContext context, ISMService smService) : Controller
    {
        public async Task<IActionResult> Select()
        {
           var smList = await smService.GetListOfViewModelsAsync();
           return View(smList);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var model = await smService.GetItemForCreateAsync(); 
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

            bool isCreated = await smService.CreateSpecificMaintenanceAsync(model, GetUserId()!);
            return RedirectToAction(nameof(Select));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var model = await smService.GetItemForEditAsync(id);
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

            bool isEdited = await smService.SaveItemToEditAsync(model, GetUserId()!);
            return RedirectToAction(nameof(Select));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            var model = await smService.GetItemToDeleteAsync(id);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(SMDeleteViewModel model)
        {
            if (model == null || !ModelState.IsValid || model.SmId == null)
            {
                //Don't delete
                return RedirectToAction("ModelNotFound","Crushes");
            }
            bool isDeleted = await smService.ConfirmDeleteAsync(model);
            return RedirectToAction(nameof(Select));
        }

        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            var model = await smService.GetDetailsAsync(id);
            return View(model);
        }

        public string? GetUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier);
        }
    }
}
