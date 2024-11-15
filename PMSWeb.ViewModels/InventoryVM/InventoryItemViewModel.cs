using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMSWeb.ViewModels.InventoryVM
{
    public class InventoryItemViewModel
    {
        public string? Id { get; set; }

        public string? Name { get; set; }

        public double Available { get; set; }

        public string? Units { get; set; }
        
        public double Used { get; set; }

        public DateTime EditedOn { get; set; }

        public string? Price { get; set; }

    }
}
