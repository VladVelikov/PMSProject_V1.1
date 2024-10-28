using Microsoft.AspNetCore.Identity;
using PMS.Data.Models.Enums;
using System.ComponentModel.DataAnnotations;
using static PMS.Common.EntityValidationConstants;

namespace PMS.Data.Models.Identity
{
    public class PMSUser : IdentityUser
    {
        
        [MaxLength(UserNameMaxLength)]
        [MinLength(UserNameMinLength)]
        public string? FullUserName { get; set; }
        
        public Position? Position { get; set; }
    }
}
