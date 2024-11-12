using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PMS.Data.Models;

namespace PMS.Data.Configurations
{
    public class JobOrderConfiguration : IEntityTypeConfiguration<JobOrder>
    {
        public void Configure(EntityTypeBuilder<JobOrder> builder)
        {
            builder
                .HasOne(x => x.Equipment)
                .WithMany(x => x.JobOrders)
                .HasForeignKey(x => x.EquipmentId)
                .OnDelete(DeleteBehavior.NoAction);
            
            builder
                .HasOne(x=>x.RoutineMaintenance)
                .WithMany(x=>x.JobOrders)
                .HasForeignKey(x=>x.RoutineMaintenanceId)
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .HasOne(x => x.SpecificMaintenance)
                .WithMany(x => x.JobOrders)
                .HasForeignKey(x => x.SpecificMaintenanceId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
