using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PMS.Data.Models;
using PMS.Services.Data.Interfaces;
using PMSWeb.ViewModels.RequisitionVM;

namespace PMSWeb.Controllers
{
    [Authorize]
    public class RequisitionController(IRequisitionService requisitionService) : BasicController
    {
        [HttpGet]
        public async Task<IActionResult> Select()  // all
        {
            var reqList = await requisitionService.GetAllItemsListAsync();
            if (reqList == null)
            {
                return RedirectToAction("EmptyList", "Crushes");
            }
            return View(reqList);  
        }

        [HttpGet]
        public async Task<IActionResult> SelectReadyForApproval()
        {
            var reqList = await requisitionService.GetAllReadyForApprovalAsync();
            if (reqList == null)
            {
                return RedirectToAction("EmptyList", "Crushes");
            }
            return View(reqList);
        }

        [HttpGet]
        public async Task<IActionResult> SelectAlreadyApproved()
        {
            var reqList = await requisitionService.GetAllApprovedAsync();
            if (reqList == null)
            {
                return RedirectToAction("EmptyList", "Crushes");
            }
            return View(reqList);
        }

        [HttpGet]
        public async Task<IActionResult> CreateSpareparts()
        {
            var model = await requisitionService.GetCreateSparesRequisitionModelAsync();
            if (model == null || model.RequisitionItems == null)
            {
                return RedirectToAction("EmptyList", "Crushes");
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> CreateConsumables()
        {
            var model = await requisitionService.GetCreateConsumablesRequisitionModelAsync();
            if (model == null || model.RequisitionItems == null)
            {
                return RedirectToAction("EmptyList", "Crushes");
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(RequisitionCreateViewModel model)
        {
            // Seperate model properties validation to avoid errors due to the presence of list items
            if (string.IsNullOrWhiteSpace(GetUserId()) || !IsValidGuid(GetUserId()!))
            {
                return RedirectToAction("WrongData", "Crushes");
            }
            if (model.RequisitionType != "consumables" && model.RequisitionType != "spareparts")
            {
                return RedirectToAction("WrongData", "Crushes");
            }
            foreach (var item in model.RequisitionItems)
            {
                if (item != null)
                {
                    if (string.IsNullOrWhiteSpace(item.Id) ||
                        !IsValidGuid(item.Id) ||
                        !IsValidDecimal(item.Price.ToString()) ||
                        !IsValidDouble(item.ToOrdered.ToString()) ||
                        string.IsNullOrWhiteSpace(item.Name))
                    {
                        return RedirectToAction("WrongData", "Crushes");
                    }
                }
            }
            bool result = await requisitionService.CreateRequisitionAsync(model, GetUserId()!);
            if (!result)
            {
                return RedirectToAction("NotCreated", "Crushes");
            }
            return RedirectToAction(nameof(Select));
        }

        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            if (!IsValidGuid(id))
            {
                return RedirectToAction("WrongData", "Crushes");
            }
            var model = await requisitionService.GetRequisitionDetailsModelAsync(id);
            if (model == null || string.IsNullOrWhiteSpace(model.RequisitionId))
            {
                return RedirectToAction("NotFound", "Crushes");
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            if (!IsValidGuid(id))
            {
                return RedirectToAction("WrongData", "Crushes");
            }
            var reqToDeleteModel = await requisitionService.GetRequisitionDeleteViewModelAsync(id);
            if (reqToDeleteModel == null || string.IsNullOrWhiteSpace(reqToDeleteModel.RequisitionId))
            {
                return RedirectToAction("NotFound", "Crushes");
            }
            return View(reqToDeleteModel);
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmDelete(string id)
        {
            if (!IsValidGuid(id))
            {
                return RedirectToAction("WrongData", "Crushes");
            }
            bool result = await requisitionService.DeleteRequisitionAsync(id);
            return RedirectToAction(nameof(Select));
        }

        [HttpGet]
        public async Task<IActionResult> Approve(string id)
        {
            if (!IsValidGuid(id))
            {
                return RedirectToAction("WrongData", "Crushes");
            }
            var result = await requisitionService.ApproveRequisition(id);
            if (result == "NullOrApproved")
            {
                return RedirectToAction(nameof(Select));
            }
            else if (result == "Error")
            {
                return RedirectToAction("NotUpdated", "Crushes");
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
