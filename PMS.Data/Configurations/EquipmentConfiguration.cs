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
    public class EquipmentConfiguration : IEntityTypeConfiguration<Equipment>
    {
        public void Configure(EntityTypeBuilder<Equipment> builder)
        {
            builder
                .HasOne(e=>e.Maker)
                .WithMany(e=>e.Equipment)
                .HasForeignKey(e=>e.MakerId)
                .OnDelete(DeleteBehavior.NoAction); 
                
        }
    }
}
