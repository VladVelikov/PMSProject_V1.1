using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMSWeb.ViewModels.JobOrderVM
{
    public class OpenManualViewModel
    {
        public string? JobId { get; set; }

        public string? URL { get; set; }

        public string? Name { get; set; }

        public string? MakerName { get; set; }

        public string? EquipmentName { get; set; }

        public string? FileContent { get; set; }

        public string? ContentType { get; set; }
    }
}
