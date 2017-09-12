using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
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

        public CheckoutService(WebshopDbContext context, IHttpContextAccessor contextAccessor)
        {
            _context = context;
            _session = contextAccessor.HttpContext.Session;
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
                PhoneNumber = GetPhoneNumberFromSession(),
                DishOrders = new List<DishOrder>()
            };
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            //create dishOrders and attach to order
            List<DishOrder> dishOrders = new List<DishOrder>();
            foreach (CartItem ci in cart)
            {
                Dish dish = _context.Dishes.Include(x => x.DishIngredients).ThenInclude(x => x.Ingredient).FirstOrDefault(x => x.DishId == ci.Dish.DishId);
                if (dish != null)
                {
                    DishOrder dishOrder = new DishOrder()
                    {
                        Amount = 1,
                        DishId = ci.Dish.DishId,
                        OrderId = order.OrderId,
                        DishOrderIngredients = new List<DishOrderIngredient>()
                    };

                    //create dishorderingredients
                    List<Ingredient> originalIngredients = new List<Ingredient>();
                    foreach (DishIngredient di in dish.DishIngredients)
                        originalIngredients.Add(di.Ingredient);

                    //compare original dish's ingredients with cartItem's
                    List<Ingredient> added = ci.Ingredients.Where(cii => !originalIngredients.Any(oii => cii.IngredientId == oii.IngredientId)).ToList();
                    List<Ingredient> removed = originalIngredients.Where(oii => !ci.Ingredients.Any(cii => oii.IngredientId == cii.IngredientId)).ToList();

                    removed.ForEach(x => dishOrder.DishOrderIngredients.Add(new DishOrderIngredient() {
                        IngredientId = x.IngredientId,
                        IsAdded = false
                    }));
                    added.ForEach(x => dishOrder.DishOrderIngredients.Add(new DishOrderIngredient() {
                        IngredientId = x.IngredientId,
                        IsAdded = true
                    }));

                    //find if there already is an identical dishOrder. If so, increase its count
                    DishOrder identical = dishOrders.FirstOrDefault(dshOrds =>
                        dshOrds.DishId == dish.DishId
                        && !dshOrds.DishOrderIngredients.Where(cii => !dishOrder.DishOrderIngredients.Any(di => cii.IngredientId == di.IngredientId)).Any()
                        && !dishOrder.DishOrderIngredients.Where(cii => !dshOrds.DishOrderIngredients.Any(di => cii.IngredientId == di.IngredientId)).Any()
                    );

                    if (identical != null)
                        identical.Amount++;
                    else
                        dishOrders.Add(dishOrder);
                }
            }

            order.DishOrders.ToList().AddRange(dishOrders);
            await _context.SaveChangesAsync();
        }
    }
}
