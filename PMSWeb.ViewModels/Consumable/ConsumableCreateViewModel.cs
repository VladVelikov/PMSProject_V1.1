using Microsoft.EntityFrameworkCore;
using PMS.Data.Models;
using PMS.Data.Models.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static PMS.Common.EntityValidationConstants.ConsumableConstants;

namespace PMSWeb.ViewModels.Consumable
{
    public class ConsumableCreateViewModel
    {

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

        public ICollection<ConsumableEquipment> ConsumablesEquipments = new HashSet<ConsumableEquipment>();

        public ICollection<ConsumableSupplier> ConsumablesSuppliers = new HashSet<ConsumableSupplier>();

    }
}
