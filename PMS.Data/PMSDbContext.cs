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

        public virtual DbSet<City> Cities { get; set; }
        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<Consumable> Consumables { get; set; }
        public virtual DbSet<Equipment> Equipments { get; set; }
        public virtual DbSet<Maker> Makers { get; set; }
        public virtual DbSet<Manual> Manuals { get; set; }
        public virtual DbSet<RoutineMaintenance> RoutineMaintenances { get; set; }
        public virtual DbSet<SpecificMaintenance> SpecificMaintenances { get; set; }
        public virtual DbSet<Sparepart> Spareparts { get; set; }
        public virtual DbSet<Supplier> Suppliers { get; set; }
        public virtual DbSet<ConsumableEquipment> ConsumablesEquipments { get; set; }
        public virtual DbSet<SparepartSupplier> SparepartsSuppliers { get; set; }
        public virtual DbSet<RoutineMaintenanceEquipment> RoutineMaintenancesEquipments { get; set; }
        public virtual DbSet<ConsumableSupplier> ConsumablesSuppliers { get; set; }
    }
}
