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
                return RedirectToAction(nameof(DeliveryDetails));
            }
            else
            {
                return RedirectToAction("Index", nameof(HomeController));
            }
        }

        public async Task<IActionResult> DeliveryDetails()
        {
            DeliveryDetailsViewModel vm = new DeliveryDetailsViewModel();
            if (_signInManager.IsSignedIn(User))
            {
                ApplicationUser user = await _userManager.GetUserAsync(User);
                user.Address = _context.Addresses.FirstOrDefault(x => x.AddressId == user.AddressId);
                vm.Address = user.Address;
                vm.PhoneNumber = user.PhoneNumber;
            }
            
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeliveryDetails(DeliveryDetailsViewModel vm)
        {
            if (ModelState.IsValid)
            {
                _checkoutService.SetAddressInSession(vm.Address);
                _checkoutService.SetPhoneNumberInSession(vm.PhoneNumber);

                return RedirectToAction(nameof(OrderVerification));
            }
            else
            {
                return View();
            }
        }

        public IActionResult OrderVerification()
        {
            List<CartItem> cart = _cartService.GetCart();
            string phoneNumber = _checkoutService.GetPhoneNumberFromSession();
            Address address = _checkoutService.GetAddressFromSession();

            if (cart == null || String.IsNullOrEmpty(phoneNumber) || address == null)
            {
                return RedirectToAction("Index", "Home");
            }

            OrderVerificationViewModel vm = new OrderVerificationViewModel()
            {
                Cart = cart,
                PhoneNumber = phoneNumber,
                DeliveryAddress = address
            };

            return View(vm);
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
                return View("OrderConfirmation");
            }
            else
            {
                return View("CardDetails");
            }
        }

        public async Task<IActionResult> PaymentChoice(bool payByCard)
        {
            if (payByCard)
            {
                return View("CardDetails");
            }
            else
            {
                await CreateOrderAsync(false);
                return View("OrderConfirmation");
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
            _cartService.EmptyCart();
        }
    }
}