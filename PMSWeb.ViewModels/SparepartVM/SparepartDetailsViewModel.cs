namespace PMSWeb.ViewModels.SparepartVM
{
    public class SparepartDetailsViewModel
    {
        public string SparepartId { get; set; } = null!;

        public string? Name { get; set; } 

        public string? Description { get; set; }
        
        public string? Price { get; set; }
        
        public string? Units { get; set; }
        
        public string? ROB { get; set; }
        
        public string? EditedOn { get; set; }
        
        public string? CreatedOn { get; set; }
        
        public string? CreatorName { get; set;}

        public string? ImageURL { get; set; }

    }
}
