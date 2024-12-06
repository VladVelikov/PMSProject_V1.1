using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PMS.Services.Data.Interfaces;
using PMSWeb.ViewModels.SparepartVM;

namespace PMSWeb.Controllers
{
    [Authorize]
    public class SparepartController(ISparepartService sparesService) : BasicController
    {
        [HttpGet]    
        public async Task<IActionResult> Select()
        {
            var sparesList = await sparesService.GetListOfViewModelsAsync();
            if (sparesList == null)
            {
                return RedirectToAction("EmptyList", "Crushes");
            }
            return View(sparesList);
        }
            
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var model = await sparesService.GetItemForCreateAsync();
            if (model == null)
            {
                return RedirectToAction("WrongData", "Crushes");
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
            if (GetUserId() == null || !IsValidGuid(GetUserId()!))
            {
                return RedirectToAction("WrongData", "Crushes");
            }
            if (string.IsNullOrWhiteSpace(model.Name) ||
                !IsValidDouble(model.ROB.ToString()) ||
                !IsValidDecimal(model.Price.ToString()) ||
                string.IsNullOrWhiteSpace(model.Units) ||
                !IsValidGuid(model.EquipmentId))
            {
                return RedirectToAction("WrongData", "Crushes");
            }
            if (!string.IsNullOrWhiteSpace(model.ImageUrl))
            {
                if (!IsSafeUrl(model.ImageUrl))
                {
                    return RedirectToAction("WrongData", "Crushes");
                }
            }
            bool isCreated = await sparesService.CreateSparepartAsync(model, GetUserId()!);
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
            var editModel = await sparesService.GetItemForEditAsync(id);
            if (editModel == null || string.IsNullOrWhiteSpace(editModel.SparepartId))
            {
                return RedirectToAction("NotFound", "Crushes");
            }
            return View(editModel);
        }
        
        [HttpPost]
        public async Task<IActionResult> Edit(SparepartEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            if (GetUserId() == null || !IsValidGuid(GetUserId()!) || model == null || model.SparepartId == null)
            {
                return RedirectToAction("WrongData", "Crushes");
            }
            if (string.IsNullOrWhiteSpace(model.Name) ||
                !IsValidDouble(model.ROB.ToString()) ||
                !IsValidDecimal(model.Price.ToString()) ||
                string.IsNullOrWhiteSpace(model.Units)) 
            {
                return RedirectToAction("WrongData", "Crushes");
            }
            if (!string.IsNullOrWhiteSpace(model.ImageUrl))
            {
                if (!IsSafeUrl(model.ImageUrl))
                {
                    return RedirectToAction("WrongData", "Crushes");
                }
            }
            bool isEdited = await sparesService.SaveItemToEditAsync(model, GetUserId()!);
            if (!isEdited)
            {
                return RedirectToAction("NotEdited", "Crushes");
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
            var model = await sparesService.GetDetailsAsync(id);
            if (model == null || string.IsNullOrWhiteSpace(model.SparepartId))
            {
                return RedirectToAction("NotFound", "Crushes");
            }
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
            var model = await sparesService.GetItemToDeleteAsync(id);
            if (model == null || string.IsNullOrWhiteSpace(model.SparepartId))
            {
                return RedirectToAction("NotFound", "Crushes");
            }
            return View(model);
        }
        
        [HttpPost]
        public async Task<IActionResult> Delete(SparepartDeleteViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("ModelIsNotValid","Crushes");
            }
            if (model.SparepartId == null || !IsValidGuid(model.SparepartId))
            {
                return RedirectToAction("WrongData", "Crushes");
            }
            bool isDeleted = await sparesService.ConfirmDeleteAsync(model);
            if (!isDeleted)
            {
                return RedirectToAction("NotDeleted", "Crushes");
            }
            return RedirectToAction(nameof(Select));
        }
    }
}
