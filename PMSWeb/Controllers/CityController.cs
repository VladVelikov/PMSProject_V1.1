using Microsoft.AspNetCore.Mvc;
using PMS.Services.Data.Interfaces;
using PMSWeb.ViewModels.CityVM;

namespace PMSWeb.Controllers
{
    public class CityController(ICityService cityService) : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Select()
        {
            return View(await cityService.GetListOfCitiesAsync());
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new CityCreateViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(CityCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("NotCreated", "Crushes");
            }

            bool isCreated = await cityService.CreateCityAsync(model);
            if (!isCreated)
            {
                return RedirectToAction("NotCreated","Crushes");   
            }

            return RedirectToAction(nameof(Select));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            var model = await cityService.GetDeleteCityModelAsync(id);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(CityDeleteViewModel model)
        {
            if (!ModelState.IsValid || model == null || model.CityId == null)
            {
                return RedirectToAction("ModelNotFound", "Crushes");
            }
            
            bool isDeleted = await cityService.DeleteCityModelAsync(model);
            if (!isDeleted)
            {
                return RedirectToAction("NotDeleted", "Crushes");
            }
            return RedirectToAction(nameof(Select));
        }

    }
}
