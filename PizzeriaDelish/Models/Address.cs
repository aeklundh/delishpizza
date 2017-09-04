using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PizzeriaDelish.Models
{
    public class Address
    {
        public int AddressId { get; set; }
        [Required(ErrorMessage = "Du måste skriva in ditt förnamn"), Display(Name = "Förnamn")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Du måste skriva in ditt efternamn"), Display(Name = "Efternamn")]
        public string Surname { get; set; }
        [Required(ErrorMessage = "Du måste skriva in din adress"), Display(Name = "Adress"), MaxLength(200)]
        public string StreetAddress { get; set; }
        [Required(ErrorMessage = "Du måste skriva in din ort"), Display(Name = "Ort"), MaxLength(50)]
        public string City { get; set; }
        [Required(ErrorMessage = "Du måste skriva i din postkod"), StringLength(5, MinimumLength = 5, ErrorMessage = "Postkoden måste vara fem tecken lång"), Display(Name = "Postkod")]
        public string PostalCode { get; set; }
    }
}
