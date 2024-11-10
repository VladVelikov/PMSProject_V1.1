using PMSWeb.ViewModels.CountryVM;

namespace PMS.Services.Data.Interfaces
{
    public interface ICountryService
    {
        public Task<ICollection<CountryDisplayViewModel>> GetListOfCountriesAsync();

        public Task<bool> CreateCountryAsync(CountryCreateViewModel model);

        public Task<CountryDeleteViewModel> GetDeleteCountryModelAsync(string cityId);

        public Task<bool> DeleteCountryModelAsync(CountryDeleteViewModel deleteModel);
    }
}
