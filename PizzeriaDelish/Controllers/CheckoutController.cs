using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using PizzeriaDelish.Models;
using PizzeriaDelish.Data;
using PizzeriaDelish.Extensions;
using PizzeriaDelish.Services;
using PizzeriaDelish.Models.CheckoutViewModels;

namespace PizzeriaDelish.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly WebshopDbContext _context;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AddressService _addressService;
        private readonly CartService _cartService;
        private readonly CheckoutService _checkoutService;

        public CheckoutController(WebshopDbContext context, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, AddressService addressService, CartService cartService, CheckoutService checkoutService)
        {
            _context = context;
            _signInManager = signInManager;
            _userManager = userManager;
            _addressService = addressService;
            _cartService = cartService;
            _checkoutService = checkoutService;
        }

        public IActionResult Index()
        {
            if (!HttpContext.Session.CartIsEmpty())
            {
                return View(HttpContext.Session.DeserialiseCart());
            }
            else
            {
                return RedirectToAction("Index", nameof(HomeController));
            }
        }

        public async Task<IActionResult> DeliveryDetails()
        {
            ApplicationUser user = null;
            if (_signInManager.IsSignedIn(User))
            {
                user = await _userManager.GetUserAsync(User);
            }
            
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeliveryDetails(DeliveryDetailsViewModel vm)
        {
            if (ModelState.IsValid)
            {
                _checkoutService.SetAddressInSession(vm.Address);
                _checkoutService.SetPhoneNumberInSession(vm.PhoneNumber);

                return RedirectToAction(nameof(Payment));
            }
            else
            {
                return View();
            }
        }

        public IActionResult Payment()
        {
            if (_checkoutService.GetAddressFromSession() == null || String.IsNullOrEmpty(_checkoutService.GetPhoneNumberFromSession()))
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Payment(PaymentDetails details)
        {
            if (ModelState.IsValid)
            {
                //verify payment with 3rd party for total:
                int total = _cartService.CalculateTotalCartPrice(_cartService.GetCart());

                //if valid payment
                await CreateOrderAsync(true);
                return View("OrderConfirmed");
            }
            else
            {
                return RedirectToAction(nameof(Payment));
            }
        }

        public async Task<IActionResult> PaymentChoice(bool payByCard)
        {
            if (payByCard)
            {
                return PartialView("_CardDetailsInputPartial");
            }
            else
            {
                await CreateOrderAsync(false);
                return View("OrderConfirmed");
            }
        }

        private async Task CreateOrderAsync(bool payByCard)
        {
            //get cart, delivery address, phone number
            List<CartItem> cart = _cartService.GetCart();

            //fetch address from temporary storage and create a db entry
            Address address = _checkoutService.GetAddressFromSession();
            address = await _addressService.AddAddressAsync(address);

            await _checkoutService.CreateOrderAsync(cart, address, payByCard);
        }
    }
}