using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMSWeb.ViewModels.Manual
{
    public class ManualDeleteViewModel
    {
        public string? ManualName {get;set;}
        
        public string? EquipmentName { get; set; }
        
        public string? MakerName { get; set; }
        
        public string? CreatedOn { get; set; }
        
        public string? ManualId { get; set; }
    }
}
