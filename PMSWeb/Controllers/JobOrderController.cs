using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PMS.Data;
using PMS.Data.Models;
using PMSWeb.ViewModels.CommonVM;
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

        public string? GetUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier)!.ToString();
        }
    }
}
