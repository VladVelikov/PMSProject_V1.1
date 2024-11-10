using Microsoft.EntityFrameworkCore;
using PMS.Data.Models;
using PMS.Data.Repository.Interfaces;
using PMS.Services.Data.Interfaces;
using PMSWeb.ViewModels.Maker;
using static PMS.Common.EntityValidationConstants;

namespace PMS.Services.Data
{
    public class MakerService(IRepository<Maker, Guid> makersRepo, 
                              IRepository<Manual, Guid> manualsRepo, 
                              IRepository<Equipment,Guid> equipmentRepo) 
                            :IMakerService
    {
        public async Task<bool> CreateMakerAsync(MakerCreateViewModel model, string userId)
        {
           
            if (userId == null)
            {
                return false;
            }
            Maker maker = new()
            {
                MakerName = model.MakerName,
                Description = model.Description,
                Email = model.Email,
                Phone = model.Phone,
                CreatorId = userId,
                CreatedOn = DateTime.Now,
                EditedOn = DateTime.Now,
                IsDeleted = false
            };
            await makersRepo.AddAsync(maker);
            return true;
        }
        
        public async Task<IEnumerable<MakerDisplayViewModel>> GetListOfViewModelsAsync()
        {
            var makers = await makersRepo
               .GetAllAsQueryable()
               .Where(m => !m.IsDeleted)
               .OrderByDescending(x => x.EditedOn)
               .ThenBy(x => x.MakerName)
               .Select(x => new MakerDisplayViewModel()
               {
                   MakerId = x.MakerId.ToString(),
                   Name = x.MakerName,
                   Description = x.Description,
                   Email = x.Email,
                   Phone = x.Phone
               })
               .ToListAsync();
            return makers;
        }

        public async Task<MakerDetailsViewModel> GetDetailsAsync(string id)
        {
            var maker = await makersRepo
                 .GetAllAsQueryable()
                 .Where(m => !m.IsDeleted)
                 .Where(m => m.MakerId.ToString().ToLower() == id.ToLower())
                 .OrderByDescending(x => x.EditedOn)
                 .ThenBy(x => x.MakerName)
                 .Select(x => new MakerDetailsViewModel()
                 {
                     MakerId = x.MakerId.ToString(),
                     Name = x.MakerName,
                     Description = x.Description,
                     Email = x.Email,
                     Phone = x.Phone,
                     CreatedOn = x.CreatedOn.ToString(PMSRequiredDateFormat),
                     EditedOn = x.EditedOn.ToString(PMSRequiredDateFormat)
                 })
                 .FirstOrDefaultAsync();
            if (maker == null)
            {
                return new MakerDetailsViewModel();
            }
            maker.ManualsList = await manualsRepo
                .GetAllAsQueryable()
                .Where(m => !m.IsDeleted)
                .Where(m => m.MakerId.ToString().ToLower() == maker.MakerId!.ToLower())
                .AsNoTracking()
                .Select(m => m.ManualName)
                .ToListAsync();

            maker.EquipmentsList = await equipmentRepo
                .GetAllAsQueryable()
                .Where(e => !e.IsDeleted)
                .Where(e => e.MakerId.ToString().ToLower() == maker.MakerId!.ToLower())
                .AsNoTracking()
                .Select(e => e.Name)
                .ToListAsync();

            return maker;
        }

        public async Task<MakerEditViewModel> GetItemForEditAsync(string id)
        {
            var model = await makersRepo
                .GetAllAsQueryable()
                .Where(x => !x.IsDeleted)
                .Where(x => x.MakerId.ToString().ToLower() == id.ToLower())
                .Select(x => new MakerEditViewModel()
                {
                    MakerName = x.MakerName,
                    Description = x.Description,
                    Email = x.Email,
                    Phone = x.Phone,
                    MakerId = id
                })
                .AsNoTracking()
                .FirstOrDefaultAsync();
            if (model == null)
            {
                return new MakerEditViewModel();
            }
            return model;
        }
        
        public async Task<bool> SaveItemToEditAsync(MakerEditViewModel model, string userId)
        {
            var editModel = await makersRepo
               .GetAllAsQueryable()
               .Where(x => !x.IsDeleted)
               .Where(x => x.MakerId.ToString().ToLower() == model.MakerId.ToLower())
               .FirstOrDefaultAsync();
            if (editModel == null)
            {
                return false;
            }

            editModel.MakerName = model.MakerName;
            editModel.Description = model.Description;
            editModel.Email = model.Email;
            editModel.Phone = model.Phone;
            editModel.EditedOn = DateTime.Now;

            await makersRepo.UpdateAsync(editModel);
            return true;
        }

        public async Task<MakerDeleteViewModel> GetItemToDeleteAsync(string id)
        {
            var model = await makersRepo
                .GetAllAsQueryable()
                .Where(m => !m.IsDeleted)
                .Where(m => m.MakerId.ToString().ToLower() == id.ToLower())
                .Select(m => new MakerDeleteViewModel()
                {
                    MakerId = m.MakerId.ToString(),
                    Name = m.MakerName,
                    Email = m.Email,
                    Phone = m.Phone
                })
                .AsNoTracking()
                .FirstOrDefaultAsync();
            return model;
        }
        
        public async Task<bool> ConfirmDeleteAsync(MakerDeleteViewModel model)
        {
            if (model.MakerId == null)
            {
                return false;
            }
            var makerToDel = await makersRepo
                .GetAllAsQueryable()
                .Where(m => !m.IsDeleted)
                .Where(m => m.MakerId.ToString().ToLower() == model.MakerId.ToLower())
                .FirstOrDefaultAsync();
            if (makerToDel == null)
            {
                return false;
            }

            makerToDel.IsDeleted = true;
            await makersRepo.UpdateAsync(makerToDel);
            return true;
        }
    }
}
