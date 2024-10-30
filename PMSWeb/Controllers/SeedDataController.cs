using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PMS.Data;
using PMS.Data.Seeders;
using System.Security.Claims;

namespace PMSWeb.Controllers
{
    public class SeedDataController(PMSDbContext context) : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        // Seeds sample data in the basic tables.
        public async Task<IActionResult> Seed()
        {
            string userId = GetUserId() ?? string.Empty;

            Seeder seeder = new();

            if (!string.IsNullOrEmpty(userId))
            {
                if (!await context.Consumables.AnyAsync())
                {
                    foreach (var c in seeder.GetConsumables(userId))
                    {
                        await context.Consumables.AddAsync(c);
                    }

                }          // Seeds Consumables
                await context.SaveChangesAsync();

                if (!await context.Makers.AnyAsync())
                {
                    foreach (var m in seeder.GetMakers(userId))
                    {
                        await context.Makers.AddAsync(m);
                    }
                }               // Seeds Makers
                await context.SaveChangesAsync();

                if (!await context.Cities.AnyAsync())
                {
                    foreach (var city in seeder.GetCities())
                    {
                        await context.Cities.AddAsync(city);
                    }
                }               // Seeds Cities
                await context.SaveChangesAsync();

                if (!await context.Countries.AnyAsync())
                {
                    foreach (var country in seeder.GetCountries())
                    {
                        await context.Countries.AddAsync(country);
                    }
                }            // Seeds Countries
                await context.SaveChangesAsync();

                if (!await context.Suppliers.AnyAsync())
                {
                    var cityIds = await context
                    .Cities
                    .AsNoTracking()
                    .Select(x => x.CityId.ToString())
                    .ToListAsync();
                    var countryIds = await context
                        .Countries
                        .AsNoTracking()
                        .Select(x => x.CountryId.ToString())
                        .ToListAsync();

                    foreach (var supplier in seeder.GetSuppliers(cityIds, countryIds, userId))
                    {
                        await context.Suppliers.AddAsync(supplier);
                    }
                }             // Seeds Suppliers
                await context.SaveChangesAsync();

                if (!await context.Equipments.AnyAsync())
                {
                    var makerIds = await context
                    .Makers
                    .AsNoTracking()
                    .Select(x => x.MakerId.ToString())
                    .ToListAsync();

                    foreach (var equipment in seeder.GetEquipments(makerIds, userId))
                    {
                        await context.Equipments.AddAsync(equipment);
                    }
                }            // Seeds Equipments
                await context.SaveChangesAsync();

                if (!await context.Spareparts.AnyAsync())
                {
                    var equipmentIds = await context
                        .Equipments
                        .AsNoTracking()
                        .Select(x => x.EquipmentId.ToString())
                        .ToListAsync();
                    foreach (var spare in seeder.GetSpareparts(equipmentIds, userId))
                    {
                        await context.Spareparts.AddAsync(spare);
                    }
                }            // Seeds Spareparts
                await context.SaveChangesAsync();

                if (!await context.RoutineMaintenances.AnyAsync())
                {
                    foreach (var routineMaintenance in seeder.GetRoutineMaintenances(userId))
                    {
                        await context.RoutineMaintenances.AddAsync(routineMaintenance);
                    }
                }    // Seeds Routine Maintenances
                await context.SaveChangesAsync();

                if (!await context.SpecificMaintenances.AnyAsync())
                {
                    var equipmentIds = await context
                        .Equipments
                        .AsNoTracking()
                        .Select(x => x.EquipmentId.ToString())
                        .ToListAsync();

                    foreach (var specificMaintenance in seeder.GetSpecificMaintenances(equipmentIds, userId))
                    {
                        await context.SpecificMaintenances.AddAsync(specificMaintenance);
                    }
                }    // Seeds Specific Maintenances
                await context.SaveChangesAsync();
            }

            return View();
        }

        public string? GetUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier)!.ToString();
        }
    }
}
