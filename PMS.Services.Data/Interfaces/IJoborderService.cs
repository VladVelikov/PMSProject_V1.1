using PMSWeb.ViewModels.JobOrderVM;

namespace PMS.Services.Data.Interfaces
{
    public interface IJoborderService
    {
        public Task <List<JobOrderDisplayViewModel>> GetListOfAllJobsAsync();
        
        public Task <List<JobOrderDisplayViewModel>> GetListOfDueJobsAsync();
        
        public Task <List<JobOrderHistoryViewModel>> GetListOfHistoryJobsAsync();

        public Task<JobHistoryDetailsViewModel> GetHistoryDetailsAsync(string id);

        public Task<JobOrderCreateViewModel> GetCreateJobModelAsync(JobOrderAddMaintenanceViewModel inputModel);

        public Task<bool> CreateJobOrderAsync(JobOrderCreateViewModel model, string userId);

        public Task<JobOrderAddMaintenanceViewModel> GetAddRoutineMaintenanceViewModelAsync(Guid equipmentId, string maintenanceType);
        
        public Task<JobOrderAddMaintenanceViewModel> GetAddSpecificMaintenanceViewModelAsync(Guid equipmentId, string maintenanceType);

        public Task<JobOrderAddEquipmentViewModel> GetAddEquipmentModelAsync();

        public Task<bool> DeleteJobOrderAsync(string id);

        public Task<CompleteTheJobViewModel> GetCompleteJobModelAsync(string id);

        public Task<bool> CloseThisJob(CompleteTheJobViewModel model, string userName);

        public Task<PartialViewModel> GetSparesPartialModelAsync(string id);

        public Task<PartialViewModel> GetConsumablesPartialModelAsync(string id);

        public Task<bool> ConfirmSparesAreUsedAsync(PartialViewModel model);

        public Task<bool> ConfirmConsumablesAreUsedAsync(PartialViewModel model);

        public Task<SelectManualViewModel> GetSelectManualViewModelAsync(string id);

        public Task<OpenManualViewModel> GetOpenManualViewModelAsync(string jobid, string manualid);

    }
}
