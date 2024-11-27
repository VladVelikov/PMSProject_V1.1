using Microsoft.EntityFrameworkCore;
using PMS.Data.Models;
using PMS.Data.Repository.Interfaces;
using PMS.Services.Data.Interfaces;
using PMSWeb.ViewModels.CommonVM;
using PMSWeb.ViewModels.InventoryVM;
using PMSWeb.ViewModels.JobOrderVM;
using System.Security.Claims;
using static PMS.Common.EntityValidationConstants;

namespace PMS.Services.Data
{
    public class JoborderService(IRepository<JobOrder, Guid> jobOrdersRepo,
                                 IRepository<RoutineMaintenance, Guid> routineMaintRepo,
                                 IRepository<SpecificMaintenance, Guid> specMaintRepo,
                                 IRepository<Equipment, Guid> equipmentRepo,
                                 IRepository<RoutineMaintenanceEquipment, Guid[]> routMaintEqRepo,
                                 IRepository<Sparepart, Guid> sparesRepo, 
                                 IRepository<ConsumableEquipment, Guid[]> consumEquipRepo) : IJoborderService
    {
        public async Task<List<JobOrderDisplayViewModel>> GetListOfAllJobsAsync()
        {
            var modelList = await jobOrdersRepo
                .GetAllAsQueryable()
                .Where(x => !x.IsDeleted)
                .Where(x => !x.IsHistory)
                .AsNoTracking()
                .Select(x => new JobOrderDisplayViewModel()
                {
                    JobId = x.JobId.ToString(),
                    JobName = x.JobName,
                    EquipmentName = x.Equipment.Name,
                    DueDate = x.DueDate.ToString(PMSRequiredDateFormat),
                    LastDoneDate = x.LastDoneDate.ToString(PMSRequiredDateFormat),
                    Type = x.Type,
                    ResponsiblePosition = x.ResponsiblePosition
                })
                .ToListAsync();
            return modelList;
        }

        public async Task<List<JobOrderDisplayViewModel>> GetListOfDueJobsAsync()
        {
            var dueJobsList = await jobOrdersRepo
                .GetAllAsQueryable()
                .Where(x => !x.IsDeleted)
                .Where(x => !x.IsHistory)
                .Where(x => x.DueDate < DateTime.UtcNow)
                .AsNoTracking()
                .Select(x => new JobOrderDisplayViewModel()
                {
                    JobId = x.JobId.ToString(),
                    JobName = x.JobName,
                    EquipmentName = x.Equipment.Name,
                    DueDate = x.DueDate.ToString(PMSRequiredDateFormat),
                    LastDoneDate = x.LastDoneDate.ToString(PMSRequiredDateFormat),
                    Type = x.Type,
                    ResponsiblePosition = x.ResponsiblePosition
                })
                .ToListAsync();
            return dueJobsList;
        }

        public async Task<List<JobOrderHistoryViewModel>> GetListOfHistoryJobsAsync()
        {
            var historyJobsList = await jobOrdersRepo
               .GetAllAsQueryable()
               .Where(x => !x.IsDeleted)
               .Where(x => x.IsHistory)
               .AsNoTracking()
               .OrderByDescending(x => x.LastDoneDate)
               .ThenBy(x => x.JobName)
               .Select(x => new JobOrderHistoryViewModel()
               {
                   JobId = x.JobId.ToString(),
                   JobName = x.JobName,
                   CompletedBy = x.CompletedBy ?? "Unknown :)",
                   LastDoneDate = x.LastDoneDate.ToString(PMSRequiredDateTimeFormat),
                   Type = x.Type,
                   ResponsiblePosition = x.ResponsiblePosition
               })
               .ToListAsync();
            return historyJobsList;
        }

        public async Task<JobHistoryDetailsViewModel> GetHistoryDetailsAsync(string id)
        {
            var model = await jobOrdersRepo
               .GetAllAsQueryable()
               .Where(x => !x.IsDeleted)
               .Where(x => x.IsHistory)
               .Where(x => x.JobId.ToString().ToLower() == id.ToLower())
               .Include(x => x.Equipment)
               .AsNoTracking()
               .Select(x => new JobHistoryDetailsViewModel()
               {
                   JobId = x.JobId.ToString(),
                   JobName = x.JobName,
                   CompletedBy = x.CompletedBy ?? string.Empty,
                   LastDoneDate = x.LastDoneDate.ToString(PMSRequiredDateFormat),
                   Type = x.Type,
                   ResponsiblePosition = x.ResponsiblePosition,
                   MaintainedEquipment = x.Equipment.Name,
                   Desription = x.JobDescription
               })
               .FirstOrDefaultAsync();
            
            return model;
        }

        public async Task<JobOrderCreateViewModel> GetCreateJobModelAsync(JobOrderAddMaintenanceViewModel inputModel)
        {
            var model = new JobOrderCreateViewModel();
            if (inputModel.TypeId == "Routine")
            {
                model = await routineMaintRepo
                    .GetAllAsQueryable()
                    .Where(x => !x.IsDeleted)
                    .Where(x => x.RoutMaintId == inputModel.MaintenanceId)
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
            else
            {
                model = await specMaintRepo
                    .GetAllAsQueryable()
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

            return model;
        }

        public async Task<bool> CreateJobOrderAsync(JobOrderCreateViewModel model, string userId)
        {
            JobOrder jobOrder = new JobOrder()
            {
                JobName = model.JobName,
                JobDescription = model.JobDescription,
                DueDate = model.DueDate,
                LastDoneDate = model.LastDoneDate,
                Interval = model.Interval,
                Type = model.Type,
                ResponsiblePosition = model.ResponsiblePosition,
                CreatorId = userId,
                EquipmentId = model.EquipmentId,
                MaintenanceId = model.SpecificMaintenanceId
            };
            await jobOrdersRepo.AddAsync(jobOrder);
            return true;
        }
        
        public async Task<JobOrderAddMaintenanceViewModel> GetAddRoutineMaintenanceViewModelAsync(Guid equipmentId, string maintenanceType)
        {
            var model = await equipmentRepo
               .GetAllAsQueryable()
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

            var routineMaintenances = await routMaintEqRepo
                .GetAllAsQueryable()
                .Where(x => x.EquipmentId == equipmentId)
                .AsNoTracking()
                .Select(x => new PairGuidViewModel()
                {
                    Name = x.RoutineMaintenance.Name,
                    Id = x.RoutineMaintenanceId
                })
                .ToListAsync();
            model.Maintenances = routineMaintenances;
            return model;
        }

        public async Task<JobOrderAddMaintenanceViewModel> GetAddSpecificMaintenanceViewModelAsync(Guid equipmentId, string maintenanceType)
        {
            var model = await equipmentRepo
                .GetAllAsQueryable()
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

            var specificManintenances = await specMaintRepo
                .GetAllAsQueryable()
                .Where(x => x.EquipmentId == equipmentId)
                .AsNoTracking()
                .Select(x => new PairGuidViewModel()
                {
                    Name = x.Name,
                    Id = x.SpecMaintId
                })
                .ToListAsync();
            model.Maintenances = specificManintenances;
            return model;

        }

        public async Task<JobOrderAddEquipmentViewModel> GetAddEquipmentModelAsync()
        {
            var model = new JobOrderAddEquipmentViewModel();

            var equipments = await equipmentRepo
                .GetAllAsQueryable()
                .Where(x => !x.IsDeleted)
                .AsNoTracking()
                .Select(x => new PairGuidViewModel()
                {
                    Name = x.Name,
                    Id = x.EquipmentId
                })
                .ToListAsync();
            model.EquipmentList = equipments;
            return model;
        }

        public async Task<bool> DeleteJobOrderAsync(string id)
        {
            var jobToDelete = await jobOrdersRepo
                .GetByIdAsync(Guid.Parse(id));
            if (jobToDelete == null)
            {
                return false;
            }
            else
            {
                jobToDelete.IsDeleted = true;
                await jobOrdersRepo.UpdateAsync(jobToDelete);
                return true;
            }
        }

        public async Task<CompleteTheJobViewModel> GetCompleteJobModelAsync(string id)
        {
            var model = await jobOrdersRepo
                .GetAllAsQueryable()
                .Where(x => !x.IsDeleted && !x.IsHistory)
                .Where(x => x.JobId.ToString().ToLower() == id.ToLower())
                .AsNoTracking()
                .Select(x => new CompleteTheJobViewModel()
                {
                    JobId = x.JobId.ToString(),
                    JobName = x.JobName,
                    Description = x.JobDescription,
                    Details = string.Empty,
                    DueDate = x.DueDate.ToString(PMSRequiredDateFormat),
                    ResponsiblePosition = x.ResponsiblePosition,
                    Equipment = x.Equipment.Name,
                    EquipmentId = x.EquipmentId.ToString()
                })
                .FirstOrDefaultAsync();

            if (model == null)
            {
                return new CompleteTheJobViewModel();
            }
            return model;
        }

        public async Task<bool> CloseThisJob(CompleteTheJobViewModel model, string userName)
        {
            var jobToClose = await jobOrdersRepo
                .GetAllAsQueryable()
                .Where(x => !x.IsDeleted)
                .Where(x => x.JobId.ToString().ToLower() == model.JobId.ToLower())
                .FirstOrDefaultAsync();
            if (jobToClose == null) 
            { 
                return false; 
            }

            string defaultDescription = jobToClose.JobDescription;
            jobToClose.JobDescription = model.Details;
            jobToClose.LastDoneDate = DateTime.UtcNow;
            jobToClose.CompletedBy = userName;
            jobToClose.IsHistory = true;

            await jobOrdersRepo.UpdateAsync(jobToClose);
            //await context.SaveChangesAsync();

            var newJob = new JobOrder()
            {
                JobId = Guid.NewGuid(),
                JobName = jobToClose.JobName,
                JobDescription = defaultDescription,
                DueDate = jobToClose.LastDoneDate.AddDays(jobToClose.Interval),
                LastDoneDate = DateTime.UtcNow,
                Interval = jobToClose.Interval,
                Type = jobToClose.Type,
                ResponsiblePosition = jobToClose.ResponsiblePosition,
                CreatorId = jobToClose.CreatorId,
                EquipmentId = jobToClose.EquipmentId,
                MaintenanceId = jobToClose.MaintenanceId,
                IsHistory = false,
                IsDeleted = false
            };
            await jobOrdersRepo.AddAsync(newJob);
            return true;
        }

        public async Task<PartialViewModel> GetSparesPartialModelAsync(string id)
        {
            var job = await jobOrdersRepo.GetByIdAsync(Guid.Parse(id));
            if (job == null) 
            {
                return new PartialViewModel();
            }

            var sparesList = await sparesRepo
                .GetAllAsQueryable()
                .Where(x => !x.IsDeleted)
                .Where(x => x.EquipmentId == job.EquipmentId)
                .AsNoTracking()
                .Select(x => new InventoryItemViewModel()
                {
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

            return model;
        }

        public async Task<PartialViewModel> GetConsumablesPartialModelAsync(string id)
        {
            var job = await jobOrdersRepo.GetByIdAsync(Guid.Parse(id));
            if (job == null)
            {
                return new PartialViewModel();
            }

            var consumables = await consumEquipRepo
                .GetAllAsQueryable()
                .Where(x => x.EquipmentId == job.EquipmentId)
                .AsNoTracking()
                .Select(x => new InventoryItemViewModel()
                {
                    Name = x.Consumable.Name,
                    Id = x.ConsumableId.ToString(),
                    Available = x.Consumable.ROB,
                    Units = x.Consumable.Units,
                    Used = 0
                })
                .ToListAsync();

            var model = new PartialViewModel()
            {
                JobId = job.JobId.ToString(),
                EquipmentId = job.EquipmentId.ToString(),
                InventoryList = consumables
            };

            return model;   
        }












        public Task<bool> ConfirmConsumablesAreUsedAsync(PartialViewModel model)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ConfirmSparesAreUsedAsync(PartialViewModel model)
        {
            throw new NotImplementedException();
        }

       

        public Task<OpenManualViewModel> GetOpenManualViewModelAsync(string jobid, string manualid)
        {
            throw new NotImplementedException();
        }

        public Task<SelectManualViewModel> GetSelectManualViewModelAsync(string id)
        {
            throw new NotImplementedException();
        }

        
    }
}
