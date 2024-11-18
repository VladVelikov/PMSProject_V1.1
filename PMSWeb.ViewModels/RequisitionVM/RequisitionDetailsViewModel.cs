namespace PMSWeb.ViewModels.RequisitionVM
{
    public class RequisitionDetailsViewModel
    {
        public string RequisitionId { get; set; }
        public string RequisitionName { get; set; }
        public string CreatedOn { get; set; }
        public string RequisitionType { get; set; }
        public string Creator {  get; set; }
        public string TotalCost { get; set; }

        public List<RequisitionItemViewModel> requisitionItems { get; set; } = new List<RequisitionItemViewModel>();    

    }
}
