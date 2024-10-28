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
    public class SpecificMaintenenaceConfiguration : IEntityTypeConfiguration<SpecificMaintenance>
    {
        public void Configure(EntityTypeBuilder<SpecificMaintenance> builder)
        {
            builder
                .HasOne(e => e.Equipment)
                .WithMany(e => e.SpecificMaintenances)
                .HasForeignKey(e => e.EquipmentId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
