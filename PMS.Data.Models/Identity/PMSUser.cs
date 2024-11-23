using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using static PMS.Common.EntityValidationConstants;

namespace PMS.Data.Models.Identity
{
    public class PMSUser : IdentityUser
    {
        [Required]
        public string Position { get; set; } = null!;
        
    }
}
