using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using PizzeriaDelish.Data;
using PizzeriaDelish.Extensions;
using PizzeriaDelish.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PizzeriaDelish.Services
{
    public class CheckoutService
    {
        private readonly WebshopDbContext _context;
        private readonly ISession _session;

        public CheckoutService(WebshopDbContext context, IServiceProvider serviceProvider)
        {
            _context = context;
            _session = serviceProvider.GetRequiredService<IHttpContextAccessor>()?.HttpContext.Session;
        }

        public void SetAddressInSession(Address address)
        {
            _session.SerialiseAddress(address);
        }

        public Address GetAddressFromSession()
        {
            return _session.DeserialiseAddress();
        }

        public void SetPhoneNumberInSession(string phoneNumber)
        {
            _session.SetString("phoneNumber", phoneNumber);
        }

        public string GetPhoneNumberFromSession()
        {
            return _session.GetString("phoneNumber");
        }

        public async Task CreateOrderAsync(List<CartItem> cart, Address dbAddress, bool paidByCard)
        {
            //create a new order and add it to the db
            Order order = new Order()
            {
                AddressId = dbAddress.AddressId,
                PaidByCard = paidByCard, 
                OrderPlaced = DateTime.Now,
                Active = true,
                PhoneNumber = GetPhoneNumberFromSession()
            };
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            //create dishOrders and attach to order

            throw new NotImplementedException();
        }
    }
}
