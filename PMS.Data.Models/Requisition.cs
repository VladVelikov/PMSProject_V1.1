using Microsoft.EntityFrameworkCore;
using PMS.Data.Models.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static PMS.Common.EntityValidationConstants.RequisitionConstants;

namespace PMS.Data.Models
{
    public class Requisition
    {
        public Requisition() 
        {
            this.RequisitionId = Guid.NewGuid(); 
        }

        [Key]
        [Comment("Unique identifier of the requisition")]
        public Guid RequisitionId { get; set; }

        [Required]
        [MaxLength(RequisitionNameMaxLength)]
        [MinLength(RequisitionNameMinLength)]
        [Comment("The name of the requisition")]
        public string RequisitionName { get; set; } = null!;

        [Required]
        [Comment("Date when requisition is created")]
        public DateTime CreatedOn { get; set; }

        [Required]
        [Comment("The type of the requisition Consumable or Spare")]
        public string RequisitionType { get; set; } = null!;

        [Required]
        [Comment("Unique identifier of the user who created the requisition")]
        public string CreatorId { get; set; } = null!;

        [ForeignKey(nameof(CreatorId))]
        public PMSUser Creator { get; set; } = null!;

        public virtual ICollection<RequisitionItem> requisitionItems { get; set; } = new List<RequisitionItem>();  

        [Comment("The total cost of all items in the requisition")]
        [Column("TotalCost", TypeName ="DECIMAL(18,2)")]
        public decimal TotalCost { get; set;}

        public bool IsDeleted { get; set; }
        
        public bool IsApproved { get; set; }
    }
}
