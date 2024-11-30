using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PMS.Services.Data.Interfaces;
using PMSWeb.ViewModels.Manual;

namespace PMSWeb.Controllers
{
    [Authorize]
    public class ManualController(IManualService manualService) : BasicController
    {
        /// <summary>
        /// TO DO Rewrite this controller with applied ~FTP or ~Cloud storage.
        /// </summary>
        /// <returns></returns>

        [HttpGet]
        public async Task<IActionResult> Select()
        {
            var manuals = await manualService.GetListOfViewModelsAsync();
            if (manuals == null)
            {
                return RedirectToAction("NotFound", "Crushes");
            }
            return View(manuals);
        }

        [HttpGet]
        public async Task<IActionResult> Create(string URL)
        {
            if (!string.IsNullOrWhiteSpace(URL))
            {
                if (!IsSafeUrl(URL))
                {
                    return RedirectToAction("WrongData", "Crushes");
                }
            }
            var model = await manualService.GetCreateViewModelAsync(URL);

            return View(model);  
        }

        [HttpPost]
        public async Task<IActionResult> Create(ManualCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            
            if (string.IsNullOrWhiteSpace(model.ManualName) ||
                !IsValidGuid(model.MakerId) ||
                !IsValidGuid(model.EquipmentId))
            {
                return RedirectToAction("WrongData", "Crushes");
            }

            if (!string.IsNullOrWhiteSpace(model.ContentURL))  // in this software we'll accept empty URL field as well.
            {
                if (!IsSafeUrl(model.ContentURL))
                {
                    return RedirectToAction("WrongData", "Crushes");
                }
            }
            
            if (string.IsNullOrEmpty(GetUserId()) || !IsValidGuid(GetUserId()!))
            {
                return RedirectToAction("WrongData", "Crushes");  
            }
            bool isCreated = await manualService.CreateManualAsync(model, GetUserId()!);

            return RedirectToAction(nameof(Select));
        }

        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            if (!IsValidGuid(id))
            {
                return RedirectToAction("WrongData", "Crushes");
            }
            var model = await manualService.GetDetailsAsync(id);
            if (model == null || string.IsNullOrWhiteSpace(model.Name))
            {
                return RedirectToAction("NotFound", "Crushes");
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Upload()
        {
            return View();
        }
       
        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile myFile)
        {
            if (myFile == null || myFile.Length <= 0)
            {
                ModelState.AddModelError("myFile", "Please upload a valid file.");
                return RedirectToAction(nameof(Upload));
            }
            const long MaxFileSize = 5 * 1024 * 1024; // 5 MB Limit applied
            if (myFile.Length > MaxFileSize)
            {
                ModelState.AddModelError("myFile", "File size cannot exceed 5 MB.");
                return RedirectToAction(nameof(Upload));
            }

            var allowedExtensions = new[] { ".pdf" }; // Add allowed extensions
            var fileExtension = Path.GetExtension(myFile.FileName);
            if (!allowedExtensions.Contains(fileExtension.ToLower()))
            {
                ModelState.AddModelError("myFile", "Unsupported file type. Please upload a *.PDF.");
                return RedirectToAction(nameof(Upload));
            }

            try
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "UploadedFiles", myFile.FileName);
                Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await myFile.CopyToAsync(stream);
                }

                string shortPath = $"\\UploadedFiles\\{myFile.FileName}";
                return RedirectToAction("Create", new { URL = shortPath });
            }
            catch
            {
                ModelState.AddModelError("myFile", $"File upload failed");
                return RedirectToAction(nameof(Upload));
            }
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            if (!IsValidGuid(id))
            {
                return RedirectToAction("WrongData", "Crushes");
            }
            var model = await manualService.GetItemToDeleteAsync(id);
            if (model == null || string.IsNullOrWhiteSpace(model.ManualId))
            {
                return RedirectToAction("NotFound", "Crushes");
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(ManualDeleteViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            if (!IsValidGuid(model.ManualId!))
            {
                return RedirectToAction("NotFound", "Crushes");
            }
            bool isDeleted = await manualService.ConfirmDeleteAsync(model);
            if (!isDeleted)
            {
                return RedirectToAction("NotDeleted", "Crushes");
            }
            return RedirectToAction(nameof(Select));
        }
    }
}
