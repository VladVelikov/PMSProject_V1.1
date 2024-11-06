using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using static PMS.Common.EntityValidationConstants.MakerConstatnts;

namespace PMSWeb.ViewModels.Maker
{
    public class MakerEditViewModel
    {
        [Required]
        public string MakerId { get; set; } = null!;

        [Required]
        [MaxLength(MakerNameMaxLength)]
        [MinLength(MakerNameMinLength)]
        public string MakerName { get; set; } = null!;


        [MaxLength(MakerDescriptionMaxLength)]
        [MinLength(MakerDescriptionMinLength)]
        public string? Description { get; set; }


        [MaxLength(MakerEmailMaxLength)]
        [MinLength(MakerEmailMinLength)]
        public string? Email { get; set; }

        [MaxLength(MakerPhoneMaxLength)]
        [MinLength(MakerPhoneMinLength)]
        public string? Phone { get; set; }

    }
}
