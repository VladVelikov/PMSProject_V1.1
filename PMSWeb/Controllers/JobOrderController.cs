using Google.Cloud.Storage.V1;
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
            if (modelList == null)
            {
                return RedirectToAction("EmptyList", "Crushes");
            }
            return View(modelList);
        }

        [HttpGet]
        public async Task<IActionResult> SelectDueJobs()
        {
            var dueJobsList = await joborderService.GetListOfDueJobsAsync();
            if (dueJobsList == null)
            {
                return RedirectToAction("EmptyList", "Crushes");
            }
            return View(dueJobsList);
        }

        [HttpGet]
        public async Task<IActionResult> SelectHistory()
        {
            var historyJobsList = await joborderService.GetListOfHistoryJobsAsync();
            if (historyJobsList == null)
            {
                return RedirectToAction("EmptyList", "Crushes");
            }
            return View(historyJobsList);
        }

        [HttpGet]
        public async Task<IActionResult> ShowHistory(string id)
        {
            if (!IsValidGuid(id))
            {
                return RedirectToAction("WrongData", "Crushes");
            }
            var model = await joborderService.GetHistoryDetailsAsync(id);
            if (string.IsNullOrEmpty(model.JobId))
            {
                return RedirectToAction("NotFound", "Crushes");
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Create(JobOrderAddMaintenanceViewModel inputModel)
        {
            if (inputModel.TypeId != "Routine" && inputModel.TypeId != "Specific")
            {
                return RedirectToAction("WrongData", "Crushes");
            }
            if (!IsValidGuid(inputModel.EquipmentId.ToString()) ||
                !IsValidGuid(inputModel.MaintenanceId.ToString()) ||
                string.IsNullOrWhiteSpace(inputModel.EquipmentName))
            {
                return RedirectToAction("WrongData", "Crushes");
            }
            var model = await joborderService.GetCreateJobModelAsync(inputModel);
            if (model == null || !IsValidGuid(model.EquipmentId.ToString())
                || string.IsNullOrWhiteSpace(model.EquipmentName))
            {
                return RedirectToAction("WrongData", "Crushes");
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(JobOrderCreateViewModel model)
        {
            if (GetUserId == null)
            {
                return RedirectToAction("WrongData", "Crushes");
            }
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            if (string.IsNullOrEmpty(model.JobName) ||
                !IsValidDate(model.DueDate.ToString()) ||
                !IsValidDate(model.LastDoneDate.ToString()) ||
                !IsValidDouble(model.Interval.ToString()) ||
                string.IsNullOrEmpty(model.Type) ||
                string.IsNullOrWhiteSpace(model.ResponsiblePosition) ||
                !IsValidGuid(model.EquipmentId.ToString()) ||
                !IsValidGuid(model.SpecificMaintenanceId.ToString())
                )
            {
                return RedirectToAction("WrongData", "Crushes");
            }
            bool result = await joborderService.CreateJobOrderAsync(model, GetUserId()!);
            if (!result)
            {
                return RedirectToAction("NotCreated", "Crushes");
            }
            return RedirectToAction(nameof(Select));
        }

        [HttpGet]
        public async Task<IActionResult> AddMaintenanceRM()
        {
            var stringId = (TempData["EquipmentId"] ?? string.Empty).ToString();
            var maintenanceType = (TempData["MaintenanceType"] ?? string.Empty).ToString();
            if (stringId == null || maintenanceType == null || !IsValidGuid(stringId))
            {
                return RedirectToAction(nameof(AddEquipment));
            }
            Guid equipmentId = Guid.Parse(stringId);
            var model = await joborderService.GetAddRoutineMaintenanceViewModelAsync(equipmentId, maintenanceType);
            if (model == null || string.IsNullOrWhiteSpace(model.EquipmentId.ToString()))
            {
                return RedirectToAction("NotFound", "Crushes");
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> AddMaintenanceSM()
        {
            var stringId = (TempData["EquipmentId"] ?? string.Empty).ToString();
            var maintenanceType = (TempData["MaintenanceType"] ?? string.Empty).ToString();
            if (stringId == null || maintenanceType == null || !IsValidGuid(stringId))
            {
                return RedirectToAction(nameof(AddEquipment));
            }
            Guid equipmentId = Guid.Parse(stringId);

            var model = await joborderService.GetAddSpecificMaintenanceViewModelAsync(equipmentId, maintenanceType);
            if (model == null || string.IsNullOrWhiteSpace(model.EquipmentId.ToString()))
            {
                return RedirectToAction("NotFound", "Crushes");
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> AddEquipment()
        {
            var model = await joborderService.GetAddEquipmentModelAsync();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddEquipment(JobOrderAddEquipmentViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("ModelNotValid", "Crushes");
            }
            if (!IsValidGuid(model.EquipmentId.ToString()))
            {
                return RedirectToAction("WrongData", "Crushes");
            }
            if (model.TypeId != "Routine" && model.TypeId != "Specific")
            {
                return RedirectToAction("WrongData", "Crushes");
            }
            TempData["EquipmentId"] = model.EquipmentId;
            TempData["MaintenanceType"] = model.TypeId;
            if (model.TypeId == "Routine")
            {
                return RedirectToAction(nameof(AddMaintenanceRM));
            }
            return RedirectToAction(nameof(AddMaintenanceSM));
        }

        [Authorize(Roles = "Manager")]
        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            if (!IsValidGuid(id.ToString()))
            {
                return RedirectToAction("WrongData", "Crushes");
            }
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
            if (!IsValidGuid(id))
            {
                return RedirectToAction("WrongData", "Crushes");
            }
            var model = await joborderService.GetCompleteJobModelAsync(id);
            if (model == null || string.IsNullOrWhiteSpace(model.JobId))
            {
                return RedirectToAction("NotFound", "Crushes");
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CloseJob(CompleteTheJobViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Details))
            {
                return RedirectToAction("CompleteJob", new { id = model.JobId });
            }
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
            if (!IsValidGuid(id))
            {
                return RedirectToAction("WrongData", "Crushes");
            }
            var model = await joborderService.GetSparesPartialModelAsync(id);
            if (model == null || string.IsNullOrWhiteSpace(model.JobId))
            {
                return RedirectToAction("NotFound", "Crushes");
            }

            return PartialView("_SparesUsedPartial", model);
        }

        [HttpGet]
        public async Task<IActionResult> ConsumablesUsedPartial(string id)
        {
            if (!IsValidGuid(id))
            {
                return RedirectToAction("WrongData", "Crushes");
            }
            var model = await joborderService.GetConsumablesPartialModelAsync(id);
            if (model == null || string.IsNullOrWhiteSpace(model.JobId))
            {
                return RedirectToAction("NotFound", "Crushes");
            }
            return PartialView("_ConsumablesUsedPartial", model);
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmSparesUsed(PartialViewModel model)
        {
            if (!IsValidGuid(model.JobId) || !IsValidGuid(model.EquipmentId))
            {
                return RedirectToAction("WrongData", "Crushes");
            }
            foreach (var item in model.InventoryList)
            {
                if (item == null || item.Id == null || !IsValidGuid(item.Id))
                {
                    return RedirectToAction("WrongData", "Crushes");
                }
            }
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
            if (!IsValidGuid(model.JobId) || !IsValidGuid(model.EquipmentId))
            {
                return RedirectToAction("WrongData", "Crushes");
            }
            foreach (var item in model.InventoryList)
            {
                if (item == null || item.Id == null || !IsValidGuid(item.Id))
                {
                    return RedirectToAction("WrongData", "Crushes");
                }
            }
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
            if (!IsValidGuid(id))
            {
                return RedirectToAction("WrongData", "Crushes");
            }
            var model = await joborderService.GetSelectManualViewModelAsync(id);
            if (model == null)
            {
                return RedirectToAction("NotFound", "Crushes");
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> OpenManual(string jobid, string manualid)   // this method to be recreated as MicroService later
        {
            if (!IsValidGuid(jobid) || !IsValidGuid(manualid))
            {
                return RedirectToAction("WrongData", "Crushes");
            }
            var model = await joborderService.GetOpenManualViewModelAsync(jobid, manualid);
            if (model == null)
            {
                return RedirectToAction("NotFound", "Crushes");
            }
            try
            {
                var storageClient = StorageClient.Create();
                string _bucketName = "pmsweb_my_bucketstorage";
                using (var memoryStream = new MemoryStream())
                {
                    await storageClient.DownloadObjectAsync(
                        bucket: _bucketName,
                        objectName: model.URL,
                        destination: memoryStream
                    );
                    memoryStream.Position = 0;

                    string base64Content = Convert.ToBase64String(memoryStream.ToArray());
                    string contentType = GetContentType(model.URL ?? "MissingFile.pdf");

                    model.FileContent = base64Content;
                    model.ContentType = contentType;
                    ViewBag.FileName = model.URL;

                    return View(model);
                }
            }
            catch (Google.GoogleApiException ex) when (ex.Error.Code == 404)
            {
                return NotFound($"File '{model.URL}' not found in storage.");
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }

            //return View(model);
        }
        
        private string GetContentType(string fileName)
        {
            var extension = Path.GetExtension(fileName).ToLowerInvariant();
            return extension switch
            {
                ".pdf" => "application/pdf",
                ".jpg" => "image/jpeg",
                ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".gif" => "image/gif",
                ".txt" => "text/plain",
                _ => "application/octet-stream"
            };
        }
    }
}
