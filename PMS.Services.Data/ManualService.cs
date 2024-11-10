using Microsoft.EntityFrameworkCore;
using PMS.Data.Models;
using PMS.Data.Repository.Interfaces;
using PMS.Services.Data.Interfaces;
using PMSWeb.ViewModels.CommonVM;
using PMSWeb.ViewModels.Manual;
using static PMS.Common.EntityValidationConstants;

namespace PMS.Services.Data
{
    public class ManualService(IRepository<Manual, Guid> manualsRepo,
                                IRepository<Maker, Guid> makersRepo,
                                IRepository<Equipment, Guid> equipmentsRepo) 
                             :IManualService
    {
        public async Task<bool> ConfirmDeleteAsync(ManualDeleteViewModel model)
        {
            if (model == null || model.ManualId == null)
            { 
                return false;
            }
                var deleteModel = await manualsRepo
                    .GetAllAsQueryable()
                    .Where(x => !x.IsDeleted)
                    .Where(x => x.ManualId.ToString().ToLower() == model.ManualId.ToLower())
                    .FirstOrDefaultAsync();
                if (deleteModel == null)
                {
                    return false;
                }
                deleteModel.IsDeleted = true;
                await manualsRepo.UpdateAsync(deleteModel);
            
            return true;
            
        }

        public async Task<bool> CreateManualAsync(ManualCreateViewModel model, string userId)
        {
            Manual manual = new Manual()
            {
                ManualName = model.ManualName,
                MakerId = Guid.Parse(model.MakerId),
                EquipmentId = Guid.Parse(model.EquipmentId),
                CreatorId = userId!,
                CreatedOn = DateTime.Now,
                EditedOn = DateTime.Now,
                IsDeleted = false,
                ContentURL = model.ContentURL
            };
            await manualsRepo.AddAsync(manual);
            return true;
        }

        public async Task<ManualCreateViewModel> GetCreateViewModelAsync(string URL)
        {
            var model = new ManualCreateViewModel();
            if (URL != null)
            {
                model.ContentURL = URL;
            }
            model.Makers = await makersRepo
                .GetAllAsQueryable()
                .Where(x => !x.IsDeleted)
                .AsNoTracking()
                .Select(x => new PairViewModel()
                {
                    Name = x.MakerName,
                    Id = x.MakerId.ToString()
                })
                .ToListAsync();

            model.Equipments = await equipmentsRepo
                .GetAllAsQueryable()
                .Where(x => !x.IsDeleted)
                .AsNoTracking()
                .Select(x => new PairViewModel()
                {
                    Name = x.Name,
                    Id = x.EquipmentId.ToString()
                })
                .ToListAsync();
            
            return model;
        }

        public async Task<ManualDetailsViewModel> GetDetailsAsync(string id)
        {
            var model = await manualsRepo
                .GetAllAsQueryable()
                .AsNoTracking()
                .Where(x => !x.IsDeleted)
                .Where(x => x.ManualId.ToString().ToLower() == id.ToLower())
                .Select(x => new ManualDetailsViewModel()
                {
                    URL = x.ContentURL,
                    Name = x.ManualName,
                    MakerName = x.Maker.MakerName,
                    EquipmentName = x.Equipment!.Name ?? string.Empty,
                })
                .FirstOrDefaultAsync();
            if (model == null)
            {
                return new ManualDetailsViewModel();
            }
            return model;
        }

        public async Task<ManualDeleteViewModel> GetItemToDeleteAsync(string id)
        {
            var model = await manualsRepo
               .GetAllAsQueryable()
               .Where(x => !x.IsDeleted)
               .Where(x => x.ManualId.ToString().ToLower() == id.ToLower())
               .AsNoTracking()
               .Select(x => new ManualDeleteViewModel()
               {
                   ManualName = x.ManualName,
                   MakerName = x.Maker.MakerName,
                   EquipmentName = x.Equipment.Name,
                   CreatedOn = x.CreatedOn.ToString(PMSRequiredDateFormat),
                   ManualId = x.ManualId.ToString()
               })
               .FirstOrDefaultAsync();
            if (model == null)
            {
                return new ManualDeleteViewModel();
            }
            return model;
        }

        public async Task<IEnumerable<ManualDisplayViewModel>> GetListOfViewModelsAsync()
        {
            var manuals = await manualsRepo
                .GetAllAsQueryable()    
                .Where(x => !x.IsDeleted)
                .OrderByDescending(x=>x.EditedOn)
                .Include(x => x.Maker)
                .Include(x => x.Equipment)
                .AsNoTracking()
                .Select(x => new ManualDisplayViewModel()
                {
                    ManualId = x.ManualId.ToString(),
                    ManualName = x.ManualName,
                    Maker = x.Maker.MakerName,
                    Equipment = x.Equipment!.Name ?? string.Empty
                })
                .ToListAsync();

            if (!manuals.Any())
            {
                return new List<ManualDisplayViewModel>();
            }
            return manuals;
        }
    }
}
