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
    public class SparepartConfiguration : IEntityTypeConfiguration<Sparepart>
    {
        public void Configure(EntityTypeBuilder<Sparepart> builder)
        {
            builder
                .HasOne(x => x.Equipment)
                .WithMany(x => x.SpareParts)
                .HasForeignKey(x => x.EquipmentId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
