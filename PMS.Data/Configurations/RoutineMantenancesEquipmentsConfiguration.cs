using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PMS.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMS.Data.Configurations
{
    public class RoutineMantenancesEquipmentsConfiguration : IEntityTypeConfiguration<RoutineMaintenanceEquipment>
    {
        public void Configure(EntityTypeBuilder<RoutineMaintenanceEquipment> builder)
        {
            builder
                .HasOne(e => e.RoutineMaintenance)
                .WithMany(e => e.RoutineMaintenancesEquipments)
                .HasForeignKey(e => e.RoutineMaintenanceId)
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .HasOne(x=>x.Equipment)
                .WithMany(x=>x.RoutineMaintenancesEquipments)
                .HasForeignKey(x=>x.EquipmentId)
                .OnDelete(DeleteBehavior.NoAction);

        }
    }
}
