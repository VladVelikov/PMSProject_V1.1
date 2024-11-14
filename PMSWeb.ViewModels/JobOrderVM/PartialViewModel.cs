using PMSWeb.ViewModels.InventoryVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMSWeb.ViewModels.JobOrderVM
{
    public class PartialViewModel
    {
        public string JobId { get; set; }
      
        public string EquipmentId { get; set; }
        
        public List<InventoryItemViewModel> InventoryList { get; set; }  = new List<InventoryItemViewModel>();
    }
}
