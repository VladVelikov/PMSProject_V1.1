using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace PMS.Data.Models
{
    [PrimaryKey(nameof(RoutineMaintenanceId), nameof(EquipmentId))]
    public class RoutineMaintenanceEquipment
    {
        public Guid RoutineMaintenanceId { get; set; } 

        [ForeignKey(nameof(RoutineMaintenanceId))]
        public RoutineMaintenance RoutineMaintenance { get; set; } = null!;

        public Guid EquipmentId { get; set; }

        [ForeignKey(nameof(EquipmentId))]   
        public Equipment Equipment { get; set; } = null!;

    }
}