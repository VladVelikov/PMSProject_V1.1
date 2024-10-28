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
    public class ManualConfiguration : IEntityTypeConfiguration<Manual>
    {
        public void Configure(EntityTypeBuilder<Manual> builder)
        {
            builder
                .HasOne(x => x.Maker)
                .WithMany(x => x.Manuals)
                .HasForeignKey(x => x.MakerId)
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .HasOne(x=>x.Sparepart)
                .WithMany(x=>x.Manuals)
                .HasForeignKey(x=>x.SparepartId)
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .HasOne(x=>x.Equipment)
                .WithMany(x=>x.Manuals)
                .HasForeignKey(x=>x.EquipmentId)
                .OnDelete(DeleteBehavior.NoAction);

        }
    }
}
