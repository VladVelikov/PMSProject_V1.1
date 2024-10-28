using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static PMS.Common.EntityValidationConstants.ConsumableConstants;

namespace PMSWeb.ViewModels.Consumable
{
    public class ConsumableEditViewModel
    {
        public string? ConsumableId { get; set; }

        [Required]
        [MaxLength(ConsumableNameMaxLength)]
        [MinLength(ConsumableNameMinLength)]
        public string Name { get; set; } = null!;

        [Required]
        [MaxLength(ConsumableUnitStringMaxLength)]
        [MinLength(ConsumableUnitStringMinLength)]
        public string Units { get; set; } = null!;

        [MaxLength(ConsumableDescriptionMaxLength)]
        [MinLength(ConsumableDescriptionMinLength)]
        public string? Description { get; set; }

        [Required]
        [Column("Price", TypeName = "DECIMAL(18,2)")]
        [Comment("The price of the consumable per unit")]
        public decimal Price { get; set; }

        [Required]
        [Range(0, ConsumableROBMaxValue)]
        [Comment("The remaining quantity on stock")]
        public double ROB { get; set; }
    }
}
