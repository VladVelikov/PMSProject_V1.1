using Microsoft.EntityFrameworkCore;
using PMS.Data.Models;
using PMS.Data.Repository.Interfaces;
using PMS.Services.Data.Interfaces;
using PMSWeb.ViewModels.CommonVM;
using PMSWeb.ViewModels.SM;
using static PMS.Common.EntityValidationConstants;

namespace PMS.Services.Data
{
    public class SMService(IRepository<SpecificMaintenance, Guid> specMaintRepo,
                                     IRepository<Equipment, Guid> equipmentRepo) : ISMService
    {
        public async Task<IEnumerable<SMDisplayViewModel>> GetListOfViewModelsAsync()
        {
            var smss = await specMaintRepo
               .GetAllAsQueryable()
               .Where(x => !x.IsDeleted)
               .Include(x => x.Equipment)
               .OrderByDescending(x => x.EditedOn)
               .AsNoTracking()
               .Select(x => new SMDisplayViewModel()
               {
                   SpecMaintId = x.SpecMaintId.ToString(),
                   Name = x.Name,
                   Description = x.Description,
                   Equipment = x.Equipment.Name,
                   LastCompletedDate = x.LastCompletedDate.ToString(PMSRequiredDateFormat),
                   Interval = x.Interval.ToString(),
                   ResponsiblePosition = x.ResponsiblePosition.ToString()
               })
               .ToListAsync();
            return smss;
        }

        public async Task<SMCreateViewModel> GetItemForCreateAsync()
        {
            var model = new SMCreateViewModel();
            var equipments = await equipmentRepo
                .GetAllAsQueryable()
                .Where(x => !x.IsDeleted)
                .AsNoTracking()
                .Select(x => new PairViewModel()
                {
                    Name = x.Name,
                    Id = x.EquipmentId.ToString()
                })
                .ToListAsync();
            model.Equipments = equipments;

            return model;
        }

        public async Task<bool> CreateSpecificMaintenanceAsync(SMCreateViewModel model, string userId)
        {
            SpecificMaintenance sm = new()
            {
                Name = model.Name,
                Description = model.Description,
                EquipmentId = Guid.Parse(model.EquipmentId),
                LastCompletedDate = DateTime.Now,
                Interval = model.Interval,
                ResponsiblePosition = model.ResponsiblePosition,
                CReatorId = userId!,
                CreatedOn = DateTime.Now,
                EditedOn = DateTime.Now,
                IsDeleted = false
            };
            try
            {
                await specMaintRepo.AddAsync(sm);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public async Task<SMEditViewModel> GetItemForEditAsync(string id)
        {
            var model = await specMaintRepo
               .GetAllAsQueryable()
               .Where(x => !x.IsDeleted)
               .Where(x => x.SpecMaintId.ToString().ToLower() == id.ToLower())
               .AsNoTracking()
               .Select(x => new SMEditViewModel()
               {
                   Name = x.Name,
                   Description = x.Description,
                   Interval = x.Interval,
                   ResponsiblePosition = x.ResponsiblePosition,
                   SMId = x.SpecMaintId.ToString(),
               })
               .FirstOrDefaultAsync();
            if (model == null)
            {
                return new SMEditViewModel();
            }
            return model;
        }

        public async Task<bool> SaveItemToEditAsync(SMEditViewModel model, string userId)
        {
            var sm = await specMaintRepo
                .GetAllAsQueryable()
                .Where(x => !x.IsDeleted)
                .Where(x => x.SpecMaintId.ToString().ToLower() == model.SMId.ToLower())
                .FirstOrDefaultAsync();
            if (sm == null)
            {
                // Don't edit the record
                return false;
            }
                // Edit the RM record
                sm.Name = model.Name;
                sm.Description = model.Description;
                sm.ResponsiblePosition = model.ResponsiblePosition;
                sm.Interval = model.Interval;
                sm.EditedOn = DateTime.Now;
            try
            {
                await specMaintRepo.UpdateAsync(sm);
                                }
            catch
            {
                return false;
            }
            return true;
        }

        public async Task<SMDeleteViewModel> GetItemToDeleteAsync(string id)
        {
            var model = await specMaintRepo
               .GetAllAsQueryable()
               .Where(x => !x.IsDeleted)
               .Where(x => x.SpecMaintId.ToString().ToLower() == id.ToLower())
               .AsNoTracking()
               .Select(x => new SMDeleteViewModel()
               {
                   Name = x.Name,
                   Description = x.Description,
                   CreatedOn = x.CreatedOn.ToString(PMSRequiredDateFormat),
                   SmId = x.SpecMaintId.ToString()
               })
               .FirstOrDefaultAsync();
            if (model == null)
            {
                return new SMDeleteViewModel();
            }
            return model;
        }

        public async Task<bool> ConfirmDeleteAsync(SMDeleteViewModel model)
        {
            if (model == null || model.SmId == null)
            {
                return false;
            }
            
            var delModel = await specMaintRepo
                .GetAllAsQueryable()
                .Where(x => !x.IsDeleted)
                .Where(x => x.SpecMaintId.ToString().ToLower() == model.SmId.ToLower())
                .FirstOrDefaultAsync();
            if (delModel == null)
            {
                return false;
            }
            try
            {
                // Execute soft delete
                delModel.IsDeleted = true;
                await specMaintRepo.UpdateAsync(delModel);
            }
            catch 
            {
                return false;
            }
                
            return true;
        }

        public async Task<SMDetailsViewModel> GetDetailsAsync(string id)
        {
            var model = await specMaintRepo
                .GetAllAsQueryable()
                .Where(x => !x.IsDeleted)
                .Where(x => x.SpecMaintId.ToString().ToLower() == id.ToLower())
                .AsNoTracking()
                .Select(x => new SMDetailsViewModel()
                {
                    SpecMaintId = x.SpecMaintId.ToString(),
                    Name = x.Name,
                    Description = x.Description,
                    Equipment = x.Equipment.Name,
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
                return new SMDetailsViewModel();
            }
            return model;
        }
    }
}
