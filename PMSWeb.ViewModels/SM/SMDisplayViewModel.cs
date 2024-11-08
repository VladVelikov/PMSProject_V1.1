using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMSWeb.ViewModels.SM
{
    public class SMDisplayViewModel
    {
        public string? SpecMaintId { get; set; }
        
        public string? Name { get; set; }
        
        public string? Description { get; set; }
        
        public string? LastCompletedDate { get; set; }
        
        public string? Interval { get; set; }
        
        public string? ResponsiblePosition { get; set; }

        public string? Equipment { get; set; }
    }
}
