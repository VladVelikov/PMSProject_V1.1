using Microsoft.EntityFrameworkCore;
using PMS.Data.Models;
using PMS.Data.Repository.Interfaces;
using PMS.Services.Data.Interfaces;
using PMSWeb.ViewModels.RM;
using static PMS.Common.EntityValidationConstants;

namespace PMS.Services.Data
{
    public class RMService(IRepository<RoutineMaintenance, Guid> rmsRepo, 
                           IRepository<RoutineMaintenanceEquipment, Guid[]> routineMaintenanceEquipmentsRepo) 
                         : IRMService
    {
        public async Task<IEnumerable<RMDisplayViewModel>> GetListOfViewModelsAsync()
        {
            var rmss = await rmsRepo
                .GetAllAsQueryable()
                .Where(x => !x.IsDeleted)
                .OrderByDescending(x => x.EditedOn)
                .AsNoTracking()
                .Select(x => new RMDisplayViewModel()
                {
                    RoutMaintId = x.RoutMaintId.ToString(),
                    Name = x.Name,
                    Description = x.Description,
                    LastCompletedDate = x.LastCompletedDate.ToString(PMSRequiredDateFormat),
                    Interval = x.Interval.ToString(),
                    ResponsiblePosition = x.ResponsiblePosition.ToString()
                })
                .ToListAsync();
            return rmss;
        }

        public async Task<bool> CreateRMAsync(RMCreateViewModel model, string userId)
        {
            RoutineMaintenance rm = new()
            {
                Name = model.Name,
                Description = model.Description,
                LastCompletedDate = DateTime.Now,
                Interval = model.Interval,
                ResponsiblePosition = model.ResponsiblePosition,
                CReatorId = userId,
                CreatedOn = DateTime.Now,
                EditedOn = DateTime.Now,
                IsDeleted = false
            };
            try
            {
                await rmsRepo.AddAsync(rm);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public async Task<RMDetailsViewModel> GetDetailsAsync(string id)
        {
            var model = await rmsRepo
               .GetAllAsQueryable()
               .Where(x => !x.IsDeleted)
               .Where(x => x.RoutMaintId.ToString().ToLower() == id.ToLower())
               .AsNoTracking()
               .Select(x => new RMDetailsViewModel()
               {
                   RoutMaintId = x.RoutMaintId.ToString(),
                   Name = x.Name,
                   Description = x.Description,
                   LastCompletedDate = x.LastCompletedDate.ToString(PMSRequiredDateFormat),
                   Interval = x.Interval.ToString(),
                   ResponsiblePosition = x.ResponsiblePosition,
                   CreatorName = x.Creator.UserName ?? string.Empty,
                   CreatedOn = x.CreatedOn.ToString(PMSRequiredDateFormat),
                   EditedOn = x.EditedOn.ToString(PMSRequiredDateFormat),
               })
               .FirstOrDefaultAsync();

            if (model == null)
            {
                return new RMDetailsViewModel();
            }

            var equipments = await routineMaintenanceEquipmentsRepo
                .GetAllAsQueryable()
                .Where(x => x.RoutineMaintenanceId.ToString().ToLower() == id.ToLower())
                .AsNoTracking()
                .Select(x => x.Equipment.Name)
                .ToListAsync();
            if (equipments.Any())
            {
                model.Equipments = equipments;
            }
            return model;
        }

        public async Task<RMEditViewModel> GetItemForEditAsync(string id)
        {
            var model = await rmsRepo
                .GetAllAsQueryable()
                .Where(x => !x.IsDeleted)
                .Where(x => x.RoutMaintId.ToString().ToLower() == id.ToLower())
                .AsNoTracking()
                .Select(x => new RMEditViewModel()
                {
                    Name = x.Name,
                    Description = x.Description,
                    Interval = x.Interval,
                    ResponsiblePosition = x.ResponsiblePosition,
                    RMId = x.RoutMaintId.ToString(),
                })
                .FirstOrDefaultAsync();
            if (model == null)
            {
                return new RMEditViewModel();
            }
            return model;
        }

        public async Task<bool> SaveItemToEditAsync(RMEditViewModel model, string userId)
        {
            var rm = await rmsRepo
               .GetAllAsQueryable()
               .Where(x => !x.IsDeleted)
               .Where(x => x.RoutMaintId.ToString().ToLower() == model.RMId.ToLower())
               .FirstOrDefaultAsync();
            if (rm == null)
            {
                // Don't edit the record
                return false;
            }
            // Edit the RM record
            rm.Name = model.Name;
            rm.Description = model.Description;
            rm.ResponsiblePosition = model.ResponsiblePosition;
            rm.Interval = model.Interval;
            rm.EditedOn = DateTime.UtcNow;
            try
            {
                await rmsRepo.UpdateAsync(rm);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public async Task<RMDeleteViewModel> GetItemToDeleteAsync(string id)
        {
            var model = await rmsRepo
                .GetAllAsQueryable()
                .Where(x => !x.IsDeleted)
                .Where(x => x.RoutMaintId.ToString().ToLower() == id.ToLower())
                .AsNoTracking()
                .Select(x => new RMDeleteViewModel()
                {
                    Name = x.Name,
                    Description = x.Description,
                    CreatedOn = x.CreatedOn.ToString(PMSRequiredDateFormat),
                    RmId = x.RoutMaintId.ToString()
                })
                .FirstOrDefaultAsync();
            if (model == null)
            {
                return new RMDeleteViewModel();
            }
            return model;
        }

        public async Task<bool> ConfirmDeleteAsync(RMDeleteViewModel model)
        {
            if (model == null || model.RmId == null)
            {
                return false;
            }
            var delModel = await rmsRepo
                .GetAllAsQueryable()
                .Where(x => !x.IsDeleted)
                .Where(x => x.RoutMaintId.ToString().ToLower() == model.RmId.ToLower())
                .FirstOrDefaultAsync();
            if (delModel == null)
            {
                return false;
            }
            // Execute soft delete
            try
            {
                delModel.IsDeleted = true;
                await rmsRepo.UpdateAsync(delModel);
            }
            catch
            {
                return false;
            }
            return true;
        }
    }
}
