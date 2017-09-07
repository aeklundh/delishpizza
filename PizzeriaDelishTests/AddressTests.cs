using PizzeriaDelish.Data;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using PizzeriaDelish.Models;
using Xunit;
using PizzeriaDelish.Services;

namespace PizzeriaDelishTests
{
    public class AddressTests : BaseTest
    {
        private WebshopDbContext _context { get; set; }

        protected override void InitialiseDatabase()
        {
            _context = _serviceProvider.GetService<WebshopDbContext>();

            List<Address> addresses = new List<Address>()
            {
                new Address() { City = "Stockholm", StreetAddress = "Storstorevägen 3", PostalCode="12345", FirstName = "Egon", Surname = "Söderblombom" },
                new Address() { City = "Turku", StreetAddress = "Åbovägen 55A", PostalCode="54321", FirstName = "Ålmar", Surname = "Ulman" }
            };
            _context.Addresses.AddRange(addresses);
            _context.SaveChanges();
        }

        [Fact]
        public async void Add_Address()
        {
            //Assemble
            AddressService addressService = _serviceProvider.GetService<AddressService>();
            Address expected = new Address() { City = "AAA", StreetAddress = "Åbovägen 55A", PostalCode = "54321", FirstName = "Ålmar", Surname = "Ulman" };
            
            //Act
            Address actual1 = await addressService.AddAddressAsync(expected);
            Address actual2 = await addressService.AddAddressAsync(expected);

            //Assert
            Assert.True(actual1.AddressId == 3);
            Assert.True(actual1.AddressId == actual2.AddressId);
        }
    }
}
