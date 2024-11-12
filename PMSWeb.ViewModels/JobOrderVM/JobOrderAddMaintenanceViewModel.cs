using PMSWeb.ViewModels.CommonVM;
using System.ComponentModel.DataAnnotations;

namespace PMSWeb.ViewModels.JobOrderVM
{
    public class JobOrderAddMaintenanceViewModel
    {
        [Required]
        public Guid EquipmentId { get; set; }

        [Required]
        public string EquipmentName { get; set; } = null!;   

        [Required]
        public Guid MaintenanceId { get; set; }

        [Required]
        public string TypeId { get; set; } = null!;

        public List<PairGuidViewModel> Maintenances { get; set; } = new List<PairGuidViewModel>();
    }
}
