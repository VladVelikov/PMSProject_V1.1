using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMSWeb.ViewModels.Equipment
{
    public class EquipmentDeleteViewModel
    {
        public string? EquipmentId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? CreatedOn { get; set; }    
    }
}
