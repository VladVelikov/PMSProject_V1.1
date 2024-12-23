﻿using PMSWeb.ViewModels.CommonVM;
using System.ComponentModel.DataAnnotations;

namespace PMSWeb.ViewModels.JobOrderVM
{
    public class JobOrderAddEquipmentViewModel
    {
        [Required]
        public Guid EquipmentId { get; set; }

        public List<PairGuidViewModel> EquipmentList { get; set; } = new List<PairGuidViewModel>();

        [Required]
        public string TypeId { get; set; } = null!;

        public List<PairViewModel> TypeList { get; set; } = new List<PairViewModel>() {
            new (){ Name = "Routine", Id = "Routine"},
            new (){ Name = "Specific", Id = "Specific"}
        };
    }
}
