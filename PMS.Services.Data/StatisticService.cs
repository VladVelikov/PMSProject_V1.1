using Microsoft.EntityFrameworkCore;
using PMS.Data.Models;
using PMS.Data.Repository.Interfaces;
using PMS.Services.Data.Interfaces;
using PMSWeb.ViewModels.CommonVM;

namespace PMS.Services.Data
{
    public class StatisticService(IRepository<Requisition, Guid> requisitionsRepo,
                                  IRepository<JobOrder, Guid> jobOrdersRepo,
                                  IRepository<Budget, Guid> budgetRepo)
                                : IStatisticService
    {
        public async Task<StatisticsViewModel> GetStatisticVieModelAsync()
        {
            var model = new StatisticsViewModel();
            try 
            {
                var completedReq = await requisitionsRepo
                   .GetAllAsQueryable()
                   .Where(x => !x.IsDeleted)
                   .Where(x => x.IsApproved)
                   .CountAsync();
                var totalRequisitions = await requisitionsRepo
                   .GetAllAsQueryable()
                   .Where(x => !x.IsDeleted)
                   .CountAsync();
                var readyToApproveReq = await requisitionsRepo
                   .GetAllAsQueryable()
                   .Where(x => !x.IsDeleted)
                   .Where(x => !x.IsApproved)
                   .CountAsync();
                model.CompletedRequisitions = completedReq;
                model.TotalRequisitions = totalRequisitions;
                model.RequisitionsReadyToApprove = readyToApproveReq;
            }
            catch 
            {
                model.CompletedRequisitions = 0;
                model.TotalRequisitions = 0;
                model.RequisitionsReadyToApprove = 0;
            }

            try 
            {
                var allJobs = await jobOrdersRepo
                .GetAllAsQueryable()
                .Where(x => !x.IsDeleted)
                .CountAsync();
                var dueJobs = await jobOrdersRepo
                    .GetAllAsQueryable()
                    .Where(x => !x.IsDeleted)
                    .Where(x => !x.IsHistory)
                    .CountAsync();
                var completedJobs = await jobOrdersRepo
                    .GetAllAsQueryable()
                    .Where(x => !x.IsDeleted)
                    .Where(x => x.IsHistory)
                    .CountAsync();
                model.AllJobs = allJobs;
                model.DueJobs = dueJobs;
                model.CompletedJobOrders = completedJobs;
            }
            catch 
            {
                model.AllJobs = 0;
                model.DueJobs = 0;
                model.CompletedJobOrders = 0;
            }

            decimal currentBallance;
            var budget = await budgetRepo
                .GetAllAsQueryable()
                .OrderByDescending(x => x.LastChangeDate)
                .FirstOrDefaultAsync();
            if (budget == null)
            {
                currentBallance = 0;
            }
            else
            {
                currentBallance = budget.Ballance;
            }
            model.RemainingBudget = currentBallance;
            return model;
        }
    }
}
