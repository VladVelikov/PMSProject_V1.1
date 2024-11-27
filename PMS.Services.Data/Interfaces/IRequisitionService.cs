using PMSWeb.ViewModels.RequisitionVM;

namespace PMS.Services.Data.Interfaces
{
    public interface IRequisitionService
    {
        public Task<List<RequisitionDisplayViewModel>> GetAllItemsListAsync();
        
        public Task<List<RequisitionDisplayViewModel>> GetAllReadyForApprovalAsync();
        
        public Task<List<RequisitionDisplayViewModel>> GetAllApprovedAsync();

        public Task<RequisitionCreateViewModel> GetCreateSparesRequisitionModelAsync();
        
        public Task<RequisitionCreateViewModel> GetCreateConsumablesRequisitionModelAsync();

        public Task<bool> CreateRequisitionAsync(RequisitionCreateViewModel model, string userId);

        public Task<RequisitionDetailsViewModel> GetRequisitionDetailsModelAsync(string id);

        public Task<RequisitionDeleteViewModel> GetRequisitionDeleteViewModelAsync(string id);
        
        public Task<bool> DeleteRequisitionAsync(string id);

        public Task<string> ApproveRequisition(string id);    

    }
}
