using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PizzeriaDelish.Models.CheckoutViewModels
{
    public class OrderVerificationViewModel
    {
        public Address DeliveryAddress { get; set; }
        public List<CartItem> Cart { get; set; }
        [Display(Name = "Telefonnummer")]
        public string PhoneNumber { get; set; }
    }
}
