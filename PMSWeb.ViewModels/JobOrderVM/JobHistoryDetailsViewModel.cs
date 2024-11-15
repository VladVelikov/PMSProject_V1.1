using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMSWeb.ViewModels.JobOrderVM
{
    public class JobHistoryDetailsViewModel
    {
        public string JobId { get; set; }
        public string JobName { get; set; }
        public string CompletedBy { get; set; }
        public string LastDoneDate { get; set; }
        public string Type { get; set; }
        public string ResponsiblePosition { get; set; }
        public string MaintainedEquipment {  get; set; }
        public string Desription { get; set; }

    }
}
