using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace PizzeriaDelish.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        [Required, Display(Name = "Förnamn")]
        public string FirstName { get; set; }
        [Required, Display(Name = "Efternamn")]
        public string Surname { get; set; }
        [Required, MaxLength(200)]
        public string Address { get; set; }
        [Required, MaxLength(50)]
        public string City { get; set; }
        [Required, StringLength(5)]
        public string PostalCode { get; set; }
    }
}
