﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PMS.Services.Data.Interfaces;
using PMSWeb.ViewModels.Equipment;
using System.Security.Claims;

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
            return View(eqList);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            
            var model = await equipmentService.GetCreateModelAsync(); 

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
            var model = await equipmentService.GetItemForEditAsync(id);
            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> Edit(EquipmentEditViewModel model,
            List<Guid> RoutineMaintenances,
            List<Guid> Consumables,
            List<Guid> AvailableRoutineMaintenances,
            List<Guid> AvailableConsumables)
        {
           
            bool isEdited = await equipmentService.SaveItemToEditAsync(model, GetUserId(), 
                RoutineMaintenances, Consumables,AvailableRoutineMaintenances, AvailableConsumables);


            return RedirectToAction(nameof(Select));
        }

        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            var model = await equipmentService.GetDetailsAsync(id);
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            var model = await equipmentService.GetItemToDeleteAsync(id);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(EquipmentDeleteViewModel model)
        {
            bool isDeleted = await equipmentService.ConfirmDeleteAsync(model);
            return RedirectToAction(nameof(Select)); 
        }

        //private string? GetUserId()
        //{
        //    return User.FindFirstValue(ClaimTypes.NameIdentifier);
        //}
    }

}