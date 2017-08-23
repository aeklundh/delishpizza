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
        public Guid UserId { get; set; }
        [Required]
        public DateTime OrderPlaced { get; set; }
        [Required]
        public bool Active { get; set; }

        public Order()
        {
            OrderPlaced = DateTime.Now;
            Active = true;
        }

        public ApplicationUser User { get; set; }
        public ICollection<DishOrder> DishOrders { get; set; }
    }
}
