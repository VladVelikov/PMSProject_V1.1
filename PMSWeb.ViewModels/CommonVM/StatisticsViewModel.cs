using static PMS.Common.EntityValidationConstants;


namespace PMSWeb.ViewModels.CommonVM
{
    public class StatisticsViewModel
    {
        public int CompletedRequisitions { get; set; }
        public int TotalRequisitions { get; set; }
        public int RequisitionsReadyToApprove { get; set; }

        public int CompletedJobOrders { get; set; }
        public int DueJobs { get; set; }
        public int AllJobs { get; set; }

        public decimal RemainingBudget { get; set; }
        public decimal TotalBudget { get; set; } = MaxBudget;
    }
}
