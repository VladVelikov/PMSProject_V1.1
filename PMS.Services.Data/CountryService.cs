using Microsoft.EntityFrameworkCore;
using PMS.Data.Models;
using PMS.Data.Repository.Interfaces;
using PMS.Services.Data.Interfaces;
using PMSWeb.ViewModels.CountryVM;
using static PMS.Common.EntityValidationConstants;

namespace PMS.Services.Data
{
    public class CountryService(IRepository<Country,Guid> countries) : ICountryService
    {
        public async Task<bool> CreateCountryAsync(CountryCreateViewModel model)
        {
            if (model == null || model.Name == null) return false;

            Country country = new Country()
            {
                Name = model.Name,
                CreatedOn = DateTime.Now,
                EditedOn = DateTime.Now
            };
            try
            {
                await countries.AddAsync(country);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public async Task<bool> DeleteCountryModelAsync(CountryDeleteViewModel deleteModel)
        {
            if (deleteModel == null || deleteModel.CountryId == null)
            {
                return false;
            }
            var modelDel = await countries.GetAllAsQueryable()
               .Where(x => x.CountryId.ToString().ToLower() == deleteModel.CountryId.ToLower())
               .FirstOrDefaultAsync();

            if (modelDel == null)
            {
                return false;
            }
            try
            {
                await countries.DeleteByIdAsync(Guid.Parse(deleteModel.CountryId));
            }
            catch
            {
                return false;
            }
            return true;
        }

        public async Task<CountryDeleteViewModel> GetDeleteCountryModelAsync(string cityId)
        {
            var model = await countries.GetAllAsQueryable()
                .AsNoTracking()
                .Where(x => x.CountryId.ToString().ToLower() == cityId.ToLower())
                .Select(x => new CountryDeleteViewModel()
                {
                    CountryId = x.CountryId.ToString(),
                    Name = x.Name,
                    CreatedOn = x.CreatedOn.ToString(PMSRequiredDateTimeFormat)
                })
                .FirstOrDefaultAsync();
            if (model != null)
            {
                return model;
            }
            return new CountryDeleteViewModel();
        }

        public async Task<ICollection<CountryDisplayViewModel>> GetListOfCountriesAsync()
        {
            var modelList = await countries
                .GetAllAsQueryable()
                .AsNoTracking()
                .OrderByDescending(x => x.CreatedOn)
                .Select(x => new CountryDisplayViewModel()
                {
                    Name = x.Name,
                    CreatedOn = x.CreatedOn.ToString(PMSRequiredDateFormat),
                    CountryId = x.CountryId.ToString()
                })
                .ToListAsync();
            if (modelList == null)
            {
                return new List<CountryDisplayViewModel>();
            }

            return modelList;
        }
    }
}
