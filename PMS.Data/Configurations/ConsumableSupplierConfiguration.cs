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
    public class ConsumableSupplierConfiguration : IEntityTypeConfiguration<ConsumableSupplier>
    {
        public void Configure(EntityTypeBuilder<ConsumableSupplier> builder)
        {
            builder
                .HasOne(e => e.Supplier)
                .WithMany(e => e.ConsumablesSuppliers)
                .HasForeignKey(e => e.SupplierId)
                .OnDelete(DeleteBehavior.NoAction);
            
            builder
                .HasOne(e => e.Consumable)
                .WithMany(e=>e.ConsumablesSuppliers)
                .HasForeignKey(e=>e.ConsumableId)
                .OnDelete(DeleteBehavior.NoAction); 
        }
    }
}
