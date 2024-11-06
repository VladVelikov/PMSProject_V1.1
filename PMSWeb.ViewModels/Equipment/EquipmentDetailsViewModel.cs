using PMS.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMSWeb.ViewModels.Equipment
{
    public class EquipmentDetailsViewModel
    {
        public string? EquipmentId { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public string? CreatedOn { get; set; }

        public string? Creator { get; set; }

        public string? EditedOn { get; set; }

        public string? Maker { get; set; }

        public List<String> SpecificMaintenances { get; set; } = new List<String>();
        
        public List<String> RoutineMaintenances { get; set; } = new List<String>();
        
        public List<String> SpareParts { get; set; } = new List<String>();
        
        public List<String> Manuals { get; set; } = new List<String>();
        
        public List<String> Consumables { get; set; } = new List<String>();
        
    }
}
