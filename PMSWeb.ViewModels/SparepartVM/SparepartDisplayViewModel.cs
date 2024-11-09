namespace PMSWeb.ViewModels.SparepartVM
{
    public class SparepartDisplayViewModel
    {
        public string? SparepartId { get; set; }

        public string? Name { get; set; } = null!;

        public string? Description { get; set; }
        
        public string? Price { get; set; }
        
        public string? Units { get; set; } = null!;

        public string? ROB { get; set; }

        public string? Equipment { get; set; }

    }
}
