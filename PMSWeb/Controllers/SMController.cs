using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PMS.Services.Data.Interfaces;
using PMSWeb.ViewModels.SM;
using static PMS.Common.EntityValidationConstants;

namespace PMSWeb.Controllers
{
    public class SMController(ISMService smService) : BasicController
    {
        public async Task<IActionResult> Select()
        {
            var smList = await smService.GetListOfViewModelsAsync();
            if (smList == null)
            {
                return RedirectToAction("EmptyList", "Crushes");
            }
            return View(smList);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var model = await smService.GetItemForCreateAsync();
            if (model == null)
            {
                return RedirectToAction("WrongData", "Crushes");
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(SMCreateViewModel model)
        {
            if (GetUserId() == null || !IsValidGuid(GetUserId()!))
            {
                return RedirectToAction("WrongData", "Crushes");
            }

            if (!PMSPositions.Contains(model.ResponsiblePosition))
            {
                model.Equipments = (await smService.GetItemForCreateAsync()).Equipments;
                ModelState.AddModelError("ResponsiblePosition", $"The Responsible positions supported are: {string.Join(", ", PMSPositions)}");
                return View(model);
            }

            if (!ModelState.IsValid)
            {
                model.Equipments = (await smService.GetItemForCreateAsync()).Equipments;
                return View(model);
            }
            if (string.IsNullOrEmpty(model.ResponsiblePosition) ||
                string.IsNullOrWhiteSpace(model.Name) ||
                !IsValidInteger(model.Interval.ToString()))
            {
                return RedirectToAction("WrongData", "Crushes");
            }
            bool isCreated = await smService.CreateSpecificMaintenanceAsync(model, GetUserId()!);
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
            var model = await smService.GetItemForEditAsync(id);
            if (model == null || string.IsNullOrWhiteSpace(model.SMId))
            {
                return RedirectToAction("NotFound", "Crushes");
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(SMEditViewModel model)
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
            bool isEdited = await smService.SaveItemToEditAsync(model, GetUserId()!);
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
            var model = await smService.GetItemToDeleteAsync(id);
            if (model == null || string.IsNullOrWhiteSpace(model.SmId))
            {
                return RedirectToAction("NotFound", "Crushes");
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(SMDeleteViewModel model)
        {
            if (model == null || !ModelState.IsValid || model.SmId == null)
            {
                //Don't delete
                return RedirectToAction("ModelNotFound", "Crushes");
            }
            bool isDeleted = await smService.ConfirmDeleteAsync(model);
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
            var model = await smService.GetDetailsAsync(id);
            if (model == null || string.IsNullOrWhiteSpace(model.SpecMaintId))
            {
                return RedirectToAction("NotFound", "Crushes");
            }
            return View(model);
        }
    }
}
