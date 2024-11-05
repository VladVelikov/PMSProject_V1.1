using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NuGet.Configuration;
using PMS.Data;
using PMS.Data.Models;
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
            var makers = await context
                .Makers
                .Where(x => x.IsDeleted == false)
                .AsNoTracking()
                .Select(x=> new {
                    x.MakerId,
                    x.MakerName
                })
                .ToListAsync(); 

            var routineMaintenances = await context 
                .RoutineMaintenances
                .Where(x => x.IsDeleted == false)
                .AsNoTracking()
                .Select(x=> new {
                    RoutineMaintenanceName = x.Name,
                    RoutineMaintenanceId = x.RoutMaintId
                })
                .ToListAsync();

            var consumables = await context
                .Consumables
                .Where(x=>x.IsDeleted == false)
                .AsNoTracking()
                .Select(x => new
                {
                    ConsName = x.Name,
                    ConsId = x.ConsumableId
                })
                .ToListAsync();

            ViewBag.RoutineMaintenances = new SelectList(routineMaintenances, "RoutineMaintenanceId", "RoutineMaintenanceName");
            ViewBag.Consumables = new SelectList(consumables, "ConsId", "ConsName");
            ViewBag.Makers = new SelectList(makers, "MakerId", "MakerName");
            return View(new EquipmentCreateViewModel());
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

            var makers = await context
                .Makers
                .Where(x => x.IsDeleted == false)
                .AsNoTracking()
                .Select (x => new {
                    x.MakerName,
                    x.MakerId
                })
                .ToListAsync();

            var consumables = await context
                .ConsumablesEquipments
                .Where(x => x.EquipmentId.ToString().ToLower() == id.ToLower())
                .AsNoTracking()
                .Select(x => new
                {
                    ConsumableName = x.Consumable.Name,
                    ConsumableId = x.ConsumableId
                })
                .ToListAsync();

            var routineMaintenances = await context
                .RoutineMaintenancesEquipments
                .Where(x => x.EquipmentId.ToString().ToLower() == id.ToLower())
                .AsNoTracking()
                .Select(x => new
                {
                    MaintenanceName = x.RoutineMaintenance.Name,
                    MaintenanceId = x.RoutineMaintenanceId
                })
                .ToListAsync();

            var availableConsumables = await context
                .Consumables
                .Where(x=>x.IsDeleted == false)
                .AsNoTracking()
                .Select(x => new
                {
                    ConsumableName = x.Name,
                    ConsumableId = x.ConsumableId
                })
                .ToListAsync();
            foreach (var cons in consumables)
            {
                var avcToRemove = availableConsumables.FirstOrDefault(ac => ac.ConsumableId == cons.ConsumableId);
                if (avcToRemove != null)
                {
                    availableConsumables.Remove(avcToRemove);
                }
            }

            var availableRoutineMaintenances = await context
                .RoutineMaintenances
                .Where(x=>x.IsDeleted == false)
                .AsNoTracking()
                .Select(x => new
                {
                    MaintenanceName = x.Name,
                    MaintenanceId = x.RoutMaintId
                })
                .ToListAsync();
            foreach (var rtm in routineMaintenances)
            {
                var rtmToRemove = availableRoutineMaintenances.FirstOrDefault(rm => rm.MaintenanceId == rtm.MaintenanceId);
                if (rtmToRemove != null)
                {
                    availableRoutineMaintenances.Remove(rtmToRemove);
                }

            }

            ViewBag.Makers = new SelectList(makers, "MakerId", "MakerName");
            ViewBag.Consumables = new SelectList(consumables, "ConsumableId", "ConsumableName");
            ViewBag.RoutineMaintenances = new SelectList(routineMaintenances, "MaintenanceId","MaintenanceName");
            ViewBag.AvailableRoutineMaintenances = new SelectList(availableRoutineMaintenances, "MaintenanceId", "MaintenanceName");
            ViewBag.AvailableConsumables = new SelectList(availableConsumables, "ConsumableId", "ConsumableName");

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



        private string? GetUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier);
        }
    }

}