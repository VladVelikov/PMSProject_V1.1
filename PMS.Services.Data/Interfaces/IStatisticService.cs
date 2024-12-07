using PMSWeb.ViewModels.CommonVM;

namespace PMS.Services.Data.Interfaces
{
    public interface IStatisticService
    {
        public Task<StatisticsViewModel> GetStatisticVieModelAsync();
    }
}
