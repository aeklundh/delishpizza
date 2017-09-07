using Microsoft.AspNetCore.Identity;
using PizzeriaDelish.Data;
using PizzeriaDelish.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PizzeriaDelish.Services
{
    public class AddressService
    {
        private readonly WebshopDbContext _context;

        public AddressService(WebshopDbContext context)
        {
            _context = context;
        }

        public async Task<Address> AddAddressAsync(Address address)
        {
            Address dbAddress = _context.Addresses.FirstOrDefault(
                                    x => x.City == address.City &&
                                    x.PostalCode == address.PostalCode &&
                                    x.StreetAddress == address.StreetAddress &&
                                    x.FirstName == address.FirstName &&
                                    x.Surname == address.Surname);

            if (dbAddress == null)
            {
                _context.Addresses.Add(address);
                await _context.SaveChangesAsync();

                return _context.Addresses.FirstOrDefault(
                    x => x.City == address.City &&
                    x.PostalCode == address.PostalCode &&
                    x.StreetAddress == address.StreetAddress &&
                    x.FirstName == address.FirstName &&
                    x.Surname == address.Surname);
            }
            else
            {
                return dbAddress;
            }
        }
    }
}