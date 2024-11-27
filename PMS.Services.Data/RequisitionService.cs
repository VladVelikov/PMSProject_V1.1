using Microsoft.EntityFrameworkCore;
using PMS.Data.Models;
using PMS.Data.Repository.Interfaces;
using PMS.Services.Data.Interfaces;
using PMSWeb.ViewModels.RequisitionVM;
using static PMS.Common.EntityValidationConstants;

namespace PMS.Services.Data
{
    public class RequisitionService(IRepository<Requisition, Guid> requisitionRepo,
                                    IRepository<Sparepart, Guid> sparesRepo,
                                    IRepository<Consumable, Guid> consumablesRepo,
                                    IRepository<RequisitionItem, Guid> reqItemsRepo,
                                    IRepository<Budget, Guid> budgetRepo) : IRequisitionService
    {

        public async Task<List<RequisitionDisplayViewModel>> GetAllItemsListAsync()
        {
            var reqList = await requisitionRepo
               .GetAllAsQueryable()
               .Where(x => !x.IsDeleted)
               .AsNoTracking()
               .OrderByDescending(x => x.CreatedOn)
               .Select(x => new RequisitionDisplayViewModel()
               {
                   RequisitionId = x.RequisitionId.ToString(),
                   RequisitionName = x.RequisitionName,
                   CreatedOn = x.CreatedOn.ToString(PMSRequiredDateFormat),
                   IsApproved = x.IsApproved,
                   RequisitionType = x.RequisitionType,
                   Creator = x.Creator.UserName ?? "Unknown",
                   TotalCost = x.TotalCost.ToString("C")
               })
               .ToListAsync();
            return reqList;
        }

        public async Task<List<RequisitionDisplayViewModel>> GetAllReadyForApprovalAsync()
        {
            var reqList = await requisitionRepo
                .GetAllAsQueryable()
                .Where(x => !x.IsDeleted)
                .Where(x => !x.IsApproved)
                .AsNoTracking()
                .OrderByDescending(x => x.CreatedOn)
                .Select(x => new RequisitionDisplayViewModel()
                {
                    RequisitionId = x.RequisitionId.ToString(),
                    RequisitionName = x.RequisitionName,
                    CreatedOn = x.CreatedOn.ToString(PMSRequiredDateFormat),
                    IsApproved = x.IsApproved,
                    RequisitionType = x.RequisitionType,
                    Creator = x.Creator.UserName ?? "Unknown",
                    TotalCost = x.TotalCost.ToString("C")
                })
                .ToListAsync();
            return reqList;
        }

        public async Task<List<RequisitionDisplayViewModel>> GetAllApprovedAsync()
        {
            var reqList = await requisitionRepo
                .GetAllAsQueryable()
                .Where(x => !x.IsDeleted)
                .Where(x => x.IsApproved)
                .AsNoTracking()
                .OrderByDescending(x => x.CreatedOn)
                .Select(x => new RequisitionDisplayViewModel()
                {
                    RequisitionId = x.RequisitionId.ToString(),
                    RequisitionName = x.RequisitionName,
                    CreatedOn = x.CreatedOn.ToString(PMSRequiredDateFormat),
                    IsApproved = x.IsApproved,
                    RequisitionType = x.RequisitionType,
                    Creator = x.Creator.UserName ?? "Unknown",
                    TotalCost = x.TotalCost.ToString("C")
                })
                .ToListAsync();
            return reqList;
        }

        public async Task<RequisitionCreateViewModel> GetCreateSparesRequisitionModelAsync()
        {
            var model = new RequisitionCreateViewModel()
            {
                RequisitionType = "spareparts"
            };
            var sparesList = await sparesRepo
                .GetAllAsQueryable()
                .Where(x => !x.IsDeleted)
                .Include(x => x.SparepartsSuppliers)
                .AsNoTracking()
                .Select(x => new RequisitionItemViewModel()
                {
                    Id = x.SparepartId.ToString(),
                    Name = x.SparepartName,
                    Available = x.ROB,
                    ToOrdered = 0,
                    Units = x.Units,
                    Price = x.Price,
                    IsSelected = false,
                    Suppliers = x.SparepartsSuppliers.Select(x => x.Supplier).ToList()
                })
                .ToListAsync();
            model.RequisitionItems = sparesList;
            return model;
        }

        public async Task<RequisitionCreateViewModel> GetCreateConsumablesRequisitionModelAsync()
        {
            var model = new RequisitionCreateViewModel()
            {
                RequisitionType = "consumables"
            };
            var sparesList = await consumablesRepo
                .GetAllAsQueryable()
                .Where(x => !x.IsDeleted)
                .Include(x => x.ConsumablesSuppliers)
                .AsNoTracking()
                .Select(x => new RequisitionItemViewModel()
                {
                    Id = x.ConsumableId.ToString(),
                    Name = x.Name,
                    Available = x.ROB,
                    ToOrdered = 0,
                    Units = x.Units,
                    Price = x.Price,
                    IsSelected = false,
                    Suppliers = x.ConsumablesSuppliers.Select(x => x.Supplier).ToList()
                })
                .ToListAsync();
            model.RequisitionItems = sparesList;
            return model;
        }

        public async Task<bool> CreateRequisitionAsync(RequisitionCreateViewModel model, string userId)
        {
            decimal totalCost = 0;
            List<RequisitionItem> requisitionItems = new List<RequisitionItem>();

            Requisition requisition = new Requisition()
            {
                //RequisitionId = Guid.NewGuid(),
                RequisitionName = model.RequisitionName ?? "Unknown",
                CreatedOn = DateTime.UtcNow,
                RequisitionType = model.RequisitionType ?? "Unknown",
                CreatorId = userId,
                IsDeleted = false,
                TotalCost = totalCost
            };

            foreach (var item in model.RequisitionItems)
            {
                if (item.IsSelected)
                {
                    RequisitionItem requisitionItem = new RequisitionItem()
                    {
                        PurchasedItemId = Guid.Parse(item.Id),
                        Name = item.Name,
                        OrderedAmount = item.ToOrdered,
                        Units = item.Units,
                        Price = item.Price,
                        Type = requisition.RequisitionType,
                        SupplierName = item.SupplierName,
                        RequisitionId = requisition.RequisitionId,
                        CreatedOn = DateTime.UtcNow
                    };
                    requisitionItems.Add(requisitionItem);
                    totalCost += (decimal)((decimal)item.ToOrdered * item.Price);
                }
            }
            requisition.TotalCost = totalCost;
            requisition.requisitionItems = requisitionItems;
            await requisitionRepo.AddAsync(requisition);
            return true;
        }
        
        public async Task<RequisitionDetailsViewModel> GetRequisitionDetailsModelAsync(string id)
        {
            var model = await requisitionRepo
               .GetAllAsQueryable()
               .Where(x => !x.IsDeleted)
               .Where(x => x.RequisitionId.ToString().ToLower() == id.ToLower())
               .AsNoTracking()
               .Select(x => new RequisitionDetailsViewModel()
               {
                   RequisitionId = x.RequisitionId.ToString(),
                   RequisitionName = x.RequisitionName,
                   CreatedOn = x.CreatedOn.ToString(PMSRequiredDateFormat),
                   IsApproved = x.IsApproved,
                   RequisitionType = x.RequisitionType,
                   Creator = x.Creator.UserName ?? "Unknown",
                   TotalCost = x.TotalCost.ToString("C"),
               })
               .FirstOrDefaultAsync();
            if (model == null)
            {
                new RequisitionDetailsViewModel();
            }
            var reqItems = await reqItemsRepo
                .GetAllAsQueryable()
                .AsNoTracking()
                .Where(x => x.RequisitionId.ToString().ToLower() == id.ToLower())
                .Select(x => new RequisitionItemViewModel()
                {
                    Id = x.ItemId.ToString(),
                    Name = x.Name,
                    ToOrdered = x.OrderedAmount,
                    Units = x.Units,
                    Price = x.Price,
                    SupplierName = x.SupplierName ?? "Unknown",
                })
                .ToListAsync();
            model.requisitionItems = reqItems;
            return model;   
        }

        public async Task<RequisitionDeleteViewModel> GetRequisitionDeleteViewModelAsync(string id)
        {
            var reqToDeleteModel = await requisitionRepo
                .GetAllAsQueryable()
                .Where(x => !x.IsDeleted)
                .Where(x => x.RequisitionId.ToString().ToLower() == id.ToLower())
                .Select(x => new RequisitionDeleteViewModel()
                {
                    RequisitionId = x.RequisitionId.ToString(),
                    RequisitionName = x.RequisitionName,
                    CreatedOn = x.CreatedOn.ToString(PMSRequiredDateFormat),
                    RequisitionType = x.RequisitionType
                })
                .FirstOrDefaultAsync();

            return reqToDeleteModel ?? new RequisitionDeleteViewModel();
        }
        
        public async Task<bool> DeleteRequisitionAsync(string id)
        {
            var reqToDel = await requisitionRepo
                 .GetAllAsQueryable()
                 .Where(x => !x.IsDeleted)
                 .Where(x => x.RequisitionId.ToString().ToLower() == id.ToLower())
                 .FirstOrDefaultAsync();
            if (reqToDel != null)
            {
                reqToDel.IsDeleted = true;
                await requisitionRepo.UpdateAsync(reqToDel);
                return true;
            }
            return false;
        }

        public async Task<string> ApproveRequisition(string id)
        {
            var reqToApprove = await requisitionRepo
                .GetAllAsQueryable()
                .Where(x => !x.IsDeleted)
                .Where(x => !x.IsApproved)
                .Where(x => x.RequisitionId.ToString().ToLower() == id.ToLower())
                .Include(x => x.requisitionItems)
                .FirstOrDefaultAsync();

            if (reqToApprove == null || reqToApprove.IsApproved)
            {
                return "NullOrApproved";
            }

            var budget = await budgetRepo
                .GetAllAsQueryable()
                .OrderByDescending(x => x.LastChangeDate)
                .FirstOrDefaultAsync();

            if (budget.Ballance < reqToApprove.TotalCost)
            {
                return "LowBallance";
            }


            var reqItemsList = reqToApprove.requisitionItems;

            budget.Ballance -= reqToApprove.TotalCost;      // to find a better way 
            await budgetRepo.UpdateAsync(budget);

            reqToApprove.IsApproved = true;                   // to find a better way 
            await requisitionRepo.UpdateAsync(reqToApprove);
            
            if (reqToApprove.RequisitionType == "consumables")
            {
                var consumables = await consumablesRepo
                 .GetAllAsQueryable()
                 .Where(x => !x.IsDeleted)
                 .ToListAsync();
                foreach (var item in reqItemsList)
                {
                    var consumableToUpdate = consumables.FirstOrDefault(x => x.ConsumableId == item.PurchasedItemId);
                    if (consumableToUpdate != null)
                    {
                        if (item.OrderedAmount > 0)
                        {
                            consumableToUpdate.ROB += item.OrderedAmount;
                            consumableToUpdate.EditedOn = DateTime.UtcNow;
                        }
                    }
                }
                await consumablesRepo.UpdateRange(consumables);
                return "Consumables";
            }
            else
            {
                var spareparts = await sparesRepo
                 .GetAllAsQueryable()
                 .Where(x => !x.IsDeleted)
                 .AsNoTracking()
                 .ToListAsync();
                foreach (var item in reqItemsList)
                {
                    var sparepartToUpdate = spareparts.FirstOrDefault(x => x.SparepartId == item.PurchasedItemId);
                    if (sparepartToUpdate != null)
                    {
                        if (item.OrderedAmount > 0)
                        {
                            sparepartToUpdate.ROB += item.OrderedAmount;
                            sparepartToUpdate.EditedOn = DateTime.UtcNow;
                        }
                    }
                }
                await sparesRepo.UpdateRange(spareparts);
                return "Spares";
            }
            
        }
    }
}
