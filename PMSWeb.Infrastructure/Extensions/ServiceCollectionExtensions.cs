using Microsoft.Extensions.DependencyInjection;
using PMS.Data.Models;
using PMS.Data.Repository;
using PMS.Data.Repository.Interfaces;
using PMS.Services.Data;
using PMS.Services.Data.Interfaces;

namespace PMSWeb.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void RegisterRepositories(this IServiceCollection services)
        {
            services.AddScoped<IRepository<Consumable, Guid>, GenericRepository<Consumable, Guid>>();
            services.AddScoped<IRepository<City, Guid>, GenericRepository<City, Guid>>();
            services.AddScoped<IRepository<Country, Guid>, GenericRepository<Country, Guid>>();
            services.AddScoped<IRepository<Equipment, Guid>, GenericRepository<Equipment, Guid>>();
            services.AddScoped<IRepository<Maker, Guid>, GenericRepository<Maker, Guid>>();
            services.AddScoped<IRepository<Manual, Guid>, GenericRepository<Manual, Guid>>();
            services.AddScoped<IRepository<RoutineMaintenance, Guid>, GenericRepository<RoutineMaintenance, Guid>>();
            services.AddScoped<IRepository<Sparepart, Guid>, GenericRepository<Sparepart, Guid>>();
            services.AddScoped<IRepository<SpecificMaintenance, Guid>, GenericRepository<SpecificMaintenance, Guid>>();
            services.AddScoped<IRepository<Supplier, Guid>, GenericRepository<Supplier, Guid>>();
            services.AddScoped<IRepository<ConsumableEquipment, Guid[]>, GenericRepository<ConsumableEquipment, Guid[]>>();
            services.AddScoped<IRepository<RoutineMaintenanceEquipment, Guid[]>, GenericRepository<RoutineMaintenanceEquipment, Guid[]>>();
            services.AddScoped<IRepository<ConsumableSupplier, Guid[]>, GenericRepository<ConsumableSupplier, Guid[]>>();
            services.AddScoped<IRepository<SparepartSupplier, Guid[]>, GenericRepository<SparepartSupplier, Guid[]>>();
            services.AddScoped<IRepository<JobOrder, Guid>, GenericRepository<JobOrder, Guid>>();
        }

        public static void RegisterMyServices(this IServiceCollection services) 
        {
            services.AddScoped<IConsumableService, ConsumableService>();
            services.AddScoped<ICityService, CityService>();
            services.AddScoped<ICountryService, CountryService>();
            services.AddScoped<IEquipmentService, EquipmentService>();
            services.AddScoped<IMakerService, MakerService>();
            services.AddScoped<IManualService, ManualService>();
            services.AddScoped<IRMService, RMService>();
            services.AddScoped<ISMService, SMService>();
            services.AddScoped<ISparepartService, SparepartService>();
            services.AddScoped<ISupplierService, SupplierService>();
            services.AddScoped<IJoborderService, JoborderService>();
            services.AddScoped<IInventoryService, InventoryService>();
        }
    }
}
