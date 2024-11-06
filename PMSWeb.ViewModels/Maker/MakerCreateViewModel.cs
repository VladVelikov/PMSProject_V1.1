using Microsoft.EntityFrameworkCore;
using PMS.Data.Models.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static PMS.Common.EntityValidationConstants.MakerConstatnts;

namespace PMSWeb.ViewModels.Maker
{
    public class MakerCreateViewModel
    {
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

    }
}
