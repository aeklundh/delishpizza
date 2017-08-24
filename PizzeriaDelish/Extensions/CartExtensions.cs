using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using PizzeriaDelish.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PizzeriaDelish.Extensions
{
    public static class CartExtensions
    {
        public static bool CartIsEmpty(this ISession session)
        {
            return String.IsNullOrWhiteSpace(session.GetString("cart"));
        }

        public static void SerialiseCart(this ISession session, ICollection<CartItem> cart)
        {
            session.SetString("cart", JsonConvert.SerializeObject(cart));
        }

        public static List<CartItem> DeserialiseCart(this ISession session)
        {
            return JsonConvert.DeserializeObject<List<CartItem>>(session.GetString("cart"));
        }
    }
}
