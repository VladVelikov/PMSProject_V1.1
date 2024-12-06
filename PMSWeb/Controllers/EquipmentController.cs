using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PMS.Services.Data.Interfaces;
using PMSWeb.ViewModels.Equipment;

namespace PMSWeb.Controllers
{
    [Authorize]
    public class EquipmentController(IEquipmentService equipmentService) : BasicController
    {
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Select()
        {
            var eqList = await equipmentService.GetListOfViewModelsAsync();   
            if (eqList.Count() == 0)
            {
                return RedirectToAction("EmptyList", "Crushes");
            }
            return View(eqList);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var model = await equipmentService.GetCreateModelAsync();
            if (model == null)
            {
                return RedirectToAction("NotFound", "Crushes");
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(EquipmentCreateViewModel model, List<Guid> RoutineMaintenances, List<Guid> Consumables)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            if (GetUserId() == null)
            {
                return RedirectToAction("WrongData", "Crushes");
            }
            if (!IsValidGuid(model.MakerId.ToString()) ||
                string.IsNullOrEmpty(model.Name) ||
                string.IsNullOrEmpty(model.Description))
            {
                return RedirectToAction("WrongData", "Crushes");
            }

            bool isCreated = await equipmentService.CreateEquipmentAsync(model,GetUserId()!, RoutineMaintenances, Consumables);
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
            var model = await equipmentService.GetItemForEditAsync(id);
            if (string.IsNullOrEmpty(model.EquipmentId))
            {
                return RedirectToAction("NotFound", "Crushes");
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EquipmentEditViewModel model,
            List<Guid> RoutineMaintenances,
            List<Guid> Consumables,
            List<Guid> AvailableRoutineMaintenances,
            List<Guid> AvailableConsumables)
        {
            if (!ModelState.IsValid)
            {
                return View(model); 
            }
            if (!IsValidGuid(model.MakerId.ToString()) ||
                GetUserId() == null ||
                string.IsNullOrEmpty(model.Name) ||
                string.IsNullOrEmpty(model.Description))
            {
                return RedirectToAction("WrongData", "Crushes");
            }
            bool isEdited = await equipmentService.SaveItemToEditAsync(model, GetUserId()!, 
                RoutineMaintenances, Consumables,AvailableRoutineMaintenances, AvailableConsumables);
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
            var model = await equipmentService.GetDetailsAsync(id);
            if (model == null || string.IsNullOrWhiteSpace(model.EquipmentId))
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
            var model = await equipmentService.GetItemToDeleteAsync(id);
            if (model == null || string.IsNullOrWhiteSpace(model.EquipmentId))
            {
                return RedirectToAction("NotFound", "Crushes");
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(EquipmentDeleteViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            if (model == null || model.EquipmentId == null)
            {
                return RedirectToAction("WrongData", "Crushes");
            }

            bool isDeleted = await equipmentService.ConfirmDeleteAsync(model);
            if (!isDeleted)
            {
                return RedirectToAction("NotDeleted", "Crushes");    
            }
            return RedirectToAction(nameof(Select)); 
        }
    }
}