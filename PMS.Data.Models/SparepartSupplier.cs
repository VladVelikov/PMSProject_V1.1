using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace PMS.Data.Models
{
    [PrimaryKey(nameof(SupplierId), nameof(SparepartId))]
    public class SparepartSupplier
    {
        public Guid SparepartId { get; set; } 

        [ForeignKey(nameof(SparepartId))]
        public virtual Sparepart Sparepart { get; set; } = null!;

        public Guid SupplierId { get; set;} 

        [ForeignKey(nameof(SupplierId))]
        public virtual Supplier Supplier { get; set; } = null!;
    }
}