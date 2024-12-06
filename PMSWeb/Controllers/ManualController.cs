using Google.Cloud.Storage.V1;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PMS.Data;
using PMS.Services.Data.Interfaces;
using PMSWeb.ViewModels.Manual;

namespace PMSWeb.Controllers
{
    [Authorize]
    public class ManualController(PMSDbContext context, IManualService manualService) : BasicController
    {
        private readonly string _bucketName = "pmsweb_my_bucketstorage";

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

            if (string.IsNullOrEmpty(GetUserId()) || !IsValidGuid(GetUserId()!))
            {
                return RedirectToAction("WrongData", "Crushes");
            }
            bool isCreated = await manualService.CreateManualAsync(model, GetUserId()!);

            return RedirectToAction(nameof(Select));
        }

        [HttpGet]
        public IActionResult Upload()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile myFile)
        {
            var manualsAlreadyUploadedFiles = await context
                .Manuals
                .AsNoTracking()
                .Where(x => !x.IsDeleted)
                .Select(x => x.ContentURL)
                .ToListAsync();

            if (myFile == null || myFile.Length <= 0)
            {
                ViewBag.Error = "Please upload a valid file.";
                return View();
            }
            const long MaxFileSize = 5 * 1024 * 1024; // 5 MB Limit applied
            if (myFile.Length > MaxFileSize)
            {
                ViewBag.Error = "File size cannot exceed 5 MB.";
                return View();
            }

            var allowedExtensions = new[] { ".pdf" }; // Add allowed extensions
            var fileExtension = Path.GetExtension(myFile.FileName);
            if (!allowedExtensions.Contains(fileExtension.ToLower()))
            {
                ViewBag.Error = "Unsupported file type. Please upload a *.PDF.";
                return View();
            }

            if (manualsAlreadyUploadedFiles.Contains(myFile.FileName))
            {
                ViewBag.Error = "File with this name already exist. Please do minor change in file name and upload again.";
                return View();
            }

            try
            {
                var storageClient = StorageClient.Create();

                using (var memoryStream = new MemoryStream())
                {
                    await myFile.CopyToAsync(memoryStream);
                    memoryStream.Position = 0;

                    var objectName = myFile.FileName;

                    var uploadResult = await storageClient.UploadObjectAsync(
                        bucket: _bucketName,
                        objectName: objectName,
                        contentType: myFile.ContentType,
                        source: memoryStream
                    );
                    return RedirectToAction("Create", new { URL = uploadResult.Name });
                }
            }
            catch
            {
                ModelState.AddModelError("myFile", $"File upload failed");
                return RedirectToAction(nameof(Upload));
            }
        }

        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            var model = await manualService.GetDetailsAsync(id);
            if (model == null || string.IsNullOrWhiteSpace(model.Name))
            {
                return RedirectToAction("NotFound", "Crushes");
            }
            string fileName = model.URL ?? "FileMissing.pdf";
            try
            {
                // Initialize Google Cloud Storage client
                var storageClient = StorageClient.Create();

                // MemoryStream to hold the downloaded file
                using (var memoryStream = new MemoryStream())
                {
                    // Download the file from the bucket into the memory stream
                    await storageClient.DownloadObjectAsync(
                        bucket: _bucketName,
                        objectName: fileName,
                        destination: memoryStream
                    );

                    // Reset the stream position to the beginning
                    memoryStream.Position = 0;

                    // Get file content as base64 string (for inline rendering in view)
                    string base64Content = Convert.ToBase64String(memoryStream.ToArray());
                    string contentType = GetContentType(fileName);

                    // Pass the base64 content and content type to the view
                    ViewBag.FileContent = base64Content;
                    ViewBag.ContentType = contentType;
                    ViewBag.FileName = fileName;

                    return View(model);
                }
            }
            catch (Google.GoogleApiException ex) when (ex.Error.Code == 404)
            {
                return NotFound($"File '{fileName}' not found in our storage.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
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

        [Authorize(Roles = "Manager")]
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
