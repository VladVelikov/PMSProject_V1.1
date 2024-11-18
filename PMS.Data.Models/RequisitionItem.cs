using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static PMS.Common.EntityValidationConstants.RequisitionConstants;

namespace PMS.Data.Models
{
    public class RequisitionItem
    {
        public RequisitionItem() 
        {
            this.ItemId = Guid.NewGuid();
        }    

        [Key]
        [Comment("Unique identifier of the RequisitionItem in the Requisition Item Table")]
        public Guid ItemId { get; set; }

        [Required]
        [Comment("Unique identifier of the item, that will be purchased -> Consumable or Spare")]
        public Guid PurchasedItemId { get; set; }

        [Required]
        [MaxLength(RequisitionItemNameMaxLength)]
        [MinLength(RequisitionItemNameMinLength)]
        [Comment("Name of the req item")]
        public string Name { get; set; } = null!;

        [Comment("The Amount To Order")]
        public double OrderedAmount { get; set; }

        [Required]
        [MaxLength(RequisitionUnitsMaxLength)]
        [MinLength(RequisitionUnitsMinLength)]
        [Comment("Measuring units if the req item")]
        public string Units { get; set; } = null!;

        [Required]
        [Column("Price", TypeName ="DECIMAL(18,2)")]
        [Comment("Price of the requisition item")]
        public decimal Price { get; set; }

        [Required]
        [MaxLength(RequisitionTypeMaxLength)]
        [MinLength(RequisitionTypeMinLength)]
        [Comment("Type of the requisition item = Consumable or Spare")]
        public string Type { get; set; } = null!;

        [Comment("Name of the item's supplier")]
        public string? SupplierName { get; set; }

        [Comment("Date when item was added to the requisition")]
        public DateTime CreatedOn { get; set; }

        [Required]
        [Comment("Uniquer identifier of the requisition whre item is placed")]
        public Guid RequisitionId { get; set; }

        [ForeignKey(nameof(RequisitionId))]
        public Requisition Requisition { get; set; } = null!; 
    }
}
