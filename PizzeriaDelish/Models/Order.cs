using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PizzeriaDelish.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public Guid UserId { get; set; }
        public DateTime OrderPlaced { get; set; }
        public bool Active { get; set; }

        public ApplicationUser User { get; set; }
        public ICollection<DishOrder> DishOrders { get; set; }
    }
}
