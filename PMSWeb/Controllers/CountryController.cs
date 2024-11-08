using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PMS.Data;
using PMS.Data.Models;
using PMSWeb.ViewModels.CountryVM;
using static PMS.Common.EntityValidationConstants;

namespace PMSWeb.Controllers
{
    public class CountryController(PMSDbContext context) : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Select()
        {
            var modelList = await context
                .Countries
                .AsNoTracking()
                .OrderByDescending(x => x.CreatedOn)
                .Select(x => new CountryDisplayViewModel()
                {
                    Name = x.Name,
                    CreatedOn = x.CreatedOn.ToString(PMSRequiredDateFormat),
                    CountryId = x.CountryId.ToString()
                })
                .ToListAsync();
            return View(modelList);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View(new CountryCreateViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(CountryCreateViewModel model)
        {
            Country country = new Country()
            {
                Name = model.Name,
                CreatedOn = DateTime.Now,
                EditedOn = DateTime.Now
            };
            await context.Countries.AddAsync(country);
            await context.SaveChangesAsync();

            return RedirectToAction(nameof(Select));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            var model = await context
                .Countries
                .AsNoTracking()
                .Where(x => x.CountryId.ToString().ToLower() == id.ToLower())
                .Select(x => new CountryDeleteViewModel()
                {
                    CountryId = x.CountryId.ToString(),
                    Name = x.Name,
                    CreatedOn = x.CreatedOn.ToString(PMSRequiredDateTimeFormat)
                })
                .FirstOrDefaultAsync();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(CountryDeleteViewModel model)
        {
            if (!ModelState.IsValid || model == null || model.CountryId == null)
            {
                return RedirectToAction(nameof(Select));
            }
            var modelDel = await context
                .Countries
                .Where(x => x.CountryId.ToString().ToLower() == model.CountryId.ToLower())
                .FirstOrDefaultAsync();

            if (modelDel != null)
            {
                context.Countries.Remove(modelDel);
                await context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Select));
        }


    }
}