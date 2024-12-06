using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PMS.Services.Data.Interfaces;
using PMSWeb.ViewModels.RM;
using static PMS.Common.EntityValidationConstants;

namespace PMSWeb.Controllers
{
    [Authorize]
    public class RMController(IRMService rmService) : BasicController
    {
        public async Task<IActionResult> Select()
        {
            var rmList = await rmService.GetListOfViewModelsAsync();
            if (rmList == null)
            {
                return RedirectToAction("EmptyList", "Crushes");
            }
            return View(rmList);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new RMCreateViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(RMCreateViewModel model)
        {
            if (GetUserId() == null || !IsValidGuid(GetUserId()!))
            {
                return RedirectToAction("WrongData", "Crushes");
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

            if (string.IsNullOrEmpty(model.ResponsiblePosition) ||
                string.IsNullOrWhiteSpace(model.Name) ||
                !IsValidInteger(model.Interval.ToString()))
            {
                return RedirectToAction("WrongData", "Crushes");
            }
            bool isCreated = await rmService.CreateRMAsync(model, GetUserId()!);
            if (!isCreated)
            {
                return RedirectToAction("NotCreated", "Crushes");
            }
            return RedirectToAction(nameof(Select));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            if (!IsValidGuid(id))
            {
                return RedirectToAction("WrongData", "Crushes");
            }
            var model = await rmService.GetItemForEditAsync(id);
            if (model == null || string.IsNullOrWhiteSpace(model.RMId))
            {
                return RedirectToAction("NotFound", "Crushes");
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(RMEditViewModel model)
        {
            if (GetUserId() == null || !IsValidGuid(GetUserId()!))
            {
                return RedirectToAction("WrongData", "Crushes");
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
            if (string.IsNullOrWhiteSpace(model.Name) ||
               !IsValidInteger(model.Interval.ToString()))
            {
                return RedirectToAction("WrongData", "Crushes");
            }
            bool isEdited = await rmService.SaveItemToEditAsync(model, GetUserId()!);
            if (!isEdited)
            {
                return RedirectToAction("NotEdited", "Crushes");
            }
            return RedirectToAction(nameof(Select));
        }

        [Authorize(Roles = "Manager")]
        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            if (!IsValidGuid(id))
            {
                return RedirectToAction("WrongData", "Crushes");
            }
            var model = await rmService.GetItemToDeleteAsync(id);
            if (model == null || string.IsNullOrWhiteSpace(model.RmId))
            {
                return RedirectToAction("NotFound", "Crushes");
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(RMDeleteViewModel model)
        {
            if (model==null || !ModelState.IsValid || model.RmId == null)
            {
                //Don't delete
                return RedirectToAction("NotFound", "Crushes");
            }
            bool isDeleted = await rmService.ConfirmDeleteAsync(model);
            if (!isDeleted)
            {
                return RedirectToAction("NotDeleted", "Crushes");
            }
            return RedirectToAction(nameof(Select));
        }

        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            if (!IsValidGuid(id))
            {
                return RedirectToAction("WrongData", "Crushes");
            }
            var model = await rmService.GetDetailsAsync(id);  
            if (model == null || string.IsNullOrWhiteSpace(model.RoutMaintId))
            {
                return RedirectToAction("NotFound", "Crushes");
            }
            return View(model);
        }
    }
}
