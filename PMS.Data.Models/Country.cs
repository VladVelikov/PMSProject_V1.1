﻿using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace PMS.Data.Models
{
    public class Country
    {
        public Country() 
        {
            this.CountryId = Guid.NewGuid();
        }    

        public Guid CountryId { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        [Required]
        [Comment("Date when created on")]
        public DateTime CreatedOn { get; set; }

        [Required]
        [Comment("Date when last edited")]
        public DateTime EditedOn { get; set; }


        public virtual ICollection<Supplier> Suppliers { get; set; } = new List<Supplier>();    
    }
}