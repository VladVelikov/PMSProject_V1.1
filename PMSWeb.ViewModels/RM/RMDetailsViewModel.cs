using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMSWeb.ViewModels.RM
{
    public class RMDetailsViewModel
    {
        public string? RoutMaintId { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public string? LastCompletedDate { get; set; }

        public string? Interval { get; set; }

        public string? ResponsiblePosition { get; set; }

        public string CreatorName {get; set; }
        
        public string CreatedOn { get; set; }

        public string EditedOn { get; set; } 

        public List<string> Equipments { get; set; } = new List<string>();
    }
}
