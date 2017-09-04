using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using PizzeriaDelish.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PizzeriaDelish.Extensions
{
    public static class CheckoutExtensions
    {
        public static bool AddressIsEmpty(this ISession session)
        {
            return String.IsNullOrWhiteSpace(session.GetString("address"));
        }

        public static bool PhoneNumberIsEmpty(this ISession session)
        {
            return String.IsNullOrWhiteSpace(session.GetString("phoneNumber"));
        }

        public static void SerialiseAddress(this ISession session, Address address)
        {
            session.SetString("address", JsonConvert.SerializeObject(address, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }));
        }

        public static Address DeserialiseAddress(this ISession session)
        {
            return JsonConvert.DeserializeObject<Address>(session.GetString("address"));
        }
    }
}
