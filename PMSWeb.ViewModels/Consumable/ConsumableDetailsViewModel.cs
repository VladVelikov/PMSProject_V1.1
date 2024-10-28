using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMSWeb.ViewModels.Consumable
{
    public class ConsumableDetailsViewModel
    {
        public string ConsumableId { get; set; }

        public string Name { get; set; } 

        public string Units { get; set; }

        public string? Description { get; set; }

        public string Price { get; set; }

        public double ROB { get; set; }

        public DateTime EditedOn { get; set; }

        public string CreatorId { get; set; }

        public string CreatorName { get; set;}

        public DateTime CreatedOn { get; set; }

    }
}
