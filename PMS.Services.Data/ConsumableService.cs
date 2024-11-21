using Microsoft.EntityFrameworkCore;
using PMS.Data.Models;
using PMS.Data.Repository.Interfaces;
using PMS.Services.Data.Interfaces;
using PMSWeb.ViewModels.Consumable;

namespace PMS.Services.Data
{
    public class ConsumableService(IRepository<Consumable, Guid> consumables) : IConsumableService
    {
        public async Task<bool> CreateConsumableAsync(ConsumableCreateViewModel model, string userId)
        {
            Consumable consumable = new Consumable()
            {
                Name = model.Name,
                Units = model.Units,
                Description = model.Description,
                Price = model.Price,
                ROB = model.ROB,
                CreatorId = userId,
                CreatedOn = DateTime.Now,
                EditedOn = DateTime.Now,
                IsDeleted = false
            };
            try
            {
                await consumables.AddAsync(consumable);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public async Task<ConsumableDetailsViewModel> GetDetailsAsync(string id)
        {
            var model = await consumables.GetAllAsQueryable()
                .Where(x => x.IsDeleted == false)
                .Where(x => x.ConsumableId.ToString() == id)
                .AsNoTracking()
                .Select(x => new ConsumableDetailsViewModel()
                {
                    ConsumableId = x.ConsumableId.ToString(),
                    Name = x.Name,
                    Units = x.Units,
                    Description = x.Description,
                    Price = x.Price.ToString("C"),
                    ROB = x.ROB,
                    CreatedOn = x.CreatedOn,
                    EditedOn = x.EditedOn,
                    CreatorId = x.CreatorId,
                    CreatorName = x.Creator.UserName ?? string.Empty,
                })
                .FirstOrDefaultAsync();
            return model?? new ConsumableDetailsViewModel();    
        }

        public async Task<IEnumerable<ConsumableDisplayViewModel>> GetListOfViewModelsAsync()
        {
            var models = await consumables.GetAllAsQueryable()
               .Where(x => x.IsDeleted == false)
               .AsNoTracking()
               .Select(x => new ConsumableDisplayViewModel()
               {
                   ConsumableId = x.ConsumableId.ToString(),
                   Name = x.Name,
                   Units = x.Units,
                   Description = x.Description,
                   Price = x.Price,
                   ROB = x.ROB,
                   EditedOn = x.EditedOn
               })
               .OrderByDescending(x => x.EditedOn)
               .ThenBy(x => x.Name)
               .ToListAsync();
            if (models.Any())
            {
                return models;  
            }
            return new List<ConsumableDisplayViewModel>();

        }

        public async Task<ConsumableEditViewModel> GetItemForEditAsync(string id)
        {
            var model = await consumables.GetAllAsQueryable()
                .AsNoTracking()
                .Where(x => x.IsDeleted == false)
                .Where(x => x.ConsumableId.ToString() == id)
                .Select(x => new ConsumableEditViewModel()
                {
                    ConsumableId = x.ConsumableId.ToString(),
                    Name = x.Name,
                    Units = x.Units,
                    Description = x.Description,
                    Price = x.Price,
                    ROB = x.ROB
                })
                .FirstOrDefaultAsync();
            return model ?? new ConsumableEditViewModel();

        }

        public async Task<bool> SaveItemToEditAsync(ConsumableEditViewModel model, string userId)
        {
            Consumable? consToEdit = await consumables.GetAllAsQueryable()
              .Where(x => x.IsDeleted == false)
              .Where(x => x.ConsumableId.ToString() == model.ConsumableId)
              .FirstOrDefaultAsync();

            if (consToEdit == null)
            {
                return false;
            }
            consToEdit.Name = model.Name;
            consToEdit.Units = model.Units;
            consToEdit.Description = model.Description;
            consToEdit.Price = model.Price;
            consToEdit.ROB = model.ROB;
            consToEdit.CreatorId = userId;
            consToEdit.IsDeleted = false;
            consToEdit.EditedOn = DateTime.Now;

            await consumables.UpdateAsync(consToEdit);

            return true;
        }


        public async Task<bool> ConfirmDeleteAsync(ConsumableDeleteViewModel model)
        {
            Consumable? consToDelete = await consumables.GetAllAsQueryable()
                .Where(x => x.IsDeleted == false)
                .Where(x => x.ConsumableId.ToString() == model.ConsumableId)
                .FirstOrDefaultAsync();
            if (consToDelete != null)
            {
                consToDelete.IsDeleted = true;
                await consumables.UpdateAsync(consToDelete);
                return true;
            }
            else
            {
                return false;
            }
           
        }

        public async Task<ConsumableDeleteViewModel> GetItemToDeleteAsync(string id)
        {
            var model = await consumables.GetAllAsQueryable()
                .Where(x => x.IsDeleted == false)
                .Where(x => x.ConsumableId.ToString() == id)
                .AsNoTracking()
                .Select(x => new ConsumableDeleteViewModel()
                {
                    ConsumableId = x.ConsumableId.ToString(),
                    Name = x.Name,
                    CreatedOn = x.CreatedOn,
                    EditedOn = x.EditedOn,
                    CreatorId = x.CreatorId,
                    CreatorName = x.Creator.UserName ?? string.Empty
                })
                .FirstOrDefaultAsync();

            return model ?? new ConsumableDeleteViewModel();
        }
    }
}
