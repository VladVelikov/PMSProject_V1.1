using PMS.Data.Models;

namespace PMSWeb.ViewModels.JobOrderVM
{
    public class SelectManualViewModel
    {
        public string? EquipmentName { get; set; }

        public string? JobId { get; set; }

        public List<PMS.Data.Models.Manual> Manuals { get; set; } = new List<PMS.Data.Models.Manual>(); 
    }
}
