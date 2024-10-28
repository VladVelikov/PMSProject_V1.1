namespace PMSWeb.ViewModels.Consumable
{
    public class ConsumableDisplayViewModel
    {
        public string ConsumableId { get; set; }

        public string Name { get; set; } = null!;

        public string? Description { get; set; }
        
        public decimal Price { get; set; }
        
        public string Units { get; set; } = null!;

        public double ROB { get; set; }
        
        public DateTime EditedOn { get; set; }

    }
}
