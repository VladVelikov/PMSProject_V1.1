using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PMS.Data;
using PMS.Data.Models;
using PMSWeb.ViewModels.CommonVM;
using PMSWeb.ViewModels.SupplierVM;
using System.Security.Claims;
using static PMS.Common.EntityValidationConstants;

namespace PMSWeb.Controllers
{
    [Authorize]
    public class SupplierController(PMSDbContext context) : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Select()
        {
            var modelList = await context
                .Suppliers
                .Where(x => !x.IsDleted)
                .OrderByDescending(x=>x.EditedOn)
                .AsNoTracking()
                .Select(x=> new SupplierDisplayViewModel() {
                    Name = x.Name,
                    Address = x.Address,
                    Email = x.Email,
                    PhoneNumber = x.PhoneNumber,
                    City = x.City.Name,
                    Country = x.Country.Name,
                    SupplierId = x.SupplierId.ToString()
                })
                .ToListAsync();
            return View(modelList);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var model = new SupplierCreateViewModel();
            var countries = await context
                .Countries
                .AsNoTracking()
                .Select(x => new PairViewModel()
                {
                    Name = x.Name,
                    Id = x.CountryId.ToString()
                })
                .ToListAsync();
            var cities = await context
                .Cities
                .AsNoTracking()
                .Select(x=> new PairViewModel() {
                    Name = x.Name,
                    Id = x.CityId.ToString()
                })
                .ToListAsync();
            if (countries != null) model.Countries = countries;
            if (cities != null) model.Cities = cities;

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(SupplierCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            Supplier supplier = new()
            {
                Name = model.Name,
                Address = model.Address,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                CityId = Guid.Parse(model.CityId),
                CountryId = Guid.Parse(model.CountryId),
                CreatorId = GetUserId()!,
                CreatedOn = DateTime.UtcNow,
                EditedOn = DateTime.UtcNow,
                IsDleted = false
            };
            await context.Suppliers.AddAsync(supplier);    
            await context.SaveChangesAsync();

            return RedirectToAction(nameof(Select));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var model = await context
                .Suppliers
                .AsNoTracking()
                .Where(x => !x.IsDleted)
                .Where(x => x.SupplierId.ToString().ToLower() == id.ToLower())
                .Select(x=> new SupplierEditViewModel() {
                    SupplierId = x.SupplierId.ToString(),
                    Name = x.Name,
                    Address = x.Address,
                    Email = x.Email,
                    PhoneNumber = x.PhoneNumber,
                    CityId = x.CityId.ToString(),
                    CountryId = x.CountryId.ToString()
                })
                .FirstOrDefaultAsync();
            if (model == null)
            {
                return RedirectToAction(nameof(Select));
            }

            var countries = await context
                .Countries
                .AsNoTracking()
                .Select(x => new PairViewModel()
                {
                    Name = x.Name,
                    Id = x.CountryId.ToString()
                })
                .ToListAsync();
            var cities = await context
                .Cities
                .AsNoTracking()
                .Select(x => new PairViewModel()
                {
                    Name = x.Name,
                    Id = x.CityId.ToString()
                })
                .ToListAsync();
            if (countries != null) model.Countries = countries;
            if (cities != null) model.Cities = cities;

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(SupplierEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
               return RedirectToAction(nameof(Select));
            }
            if (model == null || model.SupplierId == null || GetUserId() == null)
            {
                return RedirectToAction(nameof(Select));
            }
            var supplierEdit = await context
                 .Suppliers
                 .Where(x => !x.IsDleted)
                 .Where(x => x.SupplierId.ToString().ToLower() == model.SupplierId.ToLower())
                 .FirstOrDefaultAsync();
            if (supplierEdit == null)
            {
                return RedirectToAction(nameof(Select));
            }
            supplierEdit.Name = model.Name;
            supplierEdit.Address = model.Address;
            supplierEdit.Email = model.Email;
            supplierEdit.PhoneNumber = model.PhoneNumber;
            supplierEdit.CityId = Guid.Parse(model.CityId);
            supplierEdit.CountryId = Guid.Parse(model.CountryId);
            supplierEdit.CreatorId = GetUserId()!;
            supplierEdit.CreatedOn = DateTime.UtcNow;
            supplierEdit.EditedOn = DateTime.UtcNow;

            await context.SaveChangesAsync(); 

            return RedirectToAction(nameof(Select));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            var model = await context
                .Suppliers
                .AsNoTracking() 
                .Where(x=>!x.IsDleted)
                .Where(x=>x.SupplierId.ToString().ToLower() == id.ToLower())
                .Select(x=> new SupplierDeleteViewModel() {
                    SupplierId = x.SupplierId.ToString(),
                    Name = x.Name,
                    Address = x.Address,
                    CreatedOn = x.CreatedOn.ToString(PMSRequiredDateFormat)
                })
                .FirstOrDefaultAsync(); 
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(SupplierDeleteViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction(nameof(Select));
            }
            var modelDelete = await context
               .Suppliers
               .Where(x => !x.IsDleted)
               .Where(x => x.SupplierId.ToString().ToLower() == model.SupplierId.ToLower())
               .FirstOrDefaultAsync();
            if (modelDelete != null)
            {
                modelDelete.IsDleted = true;    
                await context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Select));
        }

        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            return View();
        }


        private string? GetUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier);
        }
    }
}
