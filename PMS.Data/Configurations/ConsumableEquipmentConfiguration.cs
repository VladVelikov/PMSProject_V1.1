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
    public class ConsumableEquipmentConfiguration : IEntityTypeConfiguration<ConsumableEquipment>
    {
        public void Configure(EntityTypeBuilder<ConsumableEquipment> builder)
        {
            builder
                .HasOne(e => e.Consumable)
                .WithMany(e => e.ConsumablesEquipments)
                .HasForeignKey(e => e.ConsumableId)
                .OnDelete(DeleteBehavior.NoAction);
            
            builder
                .HasOne(e=>e.Equipment)
                .WithMany(e=>e.ConsumablesEquipments)
                .HasForeignKey(e=>e.EquipmentId)
                .OnDelete(DeleteBehavior.NoAction); 
               
        }
    }
}
