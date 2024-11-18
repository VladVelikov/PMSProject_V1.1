namespace PMSWeb.ViewModels.RequisitionVM
{
    public class RequisitionCreateViewModel
    {

        public string? RequisitionId { get; set; }

        public string? RequisitionName { get; set; }

        public string? RequisitionType { get; set; }

        //public List<AllowedType> allowedTypes { get; set; } = new List<AllowedType> 
        //{
        //    new(){ Id = "Consumable", Name = "Consumable" },
        //    new(){ Id = "Sparepart", Name = "Sparepart" }
        //};

        //public class AllowedType()
        //{
        //    public string Id { get; set; } = null!;
        //    public string Name { get; set; } = null!;
        //}

        public List<RequisitionItemViewModel> RequisitionItems { get; set; } = new List<RequisitionItemViewModel>();

    }
}
