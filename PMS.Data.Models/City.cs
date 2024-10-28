using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace PMS.Data.Models
{
    public class City
    {
        public City() 
        {
            this.CityId = Guid.NewGuid();   
        }

        public Guid CityId { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        [Required]
        [Comment("Date when created on")]
        public DateTime CreatedOn { get; set; }

        [Required]
        [Comment("Date when last edited")]
        public DateTime EditedOn { get; set; }


        public ICollection<Supplier> Suppliers { get; set; } = new List<Supplier>();    
    }
}