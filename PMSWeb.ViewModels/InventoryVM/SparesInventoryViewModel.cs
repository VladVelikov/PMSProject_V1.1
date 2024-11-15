namespace PMSWeb.ViewModels.InventoryVM
{
    public class SparesInventoryViewModel
    {
        public string? Name { get; set; } 
        
        public List<InventoryItemViewModel> Spares { get; set; } = new List<InventoryItemViewModel>();
    }
}
