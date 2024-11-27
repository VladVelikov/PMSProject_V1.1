using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PMS.Services.Data.Interfaces;
using PMSWeb.ViewModels.JobOrderVM;
using System.Security.Claims;

namespace PMSWeb.Controllers
{
    [Authorize]
    public class JobOrderController(IJoborderService joborderService) : BasicController
    {
        [HttpGet]
        public async Task<IActionResult> Select()
        {
            var modelList = await joborderService.GetListOfAllJobsAsync();
            return View(modelList);
        }
        
        [HttpGet]
        public async Task<IActionResult> SelectDueJobs()
        {
            var dueJobsList = await joborderService.GetListOfDueJobsAsync();
            return View(dueJobsList);
        }
        
        [HttpGet]
        public async Task<IActionResult> SelectHistory()
        {
            var historyJobsList = await joborderService.GetListOfHistoryJobsAsync();
            return View(historyJobsList);
        }
        
        [HttpGet]
        public async Task<IActionResult> ShowHistory(string id)
        {
            var model = await joborderService.GetHistoryDetailsAsync(id);
            return View(model);
        }
        
        [HttpGet]
        public async Task<IActionResult> Create(JobOrderAddMaintenanceViewModel inputModel)
        {
            var model = await joborderService.GetCreateJobModelAsync(inputModel);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(JobOrderCreateViewModel model)
        {
            bool result = await joborderService.CreateJobOrderAsync(model, GetUserId());   
            return RedirectToAction(nameof(Select));
        }

        [HttpGet]
        public async Task<IActionResult> AddMaintenanceRM()
        {
            var stringId = TempData["EquipmentId"].ToString();
            var maintenanceType = TempData["MaintenanceType"].ToString();
            Guid equipmentId = Guid.Parse(stringId);

            var model = await joborderService.GetAddRoutineMaintenanceViewModelAsync(equipmentId, maintenanceType);
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> AddMaintenanceSM()
        {
            var stringId = TempData["EquipmentId"].ToString();
            var maintenanceType = TempData["MaintenanceType"].ToString();
            Guid equipmentId = Guid.Parse(stringId);

            var model = await joborderService.GetAddSpecificMaintenanceViewModelAsync(equipmentId, maintenanceType);
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> AddEquipment()
        {
            var model = await joborderService.GetAddEquipmentModelAsync();
            return View(model);
        }

        [HttpPost]
        public IActionResult AddEquipment(JobOrderAddEquipmentViewModel model)
        {
            TempData["EquipmentId"] = model.EquipmentId;
            TempData["MaintenanceType"] = model.TypeId;
            if (model.TypeId == "Routine")
            { 
                return RedirectToAction(nameof(AddMaintenanceRM));
            }
            return RedirectToAction(nameof(AddMaintenanceSM));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            bool result = await joborderService.DeleteJobOrderAsync(id);
            if (!result)
            {
                return RedirectToAction("NotDeleted", "Crushes");
            }
            return RedirectToAction(nameof(Select));
        }

        [HttpGet]
        public async Task<IActionResult> CompleteJob(string id)
        {
            if (id == null)
            {
                return RedirectToAction("ModelNotValid","Crushes");
            }

            var model = await joborderService.GetCompleteJobModelAsync(id);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CloseJob(CompleteTheJobViewModel model)
        {
            var userName = User.FindFirstValue(ClaimTypes.Name);
            if (userName == null)
            {
                return RedirectToAction("NotFound", "Crushes");
            }
            bool result = await joborderService.CloseThisJob(model, userName);

            if (!result) 
            {
                return RedirectToAction("NotCreated", "Crushes");
            }

            return RedirectToAction(nameof(SelectHistory));
        }

        [HttpGet]
        public async Task<IActionResult> SparesUsedPartial(string id)
        {
            var model = await joborderService.GetSparesPartialModelAsync(id); 
            return PartialView("_SparesUsedPartial", model);
        }

        [HttpGet]
        public async Task<IActionResult> ConsumablesUsedPartial(string id)
        {
           
            var model = await joborderService.GetConsumablesPartialModelAsync(id);  
            return PartialView("_ConsumablesUsedPartial", model);
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmSparesUsed(PartialViewModel model)
        {
            bool result = await joborderService.ConfirmSparesAreUsedAsync(model);
            if (!result)
            {
                return RedirectToAction("NotUpdated", "Crushes");
            }
            return RedirectToAction("CompleteJob", new { id = model.JobId });
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmConsumablesUsed(PartialViewModel model)
        {
            bool result = await joborderService.ConfirmConsumablesAreUsedAsync(model);
            if (!result)
            {
                return RedirectToAction("NotUpdated", "Crushes");
            }
            return RedirectToAction("CompleteJob", new { id = model.JobId });
        }

        [HttpGet]
        public async Task<IActionResult> Manuals(string id)
        {
            var model = await joborderService.GetSelectManualViewModelAsync(id);  
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> OpenManual(string jobid, string manualid)
        {
            var model = await joborderService.GetOpenManualViewModelAsync(jobid, manualid);   
            return View(model);
        }

    }
}
