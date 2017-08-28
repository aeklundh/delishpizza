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
        [Required(ErrorMessage = "Du måste skriva in ditt förnamn"), Display(Name = "Förnamn")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Du måste skriva in ditt efternamn"), Display(Name = "Efternamn")]
        public string Surname { get; set; }
        [Required(ErrorMessage = "Du måste skriva in din adress"), Display(Name = "Adress"), MaxLength(200)]
        public string Address { get; set; }
        [Required(ErrorMessage = "Du måste skriva in din ort"), Display(Name = "Ort"), MaxLength(50)]
        public string City { get; set; }
        [Required(ErrorMessage = "Postkoden måste vara fem tecken lång"), Display(Name = "Postkod"), StringLength(5, ErrorMessage = "Postkoden måste vara fem tecken lång")]
        public string PostalCode { get; set; }
    }
}
