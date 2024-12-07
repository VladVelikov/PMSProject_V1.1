using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PMS.Services.Data.Interfaces;
using PMSWeb.ViewModels.CountryVM;

namespace PMSWeb.Controllers
{
    [Authorize]
    public class CountryController(ICountryService countryService) : BasicController
    {
        [HttpGet]
        public async Task<IActionResult> Select()
        {
            var modelList = await countryService.GetListOfCountriesAsync();
            if (modelList.Count() == 0)
            {
                return RedirectToAction("EmptyList", "Crushes");
            }
            return View(modelList);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new CountryCreateViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CountryCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model); 
            }
            if (string.IsNullOrEmpty(model.Name))
            {
                return View(model);
            }
            bool isCreated = await countryService.CreateCountryAsync(model);
            return RedirectToAction(nameof(Select));
        }
        [Authorize(Roles = "Manager")]
        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            if (!IsValidGuid(id))
            {
                return RedirectToAction("WrongData", "Crushes");
            }

            var model = await countryService.GetDeleteCountryModelAsync(id);
            if (model == null || string.IsNullOrEmpty(model.CountryId))
            {
                return RedirectToAction("NotFound", "Crushes");
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(CountryDeleteViewModel model)
        {
            if (!ModelState.IsValid || model == null || model.CountryId == null)
            {
                return RedirectToAction(nameof(Select));
            }
            bool isDeleted = await countryService.DeleteCountryModelAsync(model);
            if (!isDeleted)
            {
                return RedirectToAction("NotDeleted", "Crushes");
            }
            return RedirectToAction(nameof(Select));
        }
    }
}