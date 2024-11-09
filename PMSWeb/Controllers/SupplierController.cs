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
            var spareparts = await context
                .Spareparts
                .AsNoTracking()
                .Where(x=>!x.IsDeleted)
                .Select(x => new PairGuidViewModel()
                {
                    Name = x.SparepartName,
                    Id = x.SparepartId
                })
                .ToListAsync();
            var consumables = await context
                .Consumables
                .AsNoTracking()
                .Where(x=>!x.IsDeleted)
                .Select(x => new PairGuidViewModel()
                {
                    Name = x.Name,
                    Id = x.ConsumableId
                })
                .ToListAsync();
            if (countries != null) model.Countries = countries;
            if (cities != null) model.Cities = cities;
            if (spareparts != null) model.Spareparts = spareparts;
            if (consumables != null) model.Consumables = consumables;


            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(SupplierCreateViewModel model,
            List<Guid> Spareparts,
            List<Guid> Consumables)
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

            foreach (var sparepartId in Spareparts)
            {
                bool alreadyAdded = await context.SparepartsSuppliers
                     .AnyAsync(x => x.SparepartId == sparepartId &&
                                  x.SupplierId == supplier.SupplierId);
                if (!alreadyAdded)
                {
                    SparepartSupplier sparesup = new()
                    {
                        SparepartId = sparepartId,
                        SupplierId = supplier.SupplierId
                    };
                    await context.SparepartsSuppliers.AddAsync(sparesup);
                }
            }

            foreach (var consumableId in Consumables)
            {
                bool alreadyAdded = await context.ConsumablesSuppliers
                    .AnyAsync(x => x.ConsumableId == consumableId &&
                                 x.SupplierId == supplier.SupplierId);
                if (!alreadyAdded)
                {
                    ConsumableSupplier consSup = new()
                    {
                        ConsumableId = consumableId,
                        SupplierId = supplier.SupplierId
                    };
                    await context.ConsumablesSuppliers.AddAsync(consSup);   
                }
            }

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

            var spareparts = await context
               .SparepartsSuppliers
               .AsNoTracking()
               .Where(x => x.SupplierId.ToString().ToLower() == id.ToLower())
               .Select(x => new PairGuidViewModel()
               {
                   Name = x.Sparepart.SparepartName,
                   Id = x.SparepartId
               })
               .ToListAsync();

            var consumables = await context
                .ConsumablesSuppliers
                .AsNoTracking()
                .Where(x => x.SupplierId.ToString().ToLower() == id.ToLower())
                .Select(x => new PairGuidViewModel()
                {
                    Name = x.Consumable.Name,
                    Id = x.ConsumableId
                })
                .ToListAsync();

            var availableSpareparts = await context
               .Spareparts
               .AsNoTracking()
               .Where(x=>!x.IsDeleted)
               .Select(x => new PairGuidViewModel()
               {
                   Name = x.SparepartName,
                   Id = x.SparepartId
               })
               .ToListAsync();
            foreach (var spare in spareparts)  // removing already selected spares from the list of all spares, to leave the available spares
            {
                var spareToRemove = availableSpareparts.FirstOrDefault(x=>x.Id == spare.Id);
                if (spareToRemove != null)
                {
                    availableSpareparts.Remove(spareToRemove);
                }
            }

            var availableConsumables = await context
                .Consumables
                .AsNoTracking()
                .Where(x=>!x.IsDeleted)
                .Select(x => new PairGuidViewModel()
                {
                    Name = x.Name,
                    Id = x.ConsumableId
                })
                .ToListAsync();
            foreach (var consum in consumables)  // removing already selected spares from the list of all spares, to leave the available spares
            {
                var consumableToRemove = availableConsumables.FirstOrDefault(x => x.Id == consum.Id);
                if (consumableToRemove != null)
                {
                    availableConsumables.Remove(consumableToRemove);
                }
            }

            if (countries != null) model.Countries = countries;
            if (cities != null) model.Cities = cities;
            if (consumables != null) model.Consumables = consumables;
            if (spareparts != null) model.Spareparts = spareparts;
            if (availableConsumables != null) model.AvailableConsumables = availableConsumables;
            if (availableSpareparts != null) model.AvailableSpareparts = availableSpareparts;

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(SupplierEditViewModel model,
            List<Guid> Spareparts,
            List<Guid> Consumables,
            List<Guid> AvailableSpareparts,
            List<Guid> AvailableConsumables)
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
            /// Apply Changes To Model
            supplierEdit.Name = model.Name;
            supplierEdit.Address = model.Address;
            supplierEdit.Email = model.Email;
            supplierEdit.PhoneNumber = model.PhoneNumber;
            supplierEdit.CityId = Guid.Parse(model.CityId);
            supplierEdit.CountryId = Guid.Parse(model.CountryId);
            supplierEdit.CreatorId = GetUserId()!;
            supplierEdit.CreatedOn = DateTime.UtcNow;
            supplierEdit.EditedOn = DateTime.UtcNow;

            /// Related Spareparts Changes Apply
            var sparesSuppliers = await context
                .SparepartsSuppliers
                .Where(x => x.SupplierId.ToString().ToLower() == model.SupplierId.ToLower())
                .ToListAsync();

            foreach (var item in sparesSuppliers)   /// remove unselected spares
            {
                if (!Spareparts.Contains(item.SparepartId))
                {
                    context.SparepartsSuppliers.Remove(item);
                }
            }

            foreach (var spareId in AvailableSpareparts)
            {
                bool alreadyAdded = await context.SparepartsSuppliers
                     .AnyAsync(x => x.SparepartId == spareId &&
                                  x.SupplierId == Guid.Parse(model.SupplierId));
                if (!alreadyAdded)
                {
                    SparepartSupplier spareSup = new()
                    {
                        SparepartId = spareId,
                        SupplierId = Guid.Parse(model.SupplierId)
                    };
                    await context.SparepartsSuppliers.AddAsync(spareSup);
                }
            }

            /// Related Consumables Changes Apply
            var consumSuppliers = await context
                .ConsumablesSuppliers
                .Where(x => x.SupplierId.ToString().ToLower() == model.SupplierId.ToLower())
                .ToListAsync();

            foreach (var item in consumSuppliers)   /// remove unselected spares
            {
                if (!Consumables.Contains(item.ConsumableId))
                {
                    context.ConsumablesSuppliers.Remove(item);
                }
            }

            foreach (var consumId in AvailableConsumables)
            {
                bool alreadyAdded = await context.ConsumablesSuppliers
                     .AnyAsync(x => x.ConsumableId == consumId &&
                                  x.SupplierId == Guid.Parse(model.SupplierId));
                if (!alreadyAdded)
                {
                    ConsumableSupplier consSup = new()
                    {
                        ConsumableId = consumId,
                        SupplierId = Guid.Parse(model.SupplierId)
                    };
                    await context.ConsumablesSuppliers.AddAsync(consSup);
                }
            }


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
            var model = await context
                .Suppliers
                .AsNoTracking()
                .Where(x=>!x.IsDleted)
                .Where(x=>x.SupplierId.ToString().ToLower() == id.ToLower())
                .Select(x=> new SupplierDetailsViewModel() {
                    SupplierId = x.SupplierId.ToString(),
                    Name = x.Name,
                    Address = x.Address,
                    Email = x.Email,
                    PhoneNumber = x.PhoneNumber,
                    City = x.City.Name,
                    Country = x.Country.Name,   
                    Creator = x.Creator.UserName,
                    CreatedOn = x.CreatedOn.ToString(PMSRequiredDateFormat),
                })
                .FirstOrDefaultAsync();
            if (model == null || model.SupplierId == null)
            {
                  return RedirectToAction(nameof(Select)); 
            }
            var consumables = await context
                .ConsumablesSuppliers
                .AsNoTracking()
                .Where(x=>x.SupplierId.ToString().ToLower() == model.SupplierId!.ToLower())
                .Select(x=>x.Consumable.Name)
                .ToListAsync();
            var spareparts = await context
                .SparepartsSuppliers
                .AsNoTracking()
                .Where(x => x.SupplierId.ToString().ToLower() == model.SupplierId!.ToLower())
                .Select(x => x.Sparepart.SparepartName)
                .ToListAsync();
            if (consumables != null) { model.Consumables = consumables; }
            if (spareparts != null) { model.Spareparts = spareparts; }

            return View(model);
        }

        private string? GetUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier);
        }
    }
}
