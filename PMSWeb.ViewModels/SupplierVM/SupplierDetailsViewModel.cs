namespace PMSWeb.ViewModels.SupplierVM
{
    public class SupplierDetailsViewModel
    {

        public string? SupplierId { get; set; }

        public string? Name { get; set; } 

        public string? Address { get; set; } 

        public string? Email { get; set; } 

        public string? PhoneNumber { get; set; } 

        public string? City { get; set; }

        public string? Country { get; set; } 
        
        public string? Creator { get; set; } 

        public string? CreatedOn { get; set; }

        public List<string> Consumables { get; set; } = new List<string>();
        public List<string> Spareparts { get; set; } = new List<string>();
    }
}
