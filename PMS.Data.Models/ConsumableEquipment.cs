using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace PMS.Data.Models
{
    [PrimaryKey(nameof(ConsumableId), nameof(EquipmentId))]
    public class ConsumableEquipment
    {
        public Guid ConsumableId { get; set; }

        [ForeignKey(nameof(ConsumableId))]
        public Consumable Consumable { get; set; } = null!;

        public Guid EquipmentId { get; set; } 

        [ForeignKey(nameof(EquipmentId))]   
        public Equipment Equipment { get; set; } = null!;
    }
}