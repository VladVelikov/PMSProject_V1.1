﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PMS.Data;
using PMS.Data.Models;
using PMSWeb.ViewModels.RequisitionVM;
using static PMS.Common.EntityValidationConstants;

namespace PMSWeb.Controllers
{
    [Authorize]
    public class RequisitionController(PMSDbContext context) : BasicController
    {
        [HttpGet]
        public async Task<IActionResult> Select()  // all
        {
            var reqList = await context
                .Requisitions
                .Where(x => !x.IsDeleted)
                .AsNoTracking()
                .OrderByDescending(x=>x.CreatedOn)
                .Select(x => new RequisitionDisplayViewModel() {
                    RequisitionId = x.RequisitionId.ToString(),
                    RequisitionName = x.RequisitionName,
                    CreatedOn = x.CreatedOn.ToString(PMSRequiredDateFormat),
                    IsApproved = x.IsApproved,  
                    RequisitionType = x.RequisitionType,
                    Creator = x.Creator.UserName ?? "Unknown",
                    TotalCost = x.TotalCost.ToString("C")
                })
                .ToListAsync();
            return View(reqList);  
        }

        [HttpGet]
        public async Task<IActionResult> SelectReadyForApproval()
        {
            var reqList = await context
                .Requisitions
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
            return View(reqList);
        }

        [HttpGet]
        public async Task<IActionResult> SelectAlreadyApproved(string x)
        {
            var reqList = await context
                .Requisitions
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
            return View(reqList);
        }

        [HttpGet]
        public async Task<IActionResult> CreateSpareparts()
        {
            var model = new RequisitionCreateViewModel()
            {
                RequisitionType = "Spareparts"
            };
            var sparesList = await context
                .Spareparts
                .Where(x => !x.IsDeleted)
                .Include(x => x.SparepartsSuppliers)
                .AsNoTracking()
                .Select(x => new RequisitionItemViewModel() {
                    Id = x.SparepartId.ToString(),
                    Name = x.SparepartName,
                    Available = x.ROB,
                    ToOrdered = 0,
                    Units = x.Units,
                    Price = x.Price,
                    IsSelected = false,
                    Suppliers = x.SparepartsSuppliers.Select(x=>x.Supplier).ToList()
                })
                .ToListAsync();  
            model.RequisitionItems = sparesList;
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> CreateConsumables()
        {
            var model = new RequisitionCreateViewModel()
            {
                RequisitionType = "Consumables"
            };
            var sparesList = await context
                .Consumables
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
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(RequisitionCreateViewModel model)
        {
            decimal totalCost = 0;
            List<RequisitionItem> requisitionItems = new List<RequisitionItem>();
            
            Requisition requisition = new Requisition()
            {
                //RequisitionId = Guid.NewGuid(),
                RequisitionName = model.RequisitionName ?? "Unknown",
                CreatedOn = DateTime.UtcNow,
                RequisitionType = model.RequisitionType ?? "Unknown",
                CreatorId = GetUserId(),
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
            await context.Requisitions.AddAsync(requisition);
            await context.SaveChangesAsync();

            return RedirectToAction(nameof(Select));
        }

        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            var model = await context
                .Requisitions
                .Where(x => !x.IsDeleted)
                .Where(x => x.RequisitionId.ToString().ToLower() == id.ToLower())
                .AsNoTracking()
                .Select(x=> new RequisitionDetailsViewModel() {
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
                return View(new RequisitionDetailsViewModel());
            }
            var reqItems = await context
                .RequisitionItems
                .AsNoTracking ()
                .Where(x=>x.RequisitionId.ToString().ToLower() == id.ToLower())
                .Select(x=> new RequisitionItemViewModel() {
                    Id = x.ItemId.ToString(),
                    Name = x.Name,
                    ToOrdered = x.OrderedAmount,
                    Units = x.Units,
                    Price = x.Price,
                    SupplierName = x.SupplierName ?? "Unknown",
                })
                .ToListAsync();
             model.requisitionItems = reqItems;  
            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            var reqToDeleteModel = await context
                .Requisitions
                .Where(x=>!x.IsDeleted)
                .Where(x=>x.RequisitionId.ToString().ToLower() == id.ToLower())
                .Select(x=> new RequisitionDeleteViewModel() {
                    RequisitionId = x.RequisitionId.ToString(),
                    RequisitionName = x.RequisitionName,
                    CreatedOn = x.CreatedOn.ToString(PMSRequiredDateFormat),
                    RequisitionType = x.RequisitionType
                })
                .FirstOrDefaultAsync();

            return View(reqToDeleteModel);
        }


        [HttpPost]
        public async Task<IActionResult> ConfirmDelete(string id)
        {
            var reqToDel = await context
                .Requisitions
                .Where(x=>!x.IsDeleted)
                .Where(x=>x.RequisitionId.ToString().ToLower() == id.ToLower())
                .FirstOrDefaultAsync();
            if (reqToDel != null)
            {
                reqToDel.IsDeleted = true;
                await context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Select));
        }

        [HttpGet]
        public async Task<IActionResult> Approve(string id)
        {
            var reqToApprove = await context
                .Requisitions
                .Where(x => !x.IsDeleted)
                .Where(x => !x.IsApproved)
                .Where(x => x.RequisitionId.ToString().ToLower() == id.ToLower())
                .Include(x=>x.requisitionItems)
                .FirstOrDefaultAsync();

            if (reqToApprove == null || reqToApprove.IsApproved)
            {
                return RedirectToAction(nameof(Select));
            }

            var budget = await context
                .Budget
                .OrderByDescending(x => x.LastChangeDate)
                .FirstOrDefaultAsync();
            
            if (budget.Ballance < reqToApprove.TotalCost)
            {
                return RedirectToAction("LowBallance", new { ballance = budget.Ballance });
            }
            

            var reqItemsList = reqToApprove.requisitionItems;
            if (reqToApprove.RequisitionType == "Consumables")
            {
                   var Consumables = await context
                    .Consumables
                    .Where(x => !x.IsDeleted)
                    .ToListAsync();
                foreach (var item in reqItemsList)
                {
                    var consumableToUpdate = Consumables.FirstOrDefault(x => x.ConsumableId == item.PurchasedItemId);
                    if (consumableToUpdate != null)
                    {
                        if (item.OrderedAmount > 0)
                        {
                            consumableToUpdate.ROB += item.OrderedAmount;
                            consumableToUpdate.EditedOn = DateTime.UtcNow;
                        }
                    }
                }
            }
            else 
            {
                   var Spareparts = await context
                    .Spareparts
                    .Where(x => !x.IsDeleted)
                    .ToListAsync();
                foreach (var item in reqItemsList)
                {
                    var sparepartToUpdate = Spareparts.FirstOrDefault(x => x.SparepartId == item.PurchasedItemId);
                    if (sparepartToUpdate != null)
                    {
                        if (item.OrderedAmount > 0)
                        {
                            sparepartToUpdate.ROB += item.OrderedAmount;
                            sparepartToUpdate.EditedOn = DateTime.UtcNow;
                        }
                    }
                }
            }

            budget.Ballance -= reqToApprove.TotalCost;
            reqToApprove.IsApproved = true;
            await context.SaveChangesAsync();

            if (reqToApprove.RequisitionType == "Consumables")
            {
              return RedirectToAction("ConsumablesInventory", "Inventory");
            }
            else 
            {
              return RedirectToAction("SparesInventory", "Inventory");
            }
              

        }

        [HttpGet]
        public IActionResult LowBallance()
        {
            return View();
        }


    }
}