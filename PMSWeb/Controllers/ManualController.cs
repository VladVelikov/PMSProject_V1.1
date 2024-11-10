using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PMS.Data;
using PMS.Services.Data.Interfaces;
using PMSWeb.ViewModels.Manual;
using System.Security.Claims;

namespace PMSWeb.Controllers
{
    [Authorize]
    public class ManualController(IManualService manualService) : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Select()
        {
            var manuals = await manualService.GetListOfViewModelsAsync();
            return View(manuals);
        }

        [HttpGet]
        public async Task<IActionResult> Create(string URL)
        {
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

            string? userId = GetUserId();
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction(nameof(Select));    
            }

            bool isCreated = await manualService.CreateManualAsync(model, userId);
            return RedirectToAction(nameof(Select));
        }

        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            var model = await manualService.GetDetailsAsync(id);
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
            if (myFile != null && myFile.Length > 0)
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
            else
            {
                return RedirectToAction(nameof(Upload));
            }
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            var model = await manualService.GetItemToDeleteAsync(id);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(ManualDeleteViewModel model)
        {
            bool isDeleted = await manualService.ConfirmDeleteAsync(model);
            return RedirectToAction(nameof(Select));
        }

        public string? GetUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier)!.ToString();
        }

    }
}
