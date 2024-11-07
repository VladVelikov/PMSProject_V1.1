using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMSWeb.ViewModels.RM
{
    public class RMDisplayViewModel
    {
        public string? RoutMaintId { get; set; }
        
        public string? Name { get; set; }
        
        public string? Description { get; set; }
        
        public string? LastCompletedDate { get; set; }
        
        public string? Interval { get; set; }
        
        public string? ResponsiblePosition { get; set; }
    }
}
