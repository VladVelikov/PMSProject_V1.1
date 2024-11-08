using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PMS.Data;
using PMS.Data.Models;
using PMSWeb.ViewModels.CityVM;
using static PMS.Common.EntityValidationConstants;

namespace PMSWeb.Controllers
{
    public class CityController(PMSDbContext context) : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Select()
        {
            var modelList = await context
                .Cities
                .AsNoTracking()
                .OrderByDescending(x=>x.CreatedOn)
                .Select(x => new CityDisplayViewModel() {
                    Name = x.Name,
                    CreatedOn = x.CreatedOn.ToString(PMSRequiredDateFormat),
                    CityId = x.CityId.ToString()
                })
                .ToListAsync();
            return View(modelList);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View(new CityCreateViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(CityCreateViewModel model)
        {
            City city = new City() {
                Name = model.Name,
                CreatedOn = DateTime.Now,
                EditedOn = DateTime.Now
            };
            await context.Cities.AddAsync(city);
            await context.SaveChangesAsync();

            return RedirectToAction(nameof(Select));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            var model = await context
                .Cities
                .AsNoTracking()
                .Where(x => x.CityId.ToString().ToLower() == id.ToLower())
                .Select(x => new CityDeleteViewModel() {
                    CityId = x.CityId.ToString(),
                    Name = x.Name,
                    CreatedOn = x.CreatedOn.ToString(PMSRequiredDateTimeFormat)
                })
                .FirstOrDefaultAsync();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(CityDeleteViewModel model)
        {
            if (!ModelState.IsValid || model == null || model.CityId == null)
            {
                return RedirectToAction(nameof(Select));
            }
            var modelDel = await context
                .Cities
                .Where(x => x.CityId.ToString().ToLower() == model.CityId.ToLower())
                .FirstOrDefaultAsync();

            if (modelDel != null)
            {
                context.Cities.Remove(modelDel);
                await context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Select));
        }


    }
}
