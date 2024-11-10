using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PMS.Data;
using PMS.Data.Models;
using PMS.Services.Data.Interfaces;
using PMSWeb.ViewModels.RM;
using System.Security.Claims;
using static PMS.Common.EntityValidationConstants;

namespace PMSWeb.Controllers
{
    [Authorize]
    public class RMController(IRMService rmService) : Controller
    {
        public async Task<IActionResult> Select()
        {
            var rmList = await rmService.GetListOfViewModelsAsync();
            return View(rmList);
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
            bool isCreated = await rmService.CreateMakerAsync(model, GetUserId()!);
            return RedirectToAction(nameof(Select));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var model = await rmService.GetItemForEditAsync(id);
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

            bool isEdited = await rmService.SaveItemToEditAsync(model, GetUserId());
            return RedirectToAction(nameof(Select));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            var model = await rmService.GetItemToDeleteAsync(id);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(RMDeleteViewModel model)
        {
            if (model==null || !ModelState.IsValid || model.RmId == null)
            {
                //Don't delete
                return RedirectToAction("NotDeleted", "Crushes");
            }
            
            bool isDeleted = await rmService.ConfirmDeleteAsync(model);
            return RedirectToAction(nameof(Select));
        }

        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            var model = await rmService.GetDetailsAsync(id);  
            return View(model);
        }

        public string? GetUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier);
        }

    }
}
