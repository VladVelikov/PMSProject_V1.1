using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PMS.Data;
using PMS.Data.Models;
using PMS.Services.Data.Interfaces;
using PMSWeb.ViewModels.RequisitionVM;
using static PMS.Common.EntityValidationConstants;

namespace PMSWeb.Controllers
{
    [Authorize]
    public class RequisitionController(PMSDbContext context, IRequisitionService requisitionService) : BasicController
    {
        [HttpGet]
        public async Task<IActionResult> Select()  // all
        {
            var reqList = await requisitionService.GetAllItemsListAsync();
            return View(reqList);  
        }

        [HttpGet]
        public async Task<IActionResult> SelectReadyForApproval()
        {
            var reqList = await requisitionService.GetAllReadyForApprovalAsync();
            return View(reqList);
        }

        [HttpGet]
        public async Task<IActionResult> SelectAlreadyApproved()
        {
            var reqList = await requisitionService.GetAllApprovedAsync();
            return View(reqList);
        }

        [HttpGet]
        public async Task<IActionResult> CreateSpareparts()
        {
            var model = await requisitionService.GetCreateSparesRequisitionModelAsync();
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> CreateConsumables()
        {
            var model = await requisitionService.GetCreateConsumablesRequisitionModelAsync();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(RequisitionCreateViewModel model)
        {
            bool result = await requisitionService.CreateRequisitionAsync(model, GetUserId());
            return RedirectToAction(nameof(Select));
        }

        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
           var model = await requisitionService.GetRequisitionDetailsModelAsync(id);
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            var reqToDeleteModel = await requisitionService.GetRequisitionDeleteViewModelAsync(id);
            return View(reqToDeleteModel);
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmDelete(string id)
        {
            bool result = await requisitionService.DeleteRequisitionAsync(id);
            return RedirectToAction(nameof(Select));
        }

        [HttpGet]
        public async Task<IActionResult> Approve(string id)
        {
            var result = await requisitionService.ApproveRequisition(id);
            if (result == "NullOrApproved")
            {
                return RedirectToAction(nameof(Select));
            }
            else if (result == "LowBallance")
            {
                return RedirectToAction(nameof(LowBallance));
            }
            else if (result == "Consumables")
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
