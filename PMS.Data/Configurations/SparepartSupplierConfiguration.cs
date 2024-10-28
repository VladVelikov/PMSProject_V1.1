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
    public class SparepartSupplierConfiguration : IEntityTypeConfiguration<SparepartSupplier>
    {
        public void Configure(EntityTypeBuilder<SparepartSupplier> builder)
        {
            builder
                .HasOne(e=>e.Sparepart)
                .WithMany(e=>e.SparepartsSuppliers)
                .HasForeignKey(e=>e.SparepartId)
                .OnDelete(DeleteBehavior.NoAction);
            
            builder
                .HasOne(e=>e.Supplier)
                .WithMany(e=>e.SparepartsSuppliers)
                .HasForeignKey(e=>e.SupplierId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
