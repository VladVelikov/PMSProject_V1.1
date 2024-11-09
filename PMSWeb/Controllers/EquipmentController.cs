﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PMS.Data;
using PMS.Data.Models;
using PMSWeb.ViewModels.CommonVM;
using PMSWeb.ViewModels.Equipment;
using System.Security.Claims;
using static PMS.Common.EntityValidationConstants;

namespace PMSWeb.Controllers
{
    [Authorize]
    public class EquipmentController(PMSDbContext context) : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Select()
        {
            var eqList = await context
                .Equipments
                .AsNoTracking()
                .Where(x => x.IsDeleted == false)
                .OrderByDescending(x=>x.EditedOn)
                .ThenBy(x=>x.Name)
                .Select(x=> new EquipmentDisplayViewModel() {
                    EquipmentId = x.EquipmentId.ToString(),
                    Name = x.Name,
                    Description = x.Description,
                    CreatedOn = x.CreatedOn.ToString(PMSRequiredDateFormat),
                    EditedOn = x.EditedOn.ToString(PMSRequiredDateFormat),
                    Creator = x.Creator.UserName,
                    Maker = x.Maker.MakerName
                })
                .ToListAsync();
                
            return View(eqList);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var model = new EquipmentCreateViewModel();
            var makers = await context
                .Makers
                .Where(x => x.IsDeleted == false)
                .AsNoTracking()
                .Select(x=> new PairGuidViewModel{
                    Id = x.MakerId,
                    Name = x.MakerName
                })
                .ToListAsync(); 

            var routineMaintenances = await context 
                .RoutineMaintenances
                .Where(x => x.IsDeleted == false)
                .AsNoTracking()
                .Select(x=> new PairGuidViewModel(){
                    Name = x.Name,
                    Id = x.RoutMaintId
                })
                .ToListAsync();

            var consumables = await context
                .Consumables
                .Where(x=>x.IsDeleted == false)
                .AsNoTracking()
                .Select(x => new PairGuidViewModel()
                {
                    Name = x.Name,
                    Id = x.ConsumableId
                })
                .ToListAsync();

            model.RoutineMaintenances = routineMaintenances;
            model.Consumables = consumables;
            model.Makers = makers;
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(EquipmentCreateViewModel model, 
            List<Guid> RoutineMaintenances,
            List<Guid> Consumables)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // below is to create and add new equipment to the dbSet
            Equipment equipment = new()
            {
                Name = model.Name,
                Description = model.Description,
                CreatedOn = DateTime.Now,
                EditedOn = DateTime.Now,
                CreatorId = GetUserId() ?? string.Empty,
                MakerId = model.MakerId,
                IsDeleted = false
            };
            await context.Equipments.AddAsync(equipment);
            
            // Adding additional related items to the current equipment when created  ( Consumables and Routine Maintenances )
             foreach (var maintenanceId in RoutineMaintenances)
             {
               bool alreadyAdded = await context.RoutineMaintenancesEquipments
                    .AnyAsync(x => x.RoutineMaintenanceId == maintenanceId &&
                                 x.EquipmentId == equipment.EquipmentId);
                    if (!alreadyAdded)
                    {
                        RoutineMaintenanceEquipment rme = new() { 
                        RoutineMaintenanceId = maintenanceId,
                        EquipmentId = equipment.EquipmentId
                        };
                        await context.RoutineMaintenancesEquipments.AddAsync(rme);
                    }
             }

             foreach (var consumableId in Consumables)
             {
                bool alreadyAdded = await context.ConsumablesEquipments
                    .AnyAsync(x=>x.ConsumableId == consumableId &&
                                 x.EquipmentId == equipment.EquipmentId);
                    if (!alreadyAdded)
                    {
                        ConsumableEquipment consumableEquipment = new() {
                            ConsumableId = consumableId,
                            EquipmentId= equipment.EquipmentId
                        };
                        await context.ConsumablesEquipments.AddAsync(consumableEquipment); 
                    }
             }

             await context.SaveChangesAsync();
            
             return RedirectToAction(nameof(Select));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var model = await context
                .Equipments
                .Where(x => x.IsDeleted == false)
                .Where(x => x.EquipmentId.ToString().ToLower() == id.ToLower())
                .AsNoTracking()
                .Select(x => new EquipmentEditViewModel()
                {
                    EquipmentId = x.EquipmentId.ToString(),
                    Name = x.Name,
                    Description = x.Description,
                    MakerId = x.MakerId
                })
                .FirstOrDefaultAsync();
            if (model == null) { return RedirectToAction(nameof(Select)); }

            var makers = await context
                .Makers
                .Where(x => x.IsDeleted == false)
                .AsNoTracking()
                .Select (x => new PairGuidViewModel(){
                    Name = x.MakerName,
                    Id = x.MakerId
                })
                .ToListAsync();

            var consumables = await context
                .ConsumablesEquipments
                .Where(x => x.EquipmentId.ToString().ToLower() == id.ToLower())
                .AsNoTracking()
                .Select(x => new PairGuidViewModel()
                {
                    Name = x.Consumable.Name,
                    Id = x.ConsumableId
                })
                .ToListAsync();

            var routineMaintenances = await context
                .RoutineMaintenancesEquipments
                .Where(x => x.EquipmentId.ToString().ToLower() == id.ToLower())
                .AsNoTracking()
                .Select(x => new PairGuidViewModel()
                {
                    Name = x.RoutineMaintenance.Name,
                    Id = x.RoutineMaintenanceId
                })
                .ToListAsync();

            var availableConsumables = await context
                .Consumables
                .Where(x=>x.IsDeleted == false)
                .AsNoTracking()
                .Select(x => new PairGuidViewModel()
                {
                    Name = x.Name,
                    Id = x.ConsumableId
                })
                .ToListAsync();
            foreach (var cons in consumables)
            {
                var avcToRemove = availableConsumables.FirstOrDefault(ac => ac.Id == cons.Id);
                if (avcToRemove != null)
                {
                    availableConsumables.Remove(avcToRemove);
                }
            }

            var availableRoutineMaintenances = await context
                .RoutineMaintenances
                .Where(x=>x.IsDeleted == false)
                .AsNoTracking()
                .Select(x => new PairGuidViewModel()
                {
                    Name = x.Name,
                    Id = x.RoutMaintId
                })
                .ToListAsync();
            foreach (var rtm in routineMaintenances)
            {
                var rtmToRemove = availableRoutineMaintenances.FirstOrDefault(rm => rm.Id == rtm.Id);
                if (rtmToRemove != null)
                {
                    availableRoutineMaintenances.Remove(rtmToRemove);
                }

            }

            model.Makers = makers;
            model.Consumables = consumables;
            model.RoutineMaintenances = routineMaintenances;
            model.AvailableRoutineMaintenances = availableRoutineMaintenances;
            model.AvailableConsumables = availableConsumables;

            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> Edit(EquipmentEditViewModel model,
            List<Guid> RoutineMaintenances,
            List<Guid> Consumables,
            List<Guid> AvailableRoutineMaintenances,
            List<Guid> AvailableConsumables)
        {
            var eq = await context  // equipment to edit 
                .Equipments
                .Where(x => x.IsDeleted == false)
                .Where(x => x.EquipmentId.ToString().ToLower() == model.EquipmentId.ToLower())
                .FirstOrDefaultAsync();

            if (eq == null) 
            {
                return RedirectToAction(nameof(Select));
            }

            eq.Name = model.Name;   
            eq.Description = model.Description;
            eq.MakerId = model.MakerId;
            eq.EditedOn = DateTime.Now; 

            var consEquipments = await context
                .ConsumablesEquipments
                .Where(x => x.EquipmentId.ToString().ToLower() == model.EquipmentId.ToLower())
                .ToListAsync();

            foreach (var item in consEquipments)
            {
                if (!Consumables.Contains(item.ConsumableId)) 
                {
                    context.ConsumablesEquipments.Remove(item);
                }
            }

            var routMaintEquipments = await context
                .RoutineMaintenancesEquipments
                .Where(x => x.EquipmentId.ToString().ToLower() == model.EquipmentId.ToLower())
                .ToListAsync();

            foreach (var item in routMaintEquipments)
            {
                if (!RoutineMaintenances.Contains(item.RoutineMaintenanceId))
                {
                    context.RoutineMaintenancesEquipments.Remove(item);
                }
            }

            foreach (var maintenanceId in AvailableRoutineMaintenances)
            {
                bool alreadyAdded = await context.RoutineMaintenancesEquipments
                     .AnyAsync(x => x.RoutineMaintenanceId == maintenanceId &&
                                  x.EquipmentId == Guid.Parse(model.EquipmentId));
                if (!alreadyAdded)
                {
                    RoutineMaintenanceEquipment rme = new()
                    {
                        RoutineMaintenanceId = maintenanceId,
                        EquipmentId = Guid.Parse(model.EquipmentId)
                    };
                    await context.RoutineMaintenancesEquipments.AddAsync(rme);
                }
            }

            foreach (var consumableId in AvailableConsumables)
            {
                bool alreadyAdded = await context.ConsumablesEquipments
                    .AnyAsync(x => x.ConsumableId == consumableId &&
                                 x.EquipmentId == Guid.Parse(model.EquipmentId));
                if (!alreadyAdded)
                {
                    ConsumableEquipment consumableEquipment = new()
                    {
                        ConsumableId = consumableId,
                        EquipmentId = Guid.Parse(model.EquipmentId)
                };
                    await context.ConsumablesEquipments.AddAsync(consumableEquipment);
                }
            }



            await context.SaveChangesAsync();

            return RedirectToAction(nameof(Select));
        }

        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            EquipmentDetailsViewModel? model = await context
                .Equipments
                .Where(x=>!x.IsDeleted)
                .Where(x=>x.EquipmentId.ToString().ToLower() == id.ToLower())
                .AsNoTracking()
                .Select(x=> new EquipmentDetailsViewModel() {
                    EquipmentId = x.EquipmentId.ToString(),
                    Name = x.Name,
                    Description = x.Description,
                    CreatedOn = x.CreatedOn.ToString(PMSRequiredDateFormat),
                    EditedOn = x.EditedOn.ToString(PMSRequiredDateFormat),
                    Creator = x.Creator.UserName,
                    Maker = x.Maker.MakerName
                })
                .FirstOrDefaultAsync();
            if (model == null)
            {
                return RedirectToAction(nameof(Select));
            }
                
            List<string> routineMaintenances = await context
                .RoutineMaintenancesEquipments
                .Where(x=>x.EquipmentId.ToString().ToLower() == id.ToLower())
                .Select(x=>x.RoutineMaintenance.Name)
                .ToListAsync();

            List<string> specificMaintenances = await context
                .SpecificMaintenances
                .Where(x => !x.IsDeleted)
                .Where(x => x.EquipmentId.ToString().ToLower() == id.ToLower())
                .Select(x => x.Name)
                .ToListAsync();

            List<string> spareParts = await context
                .Spareparts
                .Where(x => !x.IsDeleted)
                .Where(x => x.EquipmentId.ToString().ToLower() == id.ToLower())
                .Select(x => x.SparepartName)
                .ToListAsync();

            List<string> manuals = await context
                .Manuals
                .Where(x => !x.IsDeleted)
                .Where(x => x.EquipmentId.ToString().ToLower() == id.ToLower())
                .Select(x => x.ManualName)
                .ToListAsync();

            List<string> consumables = await context
                .ConsumablesEquipments
                .Where(x => x.EquipmentId.ToString().ToLower() == id.ToLower())
                .Select(x => x.Consumable.Name)
                .ToListAsync();

            if(routineMaintenances != null)
                model.RoutineMaintenances = routineMaintenances;

            if (specificMaintenances != null)
                model.SpecificMaintenances = specificMaintenances;

            if (spareParts != null)
                model.SpareParts = spareParts;

            if (manuals != null)
                model.Manuals = manuals;

            if (consumables != null)
                model.Consumables = consumables;


            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            var model = await context
                .Equipments
                .Where(x => x.IsDeleted == false)
                .Where(x => x.EquipmentId.ToString().ToLower() == id.ToLower())
                .Select(x=> new EquipmentDeleteViewModel() {
                    EquipmentId = x.EquipmentId.ToString(),
                    Name = x.Name,  
                    Description = x.Description,
                    CreatedOn = x.CreatedOn.ToString(PMSRequiredDateFormat)
                })
                .FirstOrDefaultAsync();
            if (model == null)
            {
                return RedirectToAction(nameof(Select));
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(EquipmentDeleteViewModel model)
        {
            if (model.EquipmentId == null)
            {
                return View(model);
            }
            var deleteRecord = await context
                .Equipments
                .Where(x => x.IsDeleted == false)
                .Where(x => x.EquipmentId.ToString().ToLower() == model.EquipmentId.ToLower())
                .FirstOrDefaultAsync();
            if (deleteRecord == null) 
            {
                return View(model);
            }
            deleteRecord.IsDeleted = true;  
            await context.SaveChangesAsync();

            return RedirectToAction(nameof(Select)); 
        }

        private string? GetUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier);
        }
    }

}