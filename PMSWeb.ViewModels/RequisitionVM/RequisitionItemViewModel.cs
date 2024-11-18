using PMS.Data.Models;

namespace PMSWeb.ViewModels.RequisitionVM
{
    public class RequisitionItemViewModel
    {
        public string Id { get; set; }

        public string Name { get; set; } = null!;

        public double Available { get; set; }

        public double ToOrdered { get; set; }

        public string Units { get; set; }

        public decimal Price { get; set; }

        public string SupplierName { get; set; }

        public bool IsSelected { get; set; }

        public List<Supplier> Suppliers { get; set; } = new List<Supplier>();

    }
}
