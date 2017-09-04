using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PizzeriaDelish.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        [Required]
        public int AddressId { get; set; }
        [Required]
        public DateTime OrderPlaced { get; set; }
        [Required]
        public bool Active { get; set; }
        [Required]
        public bool PaidByCard { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        public string UserId { get; set; }

        public Address DeliveryAddress { get; set; }
        public ICollection<DishOrder> DishOrders { get; set; }
    }
}
