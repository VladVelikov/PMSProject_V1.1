using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PMS.Data.Models
{
    public class Budget
    {
        [Key]
        [Comment("Unique identifier of the budget record.")]
        public int Id { get; set; }

        [Required]
        [Comment("Date When Last Time The Budget Was Increased or Decreased")]
        public DateTime LastChangeDate { get; set; }

        [Required]
        [Range(0, 100000000000000)]
        [Column("Ballance", TypeName ="DECIMAL(18,2)")]
        [Comment("Remaining funds in budget.")]
        public decimal  Ballance { get; set; }
    }
}
