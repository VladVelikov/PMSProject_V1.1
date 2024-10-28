using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PMS.Data.Configurations;
using PMS.Data.Models;
using PMS.Data.Models.Identity;
using PMS.Data.Seeders;

namespace PMS.Data
{
    public class PMSDbContext : IdentityDbContext<PMSUser>
    {

        public PMSDbContext(DbContextOptions<PMSDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new EquipmentConfiguration());
            builder.ApplyConfiguration(new ConsumableSupplierConfiguration());
            builder.ApplyConfiguration(new ConsumableEquipmentConfiguration());
            builder.ApplyConfiguration(new RoutineMantenancesEquipmentsConfiguration());
            builder.ApplyConfiguration(new SpecificMaintenenaceConfiguration());
            builder.ApplyConfiguration(new SparepartConfiguration());
            builder.ApplyConfiguration(new ManualConfiguration());
            builder.ApplyConfiguration(new SparepartSupplierConfiguration());

            base.OnModelCreating(builder);
        }

        public DbSet<City> Cities { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Consumable> Consumables { get; set; }
        public DbSet<Equipment> Equipments { get; set; }
        public DbSet<Maker> Makers { get; set; }
        public DbSet<Manual> Manuals { get; set; }
        public DbSet<RoutineMaintenance> RoutineMaintenances { get; set; }
        public DbSet<SpecificMaintenance> SpecificMaintenances { get; set; }
        public DbSet<Sparepart> Spareparts { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<ConsumableEquipment> ConsumablesEquipments { get; set; }
        public DbSet<SparepartSupplier> SparepartsSuppliers { get; set; }
        public DbSet<RoutineMaintenanceEquipment> RoutineMaintenancesEquipments { get; set; }
        public DbSet<ConsumableSupplier> ConsumablesSuppliers { get; set; }

    }
}
