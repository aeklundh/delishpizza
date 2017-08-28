using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PizzeriaDelish.Models.AccountViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Du måste ange ditt mobilnummer"),
            DataType(DataType.PhoneNumber),
            Display(Name = "Telefonnummer"),
            RegularExpression(@"(\+?46|0)7\d{8}", ErrorMessage = "Var god lägg till ett giltigt mobiltelefonnummer")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Du måste skriva in ditt förnamn"),
            Display(Name = "Förnamn")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Du måste skriva in ditt efternamn"),
            Display(Name = "Efternamn")]
        public string Surname { get; set; }

        [Required(ErrorMessage = "Du måste skriva in din adress"),
            Display(Name = "Adress"),
            MaxLength(200)]
        public string Address { get; set; }

        [Required(ErrorMessage = "Du måste skriva in din ort"),
            Display(Name = "Ort"),
            MaxLength(50)]
        public string City { get; set; }

        [Required(ErrorMessage = "Postkoden måste vara fem tecken lång"),
            StringLength(5, MinimumLength = 5, ErrorMessage = "Postkoden måste vara fem tecken lång"),
            Display(Name = "Postkod")]
        public string PostalCode { get; set; }
    }
}
