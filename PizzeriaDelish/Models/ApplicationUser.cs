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
        [Display(Name = "Användarnamn")]
        public override string UserName { get; set; }
        [Required(ErrorMessage = "Du måste ange ditt mobilnummer"), DataType(DataType.PhoneNumber), Display(Name = "Telefonnummer"),
            RegularExpression(@"(\+?46|0)7\d{8}", ErrorMessage = "Var god lägg till ett giltigt mobiltelefonnummer")]
        public override string PhoneNumber { get; set; }
        [Required]
        public int AddressId { get; set; }

        public Address Address { get; set; }
    }
}
