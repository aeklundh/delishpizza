using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PizzeriaDelish.Models.CheckoutViewModels
{
    public class DeliveryDetailsViewModel
    {
        [Required]
        public Address Address { get; set; }

        [Required(ErrorMessage = "Du måste ange ditt mobilnummer"),
            DataType(DataType.PhoneNumber),
            Display(Name = "Telefonnummer"),
            RegularExpression(@"(\+?46|0)7\d{8}",
            ErrorMessage = "Var god lägg till ett giltigt mobiltelefonnummer")]
        public string PhoneNumber { get; set; }
    }
}
