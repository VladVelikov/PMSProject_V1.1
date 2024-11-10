using PMS.Data.Models;
using PMSWeb.ViewModels.CityVM;

namespace PMS.Services.Data.Interfaces
{
    public interface ICityService
    {
        public Task<ICollection<CityDisplayViewModel>> GetListOfCitiesAsync();

        public Task<bool> CreateCityAsync(CityCreateViewModel model);

        public Task<CityDeleteViewModel> GetDeleteCityModelAsync(string cityId);
        
        public Task<bool> DeleteCityModelAsync(CityDeleteViewModel deleteModel);
    }
}
