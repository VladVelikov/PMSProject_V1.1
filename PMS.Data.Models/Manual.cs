using Microsoft.EntityFrameworkCore;
using PMS.Data.Models.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static PMS.Common.EntityValidationConstants.ManualClassConstants;

namespace PMS.Data.Models
{
    public class Manual
    {
        public Manual() 
        {
            this.ManualId = Guid.NewGuid();
        } 

        [Key]
        [Comment("Unique identifier of the manual")]
        public Guid ManualId { get; set; }

        [Required]
        [MaxLength(ManualNameMaxLength)]
        [MinLength(ManualNameMinLength)]
        [Comment("Name of the manual")]
        public string ManualName { get; set; } = null!;

        [Required]
        [Comment("Unique identifier of the maker")]
        public Guid MakerId { get; set; }

        [ForeignKey(nameof(MakerId))]
        public virtual Maker Maker { get; set; } = null!;


        [Comment("Unique identifier of the equipment")]
        public Guid EquipmentId { get; set; }

        [ForeignKey(nameof(EquipmentId))]
        public virtual Equipment? Equipment { get; set; }    

        [Required]
        [Comment("Unique identifier of the Creator")]
        public string CreatorId { get; set; } = null!;

        [ForeignKey(nameof(CreatorId))]
        public virtual PMSUser Creator { get; set; } = null!;

        [Required]
        [Comment("Date when created on")]
        public DateTime CreatedOn { get; set; }

        [Required]
        [Comment("Date when last edited")]
        public DateTime EditedOn { get; set; }


        [MaxLength(ManualURLMaxLength)]
        [MinLength(ManualURLMinLength)]
        [Comment("URL to file with content of the manual")]
        public string? ContentURL { get; set; }

        [Comment("Soft delete implemented")]
        public bool IsDeleted { get; set; }


    }
}