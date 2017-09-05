using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PizzeriaDelish.Models
{
    public class PaymentDetails
    {
        [Required(ErrorMessage = "Var god fyll i namn på kortet")]
        [Display(Name = "Namn på kortet")]
        public string NameOnCard { get; set; }

        [Required(ErrorMessage = "Var god fyll i kortnummer")]
        [RegularExpression(@"^(?:4[0-9]{12}(?:[0-9]{3})?|(?:5[1-5][0-9]{2}|222[1-9]|22[3-9][0-9]|2[3-6][0-9]{2}|27[01][0-9]|2720)[0-9]{12}|3[47][0-9]{13}|3(?:0[0-5]|[68][0-9])[0-9]{11}|6(?:011|5[0-9]{2})[0-9]{12}|(?:2131|1800|35\d{3})\d{11})$",
            ErrorMessage = "Var god fyll i ett giligt bank-/kreditkortsnummer")]
        [Display(Name = "Kortnummer")]
        public string CardNumber { get; set; }

        [Required(ErrorMessage = "Var god fyll i Cvc")]
        [RegularExpression(@"^\d{3}", ErrorMessage = "Cvc måste vara tre tecken")]
        [Display(Name = "Cvc")]
        public int Cvc { get; set; }

        [Required(ErrorMessage = "Var god fyll i kortets utgångsår")]
        [Range(2017, 2050, ErrorMessage = "Var god fyll i giltigt utgångsår")]
        [Display(Name = "Utgångsår")]
        public int YearOfExpiration { get; set; }

        [Required(ErrorMessage = "Var god fyll i månaden kortet utgår")]
        [Range(1, 12, ErrorMessage = "Var god fyll i månad (1-12)")]
        [Display(Name = "Utgångsmånad")]
        public int MonthOfExpiration { get; set; }

        [Required(ErrorMessage = "Var god fyll i gatuaddress")]
        [Display(Name = "Adress")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Var god fyll i ort")]
        [Display(Name = "Ort")]
        public string City { get; set; }

        [Required(ErrorMessage = "Var god fyll i postkod")]
        [RegularExpression(@"^\d{5}", ErrorMessage = "Postkoden måste vara fem siffror utan mellanslag (ex. 14144)")]
        [Display(Name = "Postkod")]
        public string PostalCode { get; set; }
    }
}
