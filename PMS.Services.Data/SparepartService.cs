using Microsoft.EntityFrameworkCore;
using PMS.Data.Models;
using PMS.Data.Repository.Interfaces;
using PMS.Services.Data.Interfaces;
using PMSWeb.ViewModels.CommonVM;
using PMSWeb.ViewModels.SparepartVM;
using static PMS.Common.EntityValidationConstants;


namespace PMS.Services.Data
{
    public class SparepartService(IRepository<Equipment, Guid> equipmentRepo, 
                                  IRepository<Sparepart, Guid> sparesRepo) : ISparepartService
    {
        public async Task<IEnumerable<SparepartDisplayViewModel>> GetListOfViewModelsAsync()
        {
            var models = await sparesRepo
               .GetAllAsQueryable()
               .Where(x => x.IsDeleted == false)
               .AsNoTracking()
               .OrderByDescending(x => x.EditedOn)
               .ThenBy(x => x.SparepartName)
               .Select(x => new SparepartDisplayViewModel()
               {
                   SparepartId = x.SparepartId.ToString(),
                   Name = x.SparepartName,
                   Units = x.Units,
                   Description = x.Description,
                   Equipment = x.Equipment.Name,
                   Price = x.Price.ToString("C"),
                   ROB = x.ROB.ToString(),
               })
              .ToListAsync();
            return models;
        }

        public async Task<SparepartCreateViewModel> GetItemForCreateAsync()
        {
            var model = new SparepartCreateViewModel();
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
            if (equipments != null)
            {
                model.Equipments = equipments;
            }
            return model;
        }

        public async Task<bool> CreateSparepartAsync(SparepartCreateViewModel model, string userId)
        {
            Sparepart spare = new()
            {
                SparepartName = model.Name,
                Description = model.Description ?? string.Empty,
                ROB = model.ROB,
                Price = model.Price,
                Units = model.Units,
                EquipmentId = Guid.Parse(model.EquipmentId),
                CreatedOn = DateTime.UtcNow,
                EditedOn = DateTime.UtcNow,
                ImageURL = model.ImageUrl,
                CreatorId = userId,
                IsDeleted = false
            };
            await sparesRepo.AddAsync(spare);
            return true;
        }

        public async Task<SparepartEditViewModel> GetItemForEditAsync(string id)
        {
            var editModel = await sparesRepo
                .GetAllAsQueryable()
                .Where(x => !x.IsDeleted)
                .Where(x => x.SparepartId.ToString().ToLower() == id.ToLower())
                .Select(x => new SparepartEditViewModel()
                {
                    SparepartId = x.SparepartId.ToString(),
                    Name = x.SparepartName,
                    Description = x.Description ?? string.Empty,
                    Price = x.Price,
                    Units = x.Units,
                    ROB = x.ROB,
                    ImageUrl = x.ImageURL
                })
                .AsNoTracking()
                .FirstOrDefaultAsync();
            if (editModel == null)
            {
                return new SparepartEditViewModel();
            }
            return editModel;
        }

        public async Task<bool> SaveItemToEditAsync(SparepartEditViewModel model, string userId)
        {
            if (model == null || userId == null || model.SparepartId == null)
            {
                return false;
            }
            var editModel = await sparesRepo
               .GetAllAsQueryable()
               .Where(x => !x.IsDeleted)
               .Where(x => x.SparepartId.ToString().ToLower() == model.SparepartId.ToLower())
               .FirstOrDefaultAsync();
            if (editModel == null)
            {
                return false;
            }
                editModel.SparepartName = model.Name;
                editModel.Description = model.Description ?? string.Empty;
                editModel.Price = model.Price;
                editModel.Units = model.Units;
                editModel.ROB = model.ROB;
                editModel.ImageURL = model.ImageUrl;
                editModel.EditedOn = DateTime.UtcNow;
            await sparesRepo.UpdateAsync(editModel);
            return true;
        }

        public async Task<SparepartDeleteViewModel> GetItemToDeleteAsync(string id)
        {
            var model = await sparesRepo
                .GetAllAsQueryable()
                .AsNoTracking()
                .Where(x => !x.IsDeleted)
                .Where(x => x.SparepartId.ToString().ToLower() == id.ToLower())
                .Select(x => new SparepartDeleteViewModel()
                {
                    SparepartId = x.SparepartId.ToString(),
                    Name = x.SparepartName,
                    Description = x.Description,
                    CreatedOn = x.CreatedOn.ToString(PMSRequiredDateFormat)
                })
                .FirstOrDefaultAsync();
            if (model == null)
            {
                return new SparepartDeleteViewModel();
            }
            return model;
        }

        public async Task<bool> ConfirmDeleteAsync(SparepartDeleteViewModel model)
        {
            if (model == null || model.SparepartId == null)
            {
                return false;
            }
            var deleteModel = await sparesRepo
                .GetAllAsQueryable()
                .Where(x => !x.IsDeleted)
                .Where(x => x.SparepartId.ToString().ToLower() == model.SparepartId.ToLower())
                .FirstOrDefaultAsync();
            if (deleteModel == null)
            {
                return false;
            }
            await sparesRepo.RemoveItemAsync(deleteModel);
            return true;
        }

        public async Task<SparepartDetailsViewModel> GetDetailsAsync(string id)
        {
            var model = await sparesRepo
              .GetAllAsQueryable()
              .Where(x => !x.IsDeleted)
              .Where(x => x.SparepartId.ToString().ToLower() == id.ToLower())
              .Select(x => new SparepartDetailsViewModel()
              {
                  SparepartId = x.SparepartId.ToString(),
                  Name = x.SparepartName,
                  Description = x.Description ?? string.Empty,
                  Equipment = x.Equipment.Name,
                  Price = x.Price.ToString("C"),
                  Units = x.Units,
                  ROB = x.ROB.ToString(),
                  CreatedOn = x.CreatedOn.ToString(PMSRequiredDateFormat),
                  CreatorName = x.Creator.UserName,
                  ImageURL = x.ImageURL
              })
              .AsNoTracking()
              .FirstOrDefaultAsync();
            if (model == null)
            { 
                return new SparepartDetailsViewModel();
            }
            return model;
        }
    }
}
