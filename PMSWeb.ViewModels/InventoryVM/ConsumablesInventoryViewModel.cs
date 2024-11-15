using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMSWeb.ViewModels.InventoryVM
{
    public class ConsumablesInventoryViewModel
    {
        public string? Name { get; set; }

        public List<InventoryItemViewModel> Consumables { get; set; } = new List<InventoryItemViewModel>();
    }
}
