using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace PMS.Data.Models
{
    [PrimaryKey(nameof(ConsumableId), nameof(SupplierId))]
    public class ConsumableSupplier
    {
        public Guid ConsumableId { get; set; } 

        [ForeignKey(nameof(ConsumableId))]
        public Consumable Consumable { get; set; } = null!;

        public Guid SupplierId { get; set; } 

        [ForeignKey(nameof(SupplierId))]
        public Supplier Supplier { get; set; } = null!;

    }
}