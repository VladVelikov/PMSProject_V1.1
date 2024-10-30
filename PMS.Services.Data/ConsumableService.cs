using Microsoft.EntityFrameworkCore;
using PMS.Data;
using PMS.Data.Models;
using PMS.Data.Repository.Interfaces;
using PMS.Services.Data.Interfaces;
using PMSWeb.ViewModels.Consumable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMS.Services.Data
{
    public class ConsumableService(PMSDbContext context, IRepository<Consumable, Guid> consumables) : IConsumableService
    {
        public Task<bool> ConfirmDeleteAsync(ConsumableDeleteViewModel model)
        {
            throw new NotImplementedException();
        }

        public Task<bool> CreateConsumableAsync(ConsumableCreateViewModel model)
        {
            throw new NotImplementedException();
        }

        public Task<bool> GetDetailsAsync(string Id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> GetItemForEditAsync(string Id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> GetItemToDeleteAsync(string Id)
        {
            throw new NotImplementedException();
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

        public Task<bool> SaveItemToEditAsync(ConsumableEditViewModel model)
        {
            throw new NotImplementedException();
        }
    }
}
