using Microsoft.EntityFrameworkCore;
using PMS.Data.Models.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static PMS.Common.EntityValidationConstants.MakerConstatnts;

namespace PMS.Data.Models
{
    public class Maker
    {
        public Maker()
        {
            this.MakerId = Guid.NewGuid();
        }

        [Key]
        [Comment("Unique identifier of Maker")]
        public Guid MakerId { get; set; }

        [Required]
        [MaxLength(MakerNameMaxLength)]
        [MinLength(MakerNameMinLength)]
        [Comment("Name of the maker")]
        public string MakerName { get; set; } = null!;


        [MaxLength(MakerDescriptionMaxLength)]
        [MinLength(MakerDescriptionMinLength)]
        [Comment("Description of the maker")]
        public string? Description { get; set; }


        [MaxLength(MakerEmailMaxLength)]
        [MinLength(MakerEmailMinLength)]
        [Comment("The E-mail of the maker")]
        public string? Email { get; set; }

        [MaxLength(MakerPhoneMaxLength)]
        [MinLength(MakerPhoneMinLength)]
        [Comment("Phone number of the maker")]
        public string? Phone { get; set; }

        [Required]
        [Comment("Id of the creator of this Maker entry")]
        public string CreatorId { get; set; } = null!;

        [ForeignKey(nameof(CreatorId))]
        public PMSUser Creator { get; set; } = null!;

        [Required]
        [Comment("Date when created on")]
        public DateTime CreatedOn { get; set; }

        [Required]
        [Comment("Date when last edited")]
        public DateTime EditedOn { get; set; }


        public ICollection<Equipment> Equipment { get; set; } = new List<Equipment>();

        public ICollection<Manual> Manuals { get; set; } = new List<Manual>();

        [Comment("Soft delete implemented")]
        public bool IsDeleted { get; set; } 


    }
}