using PMS.Data.Models;

namespace PMSWeb.ViewModels.Equipment
{
    public class EquipmentDisplayViewModel
    {
        public string? EquipmentId { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public string? CreatedOn { get; set; }

        public string? Creator { get; set; }   

        public string? EditedOn { get; set; }

        public string? Maker { get; set; } 


        public virtual ICollection<RoutineMaintenanceEquipment> RoutineMaintenancesEquipments { get; set; }
                                                       = new HashSet<RoutineMaintenanceEquipment>();

        public ICollection<SpecificMaintenance> SpecificMaintenances = new List<SpecificMaintenance>();


        public ICollection<Sparepart> SpareParts = new List<Sparepart>();

        public ICollection<Manual> Manuals = new List<Manual>();

        public ICollection<ConsumableEquipment> ConsumablesEquipments = new HashSet<ConsumableEquipment>();

    }
}
