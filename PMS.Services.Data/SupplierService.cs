using Microsoft.EntityFrameworkCore;
using PMS.Data.Models;
using PMS.Data.Repository.Interfaces;
using PMS.Services.Data.Interfaces;
using PMSWeb.ViewModels.CommonVM;
using PMSWeb.ViewModels.SupplierVM;
using static PMS.Common.EntityValidationConstants;

namespace PMS.Services.Data
{
    public class SupplierService(IRepository<Supplier, Guid> suppliersRepo,
                                 IRepository<Country,Guid> countriesRepo,
                                 IRepository<City,Guid> citiesRepo,
                                 IRepository<Sparepart, Guid> sparesRepo,
                                 IRepository<Consumable, Guid> consumablesRepo,
                                 IRepository<SparepartSupplier, Guid[]> sparePartsSuppliersRepo,
                                 IRepository<ConsumableSupplier, Guid[]> consumablesSuppliersRepo)  
                               :ISupplierService
    {
        public async Task<IEnumerable<SupplierDisplayViewModel>> GetListOfViewModelsAsync()
        {
            var modelList = await suppliersRepo
                .GetAllAsQueryable()
                .Where(x => !x.IsDleted)
                .OrderByDescending(x => x.EditedOn)
                .AsNoTracking()
                .Select(x => new SupplierDisplayViewModel()
                {
                    Name = x.Name,
                    Address = x.Address,
                    Email = x.Email,
                    PhoneNumber = x.PhoneNumber,
                    City = x.City.Name,
                    Country = x.Country.Name,
                    SupplierId = x.SupplierId.ToString()
                })
                .ToListAsync();
            if (modelList == null)
            {
                return new LinkedList<SupplierDisplayViewModel>();
            }
            return modelList;
        }

        public async Task<SupplierCreateViewModel> GetItemForCreateAsync()
        {
            var model = new SupplierCreateViewModel();
            var countries = await countriesRepo
                .GetAllAsQueryable()
                .AsNoTracking()
                .Select(x => new PairViewModel()
                {
                    Name = x.Name,
                    Id = x.CountryId.ToString()
                })
                .ToListAsync();
            var cities = await citiesRepo
                .GetAllAsQueryable()
                .AsNoTracking()
                .Select(x => new PairViewModel()
                {
                    Name = x.Name,
                    Id = x.CityId.ToString()
                })
                .ToListAsync();
            var spareparts = await sparesRepo
                .GetAllAsQueryable()
                .AsNoTracking()
                .Where(x => !x.IsDeleted)
                .Select(x => new PairGuidViewModel()
                {
                    Name = x.SparepartName,
                    Id = x.SparepartId
                })
                .ToListAsync();
            var consumables = await consumablesRepo
                .GetAllAsQueryable()
                .AsNoTracking()
                .Where(x => !x.IsDeleted)
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

            return model;
        }

        public async Task<bool> CreateSparepartAsync(SupplierCreateViewModel model, string userId, List<Guid> Spareparts, List<Guid> Consumables)
        {
            Supplier supplier = new()
            {
                Name = model.Name,
                Address = model.Address,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                CityId = Guid.Parse(model.CityId),
                CountryId = Guid.Parse(model.CountryId),
                CreatorId = userId,
                CreatedOn = DateTime.UtcNow,
                EditedOn = DateTime.UtcNow,
                IsDleted = false
            };
            try
            {
                await suppliersRepo.AddAsync(supplier);
            }
            catch
            {
                return false;   
            }

            var sparepartsSuppliersToAdd = new List<SparepartSupplier>();
            foreach (var sparepartId in Spareparts)
            {
                bool alreadyAdded = await sparePartsSuppliersRepo
                     .GetAllAsQueryable()
                     .AnyAsync(x => x.SparepartId == sparepartId &&
                                    x.SupplierId  == supplier.SupplierId);
                if (!alreadyAdded)
                {
                    SparepartSupplier sparesup = new()
                    {
                        SparepartId = sparepartId,
                        SupplierId = supplier.SupplierId
                    };
                    sparepartsSuppliersToAdd.Add(sparesup);
                    //await sparePartsSuppliersRepo.AddAsync(sparesup);
                }
            }
            if (sparepartsSuppliersToAdd.Count() > 0)
            {
                try
                {
                    await sparePartsSuppliersRepo.AddRangeAsync(sparepartsSuppliersToAdd);
                }
                catch
                {
                    return false;
                }
            }
           

            var consumablesSuppliersToAdd = new List<ConsumableSupplier>();
            foreach (var consumableId in Consumables)
            {
                bool alreadyAdded = await consumablesSuppliersRepo
                    .GetAllAsQueryable()
                    .AnyAsync(x => x.ConsumableId == consumableId &&
                                   x.SupplierId == supplier.SupplierId);
                if (!alreadyAdded)
                {
                    ConsumableSupplier consSup = new()
                    {
                        ConsumableId = consumableId,
                        SupplierId = supplier.SupplierId
                    };
                    consumablesSuppliersToAdd.Add(consSup); 
                    //await consumablesSuppliersRepo.AddAsync(consSup);
                }
            }
            if (consumablesSuppliersToAdd.Count > 0)
            {
                try
                {
                    await consumablesSuppliersRepo.AddRangeAsync(consumablesSuppliersToAdd);
                }
                catch
                {
                    return false;
                }
            }
            
            return true;
        }

        public async Task<SupplierEditViewModel> GetItemForEditAsync(string id)
        {
            var model = await suppliersRepo
                .GetAllAsQueryable()
                .AsNoTracking()
                .Where(x => !x.IsDleted)
                .Where(x => x.SupplierId.ToString().ToLower() == id.ToLower())
                .Select(x => new SupplierEditViewModel()
                {
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
                return new SupplierEditViewModel() {
                    SupplierId = string.Empty,
                    Name = string.Empty,
                    Address = string.Empty,
                    Email = string.Empty,
                    PhoneNumber = string.Empty,
                    CityId = string.Empty,
                    CountryId = string.Empty
                };
            }

            var countries = await countriesRepo
                .GetAllAsQueryable()
                .AsNoTracking()
                .Select(x => new PairViewModel()
                {
                    Name = x.Name,
                    Id = x.CountryId.ToString()
                })
                .ToListAsync();
            var cities = await citiesRepo
                .GetAllAsQueryable()
                .AsNoTracking()
                .Select(x => new PairViewModel()
                {
                    Name = x.Name,
                    Id = x.CityId.ToString()
                })
                .ToListAsync();

            var spareparts = await sparePartsSuppliersRepo
               .GetAllAsQueryable()
               .AsNoTracking()
               .Where(x => x.SupplierId.ToString().ToLower() == id.ToLower())
               .Select(x => new PairGuidViewModel()
               {
                   Name = x.Sparepart.SparepartName,
                   Id = x.SparepartId
               })
               .ToListAsync();

            var consumables = await consumablesSuppliersRepo
                .GetAllAsQueryable()
                .AsNoTracking()
                .Where(x => x.SupplierId.ToString().ToLower() == id.ToLower())
                .Select(x => new PairGuidViewModel()
                {
                    Name = x.Consumable.Name,
                    Id = x.ConsumableId
                })
                .ToListAsync();

            var availableSpareparts = await sparesRepo
               .GetAllAsQueryable()
               .AsNoTracking()
               .Where(x => !x.IsDeleted)
               .Select(x => new PairGuidViewModel()
               {
                   Name = x.SparepartName,
                   Id = x.SparepartId
               })
               .ToListAsync();
            foreach (var spare in spareparts)  // removing already selected spares from the list of all spares, to leave the available spares
            {
                var spareToRemove = availableSpareparts.FirstOrDefault(x => x.Id == spare.Id);
                if (spareToRemove != null)
                {
                    availableSpareparts.Remove(spareToRemove);
                }
            }

            var availableConsumables = await consumablesRepo
                .GetAllAsQueryable()
                .AsNoTracking()
                .Where(x => !x.IsDeleted)
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
            return model;
        }

        public async Task<bool> SaveItemToEditAsync(SupplierEditViewModel model, string userId,
                                                               List<Guid> Spareparts,
                                                               List<Guid> Consumables,
                                                               List<Guid> AvailableSpareparts,
                                                               List<Guid> AvailableConsumables)
        {
            var supplierEdit = await suppliersRepo
                 .GetAllAsQueryable()
                 .Where(x => !x.IsDleted)
                 .Where(x => x.SupplierId.ToString().ToLower() == model.SupplierId.ToLower())
                 .FirstOrDefaultAsync();
            if (supplierEdit == null)
            {
                return false;
            }
            /// Apply Changes To Model
            supplierEdit.Name = model.Name;
            supplierEdit.Address = model.Address;
            supplierEdit.Email = model.Email;
            supplierEdit.PhoneNumber = model.PhoneNumber;
            supplierEdit.CityId = Guid.Parse(model.CityId);
            supplierEdit.CountryId = Guid.Parse(model.CountryId);
            supplierEdit.CreatorId = userId;
            supplierEdit.CreatedOn = DateTime.UtcNow;
            supplierEdit.EditedOn = DateTime.UtcNow;

            /// Related Spareparts Changes Apply
            var sparesSuppliers = await sparePartsSuppliersRepo
                .GetAllAsQueryable()
                .Where(x => x.SupplierId.ToString().ToLower() == model.SupplierId.ToLower())
                .ToListAsync();

            var sparepartsSuppliersToRemove = new List<SparepartSupplier>();
            foreach (var item in sparesSuppliers)   /// remove unselected spares
            {
                if (!Spareparts.Contains(item.SparepartId))
                {
                    sparepartsSuppliersToRemove.Add(item);
                }
            }
            if (sparepartsSuppliersToRemove.Count() > 0)
            {
                try
                {
                    await sparePartsSuppliersRepo.RemoveRangeAsync(sparepartsSuppliersToRemove);
                }
                catch
                {
                    return false;
                }
            }

            var sparepartsSuppliersToAdd = new List<SparepartSupplier>();   
            foreach (var spareId in AvailableSpareparts)
            {
                bool alreadyAdded = await sparePartsSuppliersRepo.GetAllAsQueryable()
                     .AnyAsync(x => x.SparepartId == spareId &&
                                  x.SupplierId == Guid.Parse(model.SupplierId));
                if (!alreadyAdded)
                {
                    SparepartSupplier spareSup = new()
                    {
                        SparepartId = spareId,
                        SupplierId = Guid.Parse(model.SupplierId)
                    };
                    sparepartsSuppliersToAdd.Add(spareSup);
                }
            }
            if (sparepartsSuppliersToAdd.Count() > 0)
            {
                try
                {
                    await sparePartsSuppliersRepo.AddRangeAsync(sparepartsSuppliersToAdd);
                }
                catch
                {
                    return false;
                }
            }

            /// Related Consumables Changes Apply
            var consumSuppliers = await consumablesSuppliersRepo
                .GetAllAsQueryable()
                .Where(x => x.SupplierId.ToString().ToLower() == model.SupplierId.ToLower())
                .ToListAsync();

            var consumablesSuppliersToRemove = new List<ConsumableSupplier>();
            foreach (var item in consumSuppliers)   /// remove unselected spares
            {
                if (!Consumables.Contains(item.ConsumableId))
                {
                    consumablesSuppliersToRemove.Add(item);
                }
            }
            if (consumablesSuppliersToRemove.Count() > 0)
            {
                try
                {
                    await consumablesSuppliersRepo.RemoveRangeAsync(consumablesSuppliersToRemove);
                }
                catch
                {
                    return false;
                }
            }

            var consumablesSuppliersToAdd = new List<ConsumableSupplier>();
            foreach (var consumId in AvailableConsumables)
            {
                bool alreadyAdded = await consumablesSuppliersRepo.GetAllAsQueryable()
                     .AnyAsync(x => x.ConsumableId == consumId &&
                                  x.SupplierId == Guid.Parse(model.SupplierId));
                if (!alreadyAdded)
                {
                    ConsumableSupplier consSup = new()
                    {
                        ConsumableId = consumId,
                        SupplierId = Guid.Parse(model.SupplierId)
                    };
                    consumablesSuppliersToAdd.Add(consSup); 
                }
            }
            if (consumablesSuppliersToAdd.Count() > 0)
            {
                try
                {
                    await consumablesSuppliersRepo.AddRangeAsync(consumablesSuppliersToAdd);
                }
                catch
                {
                    return false;
                }
            }
            return true;
        }

        public async Task<SupplierDetailsViewModel> GetDetailsAsync(string id)
        {
            var model = await suppliersRepo
               .GetAllAsQueryable()
               .AsNoTracking()
               .Where(x => !x.IsDleted)
               .Where(x => x.SupplierId.ToString().ToLower() == id.ToLower())
               .Select(x => new SupplierDetailsViewModel()
               {
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
                return new SupplierDetailsViewModel();
            }
            var consumables = await consumablesSuppliersRepo
                .GetAllAsQueryable()
                .AsNoTracking()
                .Where(x => x.SupplierId.ToString().ToLower() == model.SupplierId!.ToLower())
                .Select(x => x.Consumable.Name)
                .ToListAsync();
            var spareparts = await sparePartsSuppliersRepo
                .GetAllAsQueryable()
                .AsNoTracking()
                .Where(x => x.SupplierId.ToString().ToLower() == model.SupplierId!.ToLower())
                .Select(x => x.Sparepart.SparepartName)
                .ToListAsync();
            if (consumables != null) { model.Consumables = consumables; }
            if (spareparts != null) { model.Spareparts = spareparts; }

            return model;
        }

        public async Task<SupplierDeleteViewModel> GetItemToDeleteAsync(string id)
        {
            var model = await suppliersRepo
                .GetAllAsQueryable()
                .AsNoTracking()
                .Where(x => !x.IsDleted)
                .Where(x => x.SupplierId.ToString().ToLower() == id.ToLower())
                .Select(x => new SupplierDeleteViewModel()
                {
                    SupplierId = x.SupplierId.ToString(),
                    Name = x.Name,
                    Address = x.Address,
                    CreatedOn = x.CreatedOn.ToString(PMSRequiredDateFormat)
                })
                .FirstOrDefaultAsync();
            if (model == null)
            {
                return new SupplierDeleteViewModel();
            }
            return model;
        }

        public async Task<bool> ConfirmDeleteAsync(SupplierDeleteViewModel model)
        {
            
            var modelDelete = await suppliersRepo
               .GetAllAsQueryable()
               .Where(x => !x.IsDleted)
               .Where(x => x.SupplierId.ToString().ToLower() == model.SupplierId.ToLower())
               .FirstOrDefaultAsync();
            if (modelDelete == null)
            {
                return false;
            }
            try
            {
                modelDelete.IsDleted = true;
                await suppliersRepo.UpdateAsync(modelDelete);
            }
            catch
            {
                return false;
            }
           
            return true;
        }


       
    }
}
