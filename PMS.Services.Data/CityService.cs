using Microsoft.EntityFrameworkCore;
using PMS.Data.Models;
using PMS.Data.Repository.Interfaces;
using PMS.Services.Data.Interfaces;
using PMSWeb.ViewModels.CityVM;
using static PMS.Common.EntityValidationConstants;

namespace PMS.Services.Data
{
    public class CityService(IRepository<City,Guid> cities) : ICityService
    {
        public async Task<bool> CreateCityAsync(CityCreateViewModel model)
        {
            if (model == null || model.Name == null) return false; 
            
            City city = new City()
            {
                Name = model.Name,
                CreatedOn = DateTime.Now,
                EditedOn = DateTime.Now
            };
            try
            {
                await cities.AddAsync(city);
            }
            catch 
            { 
                return false; 
            }
            return true;
        }

        public async Task<bool> DeleteCityModelAsync(CityDeleteViewModel deleteModel)
        {
            if (deleteModel == null || deleteModel.CityId == null)
            {
                return false;
            }
            var modelDel = cities.GetAllAsQueryable()
               .Where(x => x.CityId.ToString().ToLower() == deleteModel.CityId.ToLower())
               .FirstOrDefaultAsync();

            if (modelDel == null)
            {
                return false;
            }
            try
            {
                await cities.DeleteByIdAsync(Guid.Parse(deleteModel.CityId));
            }
            catch 
            {
                return false;
            }
            return true;
        }

        public async Task<CityDeleteViewModel> GetDeleteCityModelAsync(string cityId)
        {
            var model = await cities.GetAllAsQueryable() 
                .AsNoTracking()
                .Where(x => x.CityId.ToString().ToLower() == cityId.ToLower())
                .Select(x => new CityDeleteViewModel()
                {
                    CityId = x.CityId.ToString(),
                    Name = x.Name,
                    CreatedOn = x.CreatedOn.ToString(PMSRequiredDateTimeFormat)
                })
                .FirstOrDefaultAsync();
            if (model != null)
            { 
              return model;
            }
            return new CityDeleteViewModel();
        }

        public async Task<ICollection<CityDisplayViewModel>> GetListOfCitiesAsync()
        {
            var modelList = await cities
                .GetAllAsQueryable()
                .AsNoTracking()
                .OrderByDescending(x => x.CreatedOn)
                .Select(x => new CityDisplayViewModel()
                {
                    Name = x.Name,
                    CreatedOn = x.CreatedOn.ToString(PMSRequiredDateFormat),
                    CityId = x.CityId.ToString()
                })
                .ToListAsync();
            if (modelList == null)
            {
                return new List<CityDisplayViewModel>();
            }

            return modelList;
        }
    }
}
