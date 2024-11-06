using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMSWeb.ViewModels.Maker
{
    public class MakerDetailsViewModel
    {
        public string? MakerId { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public string? Email { get; set; }

        public string? Phone { get; set; }

        public string? CreatedOn { get; set; }

        public string? EditedOn { get; set; }

        public List<string> EquipmentsList { get; set; }

        public List<string> ManualsList { get; set; }

    }
}
