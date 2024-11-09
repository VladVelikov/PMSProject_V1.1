namespace PMSWeb.ViewModels.SupplierVM
{
    public class SupplierDisplayViewModel
    {
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }
        public string SupplierId { get; set; } = null!;

    }
}
