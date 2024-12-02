using Microsoft.EntityFrameworkCore;
using PMS.Data.Models;
using PMS.Data.Repository.Interfaces;
using PMS.Services.Data.Interfaces;
using PMSWeb.ViewModels.CommonVM;
using PMSWeb.ViewModels.Equipment;
using static PMS.Common.EntityValidationConstants;

namespace PMS.Services.Data
{
    public class EquipmentService(IRepository<Equipment,Guid> equipments,
                                  IRepository<Maker, Guid> makersRepo,
                                  IRepository<RoutineMaintenance, Guid> routineMaintenancesRepo,
                                  IRepository<Consumable, Guid> consumablesRepo,
                                  IRepository<RoutineMaintenanceEquipment, Guid[]> routineMaintenanceEquipmentRepo,
                                  IRepository<ConsumableEquipment, Guid[]> consumableEquipmentRepo,
                                  IRepository<SpecificMaintenance, Guid> specificMaintenancesRepo,
                                  IRepository<Sparepart, Guid> sparePartsRepo,
                                  IRepository<Manual, Guid> manualsRepo) 
                                :IEquipmentService
    {
        public async Task<bool> ConfirmDeleteAsync(EquipmentDeleteViewModel model)
        {
            if (model==null || model.EquipmentId == null)
            {
                return false;
            }
            var deleteRecord = await equipments
                .GetAllAsQueryable()
                .Where(x => x.IsDeleted == false)
                .Where(x => x.EquipmentId.ToString().ToLower() == model.EquipmentId.ToLower())
                .FirstOrDefaultAsync();
            if (deleteRecord == null)
            {
                return false;
            }
            try
            {
                deleteRecord.IsDeleted = true;
                await equipments.UpdateAsync(deleteRecord);
            }
            catch
            {
                return false;
            }

            return true;
        }

        public async Task<EquipmentCreateViewModel> GetCreateModelAsync()
        {
            var model = new EquipmentCreateViewModel();
            var makers = await makersRepo
                .GetAllAsQueryable()
                .Where(x => x.IsDeleted == false)
                .AsNoTracking()
                .Select(x => new PairGuidViewModel()
                {
                    Id = x.MakerId,
                    Name = x.MakerName
                })
                .ToListAsync();

            var routineMaintenances = await routineMaintenancesRepo
                .GetAllAsQueryable()
                .Where(x => x.IsDeleted == false)
                .AsNoTracking()
                .Select(x => new PairGuidViewModel()
                {
                    Name = x.Name,
                    Id = x.RoutMaintId
                })
                .ToListAsync();

            var consumables = await consumablesRepo
                .GetAllAsQueryable()
                .Where(x => x.IsDeleted == false)
                .AsNoTracking()
                .Select(x => new PairGuidViewModel()
                {
                    Name = x.Name,
                    Id = x.ConsumableId
                })
                .ToListAsync();

            model.RoutineMaintenances = routineMaintenances;
            model.Consumables = consumables;
            model.Makers = makers;

            return model;
        }

        public async Task<bool> CreateEquipmentAsync(EquipmentCreateViewModel model, string userId,
            List<Guid> RoutineMaintenances,
            List<Guid> Consumables)
        {
            // below is to create and add new equipment to the dbSet
            Equipment equipment = new()
            {
                Name = model.Name,
                Description = model.Description,
                CreatedOn = DateTime.Now,
                EditedOn = DateTime.Now,
                CreatorId = userId,
                MakerId = model.MakerId,
                IsDeleted = false
            };
            await equipments.AddAsync(equipment);

            // Adding additional related items to the current equipment when created  ( Consumables and Routine Maintenances )
            foreach (var maintenanceId in RoutineMaintenances)
            {
                bool alreadyAdded = await routineMaintenanceEquipmentRepo.GetAllAsQueryable()
                     .AnyAsync(x => x.RoutineMaintenanceId == maintenanceId &&
                                  x.EquipmentId == equipment.EquipmentId);
                if (!alreadyAdded)
                {
                    RoutineMaintenanceEquipment rme = new()
                    {
                        RoutineMaintenanceId = maintenanceId,
                        EquipmentId = equipment.EquipmentId
                    };
                    await routineMaintenanceEquipmentRepo.AddAsync(rme);
                }
            }

            foreach (var consumableId in Consumables)
            {
                bool alreadyAdded = await consumableEquipmentRepo.GetAllAsQueryable()   
                    .AnyAsync(x => x.ConsumableId == consumableId &&
                                 x.EquipmentId == equipment.EquipmentId);
                if (!alreadyAdded)
                {
                    ConsumableEquipment consumableEquipment = new()
                    {
                        ConsumableId = consumableId,
                        EquipmentId = equipment.EquipmentId
                    };
                    await consumableEquipmentRepo.AddAsync(consumableEquipment);
                }
            }
           
            return true;
        }

        public async Task<EquipmentDetailsViewModel> GetDetailsAsync(string id)
        {
            EquipmentDetailsViewModel? model = await equipments
               .GetAllAsQueryable()
               .Where(x => !x.IsDeleted)
               .Where(x => x.EquipmentId.ToString().ToLower() == id.ToLower())
               .AsNoTracking()
               .Select(x => new EquipmentDetailsViewModel()
               {
                   EquipmentId = x.EquipmentId.ToString(),
                   Name = x.Name,
                   Description = x.Description,
                   CreatedOn = x.CreatedOn.ToString(PMSRequiredDateFormat),
                   EditedOn = x.EditedOn.ToString(PMSRequiredDateFormat),
                   Creator = x.Creator.UserName,
                   Maker = x.Maker.MakerName
               })
               .FirstOrDefaultAsync();
            if (model == null)
            {
                return new EquipmentDetailsViewModel();
            }


            List<string> routineMaintenances = await routineMaintenanceEquipmentRepo
                .GetAllAsQueryable()
                .Include(x => x.RoutineMaintenance)
                .Where(x => x.EquipmentId.ToString().ToLower() == id.ToLower())
                .Where(x => x.RoutineMaintenance.IsDeleted == false)
                .Select(x => x.RoutineMaintenance.Name)
                .ToListAsync();

            List<string> specificMaintenances = await specificMaintenancesRepo
                .GetAllAsQueryable()
                .Where(x => !x.IsDeleted)
                .Where(x => x.EquipmentId.ToString().ToLower() == id.ToLower())
                .Select(x => x.Name)
                .ToListAsync();

            List<string> spareParts = await sparePartsRepo
                .GetAllAsQueryable()
                .Where(x => !x.IsDeleted)
                .Where(x => x.EquipmentId.ToString().ToLower() == id.ToLower())
                .Select(x => x.SparepartName)
                .ToListAsync();

            List<string> manuals = await manualsRepo
                .GetAllAsQueryable()
                .Where(x => !x.IsDeleted)
                .Where(x => x.EquipmentId.ToString().ToLower() == id.ToLower())
                .Select(x => x.ManualName)
                .ToListAsync();

            List<string> consumables = await consumableEquipmentRepo
                .GetAllAsQueryable()
                .Include(x=>x.Consumable)
                .Where(x => x.EquipmentId.ToString().ToLower() == id.ToLower())
                .Where(x=>x.Consumable.IsDeleted == false)
                .Select(x => x.Consumable.Name)
                .ToListAsync();

            if (routineMaintenances != null)
                model.RoutineMaintenances = routineMaintenances;

            if (specificMaintenances != null)
                model.SpecificMaintenances = specificMaintenances;

            if (spareParts != null)
                model.SpareParts = spareParts;

            if (manuals != null)
                model.Manuals = manuals;

            if (consumables != null)
                model.Consumables = consumables;

            return model;
        }

        public async Task<EquipmentEditViewModel> GetItemForEditAsync(string id)
        {
            var model = await equipments
                .GetAllAsQueryable()
                .Where(x => x.IsDeleted == false)
                .Where(x => x.EquipmentId.ToString().ToLower() == id.ToLower())
                .AsNoTracking()
                .Select(x => new EquipmentEditViewModel()
                {
                    EquipmentId = x.EquipmentId.ToString(),
                    Name = x.Name,
                    Description = x.Description,
                    MakerId = x.MakerId
                })
                .FirstOrDefaultAsync();
            if (model == null) 
            {
                return new EquipmentEditViewModel(); 
            }

            var makers = await makersRepo
                .GetAllAsQueryable()
                .Where(x => x.IsDeleted == false)
                .AsNoTracking()
                .Select(x => new PairGuidViewModel()
                {
                    Name = x.MakerName,
                    Id = x.MakerId
                })
                .ToListAsync();

            var consumables = await consumableEquipmentRepo
                .GetAllAsQueryable()
                .Include(x=>x.Consumable)
                .Where(x => x.EquipmentId.ToString().ToLower() == id.ToLower())
                .Where(x=>x.Consumable.IsDeleted == false)
                .AsNoTracking()
                .Select(x => new PairGuidViewModel()
                {
                    Name = x.Consumable.Name,
                    Id = x.ConsumableId
                })
                .ToListAsync();

            var routineMaintenances = await routineMaintenanceEquipmentRepo
                .GetAllAsQueryable()
                .Include(x=>x.RoutineMaintenance)
                .Where(x => x.EquipmentId.ToString().ToLower() == id.ToLower())
                .Where(x=>x.RoutineMaintenance.IsDeleted == false)
                .AsNoTracking()
                .Select(x => new PairGuidViewModel()
                {
                    Name = x.RoutineMaintenance.Name,
                    Id = x.RoutineMaintenanceId
                })
                .ToListAsync();

            var availableConsumables = await consumablesRepo
                .GetAllAsQueryable()
                .Where(x => x.IsDeleted == false)
                .AsNoTracking()
                .Select(x => new PairGuidViewModel()
                {
                    Name = x.Name,
                    Id = x.ConsumableId
                })
                .ToListAsync();
            foreach (var cons in consumables)
            {
                var avcToRemove = availableConsumables.FirstOrDefault(ac => ac.Id == cons.Id);
                if (avcToRemove != null)
                {
                    availableConsumables.Remove(avcToRemove);
                }
            }

            var availableRoutineMaintenances = await routineMaintenancesRepo
                .GetAllAsQueryable()
                .Where(x => x.IsDeleted == false)
                .AsNoTracking()
                .Select(x => new PairGuidViewModel()
                {
                    Name = x.Name,
                    Id = x.RoutMaintId
                })
                .ToListAsync();
            foreach (var rtm in routineMaintenances)
            {
                var rtmToRemove = availableRoutineMaintenances.FirstOrDefault(rm => rm.Id == rtm.Id);
                if (rtmToRemove != null)
                {
                    availableRoutineMaintenances.Remove(rtmToRemove);
                }

            }

            model.Makers = makers;
            model.Consumables = consumables;
            model.RoutineMaintenances = routineMaintenances;
            model.AvailableRoutineMaintenances = availableRoutineMaintenances;
            model.AvailableConsumables = availableConsumables;

            return model;
        }

        public async Task<EquipmentDeleteViewModel> GetItemToDeleteAsync(string id)
        {
            var model = await equipments
                .GetAllAsQueryable()
                .Where(x => x.IsDeleted == false)
                .Where(x => x.EquipmentId.ToString().ToLower() == id.ToLower())
                .Select(x => new EquipmentDeleteViewModel()
                {
                    EquipmentId = x.EquipmentId.ToString(),
                    Name = x.Name,
                    Description = x.Description,
                    CreatedOn = x.CreatedOn.ToString(PMSRequiredDateFormat)
                })
                .FirstOrDefaultAsync();
            if (model == null)
            {
                return new EquipmentDeleteViewModel();
            }
            return model;
        }

        public async Task<IEnumerable<EquipmentDisplayViewModel>> GetListOfViewModelsAsync()
        {
            var eqList = await equipments
                .GetAllAsQueryable()
                .AsNoTracking()
                .Where(x => x.IsDeleted == false)
                .OrderByDescending(x => x.EditedOn)
                .ThenBy(x => x.Name)
                .Select(x => new EquipmentDisplayViewModel()
                {
                    EquipmentId = x.EquipmentId.ToString(),
                    Name = x.Name,
                    Description = x.Description,
                    CreatedOn = x.CreatedOn.ToString(PMSRequiredDateFormat),
                    EditedOn = x.EditedOn.ToString(PMSRequiredDateFormat),
                    Creator = x.Creator.UserName,
                    Maker = x.Maker.MakerName
                })
                .ToListAsync();
            if (eqList == null)
            {
                return new List<EquipmentDisplayViewModel>();
            }
            return eqList;
        }

        public async Task<bool> SaveItemToEditAsync(EquipmentEditViewModel model, string userId,
            List<Guid> RoutineMaintenances,
            List<Guid> Consumables,
            List<Guid> AvailableRoutineMaintenances,
            List<Guid> AvailableConsumables)
        {
            var eq = await equipments  // equipment to edit 
               .GetAllAsQueryable()
               .Where(x => x.IsDeleted == false)
               .Where(x => x.EquipmentId.ToString().ToLower() == model.EquipmentId.ToLower())
               .FirstOrDefaultAsync();

            if (eq == null)
            {
                return false;
            }

            eq.Name = model.Name;
            eq.Description = model.Description;
            eq.MakerId = model.MakerId;
            eq.EditedOn = DateTime.Now;
            try
            {
                await equipments.UpdateAsync(eq);
            }
            catch
            {
                return false;
            }
            var consEquipments = await consumableEquipmentRepo
                .GetAllAsQueryable()
                .Where(x => x.EquipmentId.ToString().ToLower() == model.EquipmentId.ToLower())
                .ToListAsync();


            var consumableEquipmentsToRemove = new List<ConsumableEquipment>(); 
            foreach (var item in consEquipments)
            {
                if (!Consumables.Contains(item.ConsumableId))
                {
                    consumableEquipmentsToRemove.Add(item); 
                }
            }
            if (consumableEquipmentsToRemove.Count > 0)
            {
                try
                {
                    await consumableEquipmentRepo.RemoveRangeAsync(consumableEquipmentsToRemove);
                }
                catch
                {
                    return false;
                }
                
            }


            var routMaintEquipments = await routineMaintenanceEquipmentRepo
                .GetAllAsQueryable()
                .Where(x => x.EquipmentId.ToString().ToLower() == model.EquipmentId.ToLower())
                .ToListAsync();

            var routineMaintenanceEquipmentsToRemove = new List<RoutineMaintenanceEquipment>(); 
            foreach (var item in routMaintEquipments)
            {
                if (!RoutineMaintenances.Contains(item.RoutineMaintenanceId))
                {
                    routineMaintenanceEquipmentsToRemove.Add(item); 
                }
            }
            if (routineMaintenanceEquipmentsToRemove.Count > 0)
            {
                try
                {
                    await routineMaintenanceEquipmentRepo.RemoveRangeAsync(routineMaintenanceEquipmentsToRemove);
                }
                catch
                {
                    return false;
                }
            }


            var routineMaintenancesEquipmentsToAdd = new List<RoutineMaintenanceEquipment>(); 
            foreach (var maintenanceId in AvailableRoutineMaintenances)
            {
                bool alreadyAdded = await routineMaintenanceEquipmentRepo.GetAllAsQueryable()
                     .AnyAsync(x => x.RoutineMaintenanceId == maintenanceId &&
                                  x.EquipmentId == Guid.Parse(model.EquipmentId));
                if (!alreadyAdded)
                {
                    RoutineMaintenanceEquipment rme = new()
                    {
                        RoutineMaintenanceId = maintenanceId,
                        EquipmentId = Guid.Parse(model.EquipmentId)
                    };
                    routineMaintenancesEquipmentsToAdd.Add(rme);    
                }
            }
            if (routineMaintenancesEquipmentsToAdd.Count > 0)
            {
                try
                {
                    await routineMaintenanceEquipmentRepo.AddRangeAsync(routineMaintenancesEquipmentsToAdd);
                }
                catch
                {
                    return false;
                }
            }

            var consumablesEquipmentsToAdd = new List<ConsumableEquipment>();
            foreach (var consumableId in AvailableConsumables)
            {
                bool alreadyAdded = await consumableEquipmentRepo.GetAllAsQueryable()
                    .AnyAsync(x => x.ConsumableId == consumableId &&
                                 x.EquipmentId == Guid.Parse(model.EquipmentId));
                if (!alreadyAdded)
                {
                    ConsumableEquipment consumableEquipment = new()
                    {
                        ConsumableId = consumableId,
                        EquipmentId = Guid.Parse(model.EquipmentId)
                    };
                    consumablesEquipmentsToAdd.Add(consumableEquipment);
                }
            }
            if (consumablesEquipmentsToAdd.Count > 0)
            {
                try
                {
                    await consumableEquipmentRepo.AddRangeAsync(consumablesEquipmentsToAdd);
                }
                catch
                {
                    return false;
                }
            }
            return true;    
        }
    }
}
