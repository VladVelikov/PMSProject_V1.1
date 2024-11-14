using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PMS.Data;
using PMS.Data.Models;
using PMSWeb.ViewModels.CommonVM;
using PMSWeb.ViewModels.InventoryVM;
using PMSWeb.ViewModels.JobOrderVM;
using System.Security.Claims;
using static PMS.Common.EntityValidationConstants;

namespace PMSWeb.Controllers
{
    public class JobOrderController(PMSDbContext context) : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Select()
        {
            var modelList = await context
                .JobOrders
                .Where(x => !x.IsDeleted)
                .Where(x => !x.IsHistory)
                .AsNoTracking()
                .Select(x => new JobOrderDisplayViewModel()
                {
                    JobId = x.JobId.ToString(),
                    JobName = x.JobName,
                    DueDate = x.DueDate.ToString(PMSRequiredDateFormat),
                    LastDoneDate = x.LastDoneDate.ToString(PMSRequiredDateTimeFormat),
                    Type = x.Type,  
                    ResponsiblePosition = x.ResponsiblePosition
                })
                .ToListAsync();

            return View(modelList);
        }

        [HttpGet]
        public async Task<IActionResult> Create(JobOrderAddMaintenanceViewModel inputModel)
        {
            var model = new JobOrderCreateViewModel();
            if (inputModel.TypeId == "Routine")
            {
                model = await context
                    .RoutineMaintenances
                    .Where(x => !x.IsDeleted)
                    .Where(x=>x.RoutMaintId == inputModel.MaintenanceId)
                    .Select(x=> new JobOrderCreateViewModel() {
                       LastDoneDate = x.LastCompletedDate,
                       Interval = x.Interval,
                       Type = inputModel.TypeId,
                       ResponsiblePosition = x.ResponsiblePosition,
                       EquipmentId = inputModel.EquipmentId,
                       RoutineMaintenanceId = inputModel.MaintenanceId,
                       SpecificMaintenanceId = inputModel.MaintenanceId,
                       MaintenanceName = x.Name,
                       EquipmentName = inputModel.EquipmentName,
                       MaintenanceType = inputModel.TypeId,
                       JobDescription = x.Description ?? string.Empty
                    })
                    .FirstOrDefaultAsync();
            }
            else 
            {
                model = await context
                    .SpecificMaintenances
                    .Where(x => !x.IsDeleted)
                    .Where(x => x.SpecMaintId == inputModel.MaintenanceId)
                    .Select(x => new JobOrderCreateViewModel()
                    {
                        LastDoneDate = x.LastCompletedDate,
                        Interval = x.Interval,
                        Type = inputModel.TypeId,
                        ResponsiblePosition = x.ResponsiblePosition,
                        EquipmentId = inputModel.EquipmentId,
                        RoutineMaintenanceId = inputModel.MaintenanceId,
                        SpecificMaintenanceId = inputModel.MaintenanceId,
                        MaintenanceName = x.Name,
                        EquipmentName = inputModel.EquipmentName,
                        MaintenanceType = inputModel.TypeId,
                        JobDescription = x.Description ?? string.Empty
                    })
                    .FirstOrDefaultAsync();
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(JobOrderCreateViewModel model)
        {
            JobOrder jobOrder = new JobOrder() 
            {
                JobName = model.JobName,
                JobDescription = model.JobDescription,
                DueDate = model.DueDate,
                LastDoneDate = model.LastDoneDate,
                Interval = model.Interval,
                Type = model.Type,
                ResponsiblePosition= model.ResponsiblePosition,
                CreatorId = GetUserId(),
                EquipmentId= model.EquipmentId,
                MaintenanceId = model.SpecificMaintenanceId
            };
            await context.JobOrders.AddAsync(jobOrder);
            await context.SaveChangesAsync();

            return RedirectToAction(nameof(Select));
        }

        [HttpGet]
        public async Task<IActionResult> AddMaintenanceRM()
        {
            var stringId = TempData["EquipmentId"].ToString();
            var maintenanceType = TempData["MaintenanceType"].ToString();
            Guid equipmentId = Guid.Parse(stringId);

            var model = await context
                .Equipments
                .Where(x => !x.IsDeleted)
                .Where(x => x.EquipmentId == equipmentId)
                .AsNoTracking()
                .Select(x => new JobOrderAddMaintenanceViewModel()
                {
                    EquipmentId = equipmentId,
                    EquipmentName = x.Name,
                    TypeId = maintenanceType
                })
                .FirstOrDefaultAsync();

            var routineMaintenances = await context
                .RoutineMaintenancesEquipments
                .Where(x => x.EquipmentId == equipmentId)
                .AsNoTracking()
                .Select(x => new PairGuidViewModel()
                {
                    Name = x.RoutineMaintenance.Name,
                    Id = x.RoutineMaintenanceId
                })
                .ToListAsync();
            model.Maintenances = routineMaintenances;
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> AddMaintenanceSM()
        {
            var stringId = TempData["EquipmentId"].ToString();
            var maintenanceType = TempData["MaintenanceType"].ToString();
            Guid equipmentId = Guid.Parse(stringId);

            var model = await context
                .Equipments
                .Where(x => !x.IsDeleted)
                .Where(x => x.EquipmentId == equipmentId)
                .AsNoTracking()
                .Select(x => new JobOrderAddMaintenanceViewModel()
                {
                    EquipmentId = equipmentId,
                    EquipmentName = x.Name,
                    TypeId = maintenanceType
                })
                .FirstOrDefaultAsync();

            var routineMaintenances = await context
                .RoutineMaintenancesEquipments
                .Where(x => x.EquipmentId == equipmentId)
                .AsNoTracking()
                .Select(x => new PairGuidViewModel()
                {
                    Name = x.RoutineMaintenance.Name,
                    Id = x.RoutineMaintenanceId
                })
                .ToListAsync();

            var specificManintenances = await context
                .SpecificMaintenances
                .Where(x => x.EquipmentId == equipmentId)
                .AsNoTracking()
                .Select(x => new PairGuidViewModel()
                {
                    Name = x.Name,
                    Id = x.SpecMaintId
                })
                .ToListAsync();
            model.Maintenances = specificManintenances;
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> AddEquipment()
        {
            var model = new JobOrderAddEquipmentViewModel();
            
            var equipments = await context
                .Equipments
                .Where(x=>!x.IsDeleted)
                .AsNoTracking()
                .Select(x=> new PairGuidViewModel() { 
                    Name = x.Name,
                    Id = x.EquipmentId
                })
                .ToListAsync();
            model.EquipmentList = equipments;
            return View(model);
        }

        [HttpPost]
        public IActionResult AddEquipment(JobOrderAddEquipmentViewModel model)
        {
            TempData["EquipmentId"] = model.EquipmentId;
            TempData["MaintenanceType"] = model.TypeId;
            if (model.TypeId == "Routine")
            { 
                return RedirectToAction(nameof(AddMaintenanceRM));
            }
            return RedirectToAction(nameof(AddMaintenanceSM));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            var jobToDelete = await context
                .JobOrders
                .FindAsync(Guid.Parse(id));
            if (jobToDelete != null) 
                jobToDelete.IsDeleted = true;
            await context.SaveChangesAsync();

            return RedirectToAction(nameof(Select));
        }

        [HttpGet]
        public async Task<IActionResult> CompleteJob(string id)
        {
            var model = await context
                .JobOrders
                .Where(x => !x.IsDeleted && !x.IsHistory)
                .Where(x => x.JobId.ToString().ToLower() == id.ToLower())
                .AsNoTracking()
                .Select(x=> new CompleteTheJobViewModel() { 
                    JobId = x.JobId.ToString(),
                    JobName = x.JobName,
                    Details = string.Empty,
                    DueDate = x.DueDate.ToString(PMSRequiredDateFormat),
                    ResponsiblePosition = x.ResponsiblePosition,
                    Equipment = x.Equipment.Name
                })
                .FirstOrDefaultAsync();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CloseJob(CompleteTheJobViewModel model)
        {
            return RedirectToAction(nameof(Select));
        }

        [HttpGet]
        public async Task<IActionResult> SparesUsedPartial(string id)
        {
            var job = context.JobOrders.Find(Guid.Parse(id));

            var sparesList = await context
                .Spareparts
                .Where(x => !x.IsDeleted)
                .Where(x => x.EquipmentId == job.EquipmentId)
                .AsNoTracking()
                .Select(x=> new InventoryItemViewModel() {
                    Name = x.SparepartName,
                    Id = x.SparepartId.ToString(),
                    Available = x.ROB,
                    Units = x.Units,    
                    Used = 0
                })
                .ToListAsync();
            var model = new PartialViewModel()
            {
                JobId = job.JobId.ToString(),
                EquipmentId = job.EquipmentId.ToString(),
                InventoryList = sparesList  
            };

            var spares = new List<InventoryItemViewModel>(); 
            return PartialView("_SparesUsedPartial", model);
        }

        [HttpGet]
        public async Task<IActionResult> ConsumablesUsedPartial(string id)
        {
            var job = context.JobOrders.Find(Guid.Parse(id));

            var consumables = await context
                .ConsumablesEquipments
                .Where(x => x.EquipmentId == job.EquipmentId)
                .AsNoTracking()
                .Select(x=> new InventoryItemViewModel() {
                    Name = x.Consumable.Name,
                    Id = x.ConsumableId.ToString(),
                    Available = x.Consumable.ROB,
                    Units = x.Consumable.Units,
                    Used = 0
                })
                .ToListAsync();
            
            var model = new PartialViewModel() {
                JobId = job.JobId.ToString(),
                EquipmentId = job.EquipmentId.ToString(),
                InventoryList = consumables
            };
              
            return PartialView("_ConsumablesUsedPartial", model);
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmSparesUsed(PartialViewModel model)
        {
            var mySpares = await context
                .Spareparts
                .Where(x => !x.IsDeleted)
                .Where(x => x.EquipmentId.ToString().ToLower() == model.EquipmentId.ToLower())
                .ToListAsync();

            foreach (var item in model.InventoryList)
            {
                var spare = mySpares.FirstOrDefault(x => x.SparepartId.ToString().ToLower() == item.Id.ToLower());
                if (item.Used < 0)
                {
                    //do nothing
                    //spare.ROB -= item.Used;  // for testing only 
                }
                else if (item.Used > spare.ROB)
                {
                    spare.ROB = 0;
                }
                else 
                {
                    spare.ROB -= item.Used; 
                }
            }    
            await context.SaveChangesAsync();
            return RedirectToAction("CompleteJob", new { id = model.JobId });
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmConsumablesUsed(PartialViewModel model)
        {
            var myConsumables = await context
                .ConsumablesEquipments
                .Where(x => x.EquipmentId.ToString().ToLower() == model.EquipmentId.ToLower())
                .Select(x=>x.Consumable)
                .ToListAsync();

            foreach (var item in model.InventoryList)
            {
                var spare = myConsumables.FirstOrDefault(x => x.ConsumableId.ToString().ToLower() == item.Id.ToLower());
                if (item.Used < 0)
                {
                    //do nothing
                    //spare.ROB -= item.Used;  // for testing only 
                }
                else if (item.Used > spare.ROB)
                {
                    spare.ROB = 0;
                }
                else
                {
                    spare.ROB -= item.Used;
                }
            }
            await context.SaveChangesAsync();
            return RedirectToAction("CompleteJob", new { id = model.JobId });
        }



        public string? GetUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier)!.ToString();
        }
    }
}
