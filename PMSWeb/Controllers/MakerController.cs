using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PMS.Services.Data.Interfaces;
using PMSWeb.ViewModels.Maker;

namespace PMSWeb.Controllers
{
    [Authorize]
    public class MakerController(IMakerService makerService) : BasicController
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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MakerCreateViewModel model)
        {
            if (!ModelState.IsValid || string.IsNullOrWhiteSpace(model.MakerName))
            {
                return View(model);
            }
            if (GetUserId() == null || !IsValidGuid(GetUserId()!))
            {
                return RedirectToAction("WrongData", "Crushes");
            }
            bool isCreated = await makerService.CreateMakerAsync(model, GetUserId()!);
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
            var model = await makerService.GetItemForEditAsync(id);
            if (model == null || string.IsNullOrWhiteSpace(model.MakerName))
            {
                return RedirectToAction("NotFound", "Crushes");
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(MakerEditViewModel model)
        {
            if (!ModelState.IsValid || string.IsNullOrWhiteSpace(model.MakerName))
            {
                return View(model);
            }
            if (GetUserId() == null || !IsValidGuid(GetUserId()!))
            {
                return RedirectToAction("WrongData", "Crushes");
            }
            bool isEdited = await makerService.SaveItemToEditAsync(model, GetUserId()!);
            if (!isEdited)
            {
                return RedirectToAction("NotEdited", "Crushes");
            }
            return RedirectToAction(nameof(Select));
        }

        public async Task<IActionResult> Details(string id)
        {
            var model = await makerService.GetDetailsAsync(id);   
            return View(model);
        }

        [Authorize(Roles = "Manager")]
        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            if (!IsValidGuid(id))
            {
                return RedirectToAction("WrongData", "Crushes");
            }
            var model = await makerService.GetItemToDeleteAsync(id);
            if (model == null || string.IsNullOrWhiteSpace(model.MakerId) || !IsValidGuid(model.MakerId))
            {
                return RedirectToAction("NotFound", "Crushes");
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(MakerDeleteViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            if (model == null || model.MakerId == null || !IsValidGuid(model.MakerId))
            {
                return RedirectToAction("WrongData", "Crushes");
            }
            bool isDeleted = await makerService.ConfirmDeleteAsync(model);
            if (!isDeleted)
            {
                return RedirectToAction("NotDeleted", "Crushes");
            }
            return RedirectToAction(nameof(Select));
        }    
    }
}

