﻿using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VeterinaryClinic.Data;
using VeterinaryClinic.Data.Entities;
using VeterinaryClinic.Helpers;
using VeterinaryClinic.Models;

namespace VeterinaryClinic.Repositories
{
    public class PetRepository : GenericRepository<Pet>, IPetRepository
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;
        private readonly IConverterHelper _converterHelper;
        private readonly IBlobHelper _blobHelper;

        public PetRepository(DataContext context, 
            IUserHelper userHelper,
            IConverterHelper converterHelper,
            IBlobHelper blobHelper) : base(context)
        {
            _context = context;
            _userHelper = userHelper;
            _converterHelper = converterHelper;
            _blobHelper = blobHelper;
        }

        public IQueryable GetAllWithCustomers()
        {
            return _context.Pets.Include(p => p.Customer);
        }

        public IEnumerable<SelectListItem> GetComboPets()
        {
            var list = _context.Pets.Select(p => new SelectListItem
            {
                Text = p.Name,
                Value = p.Id.ToString(),

            }).ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "(Select your Pet...)",
                Value = "0"
            });

            return list;
        }
        public IEnumerable<SelectListItem> GetComboCustomers()
        {
            var list = _context.Customers.Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Id.ToString(),

            }).ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "(Select the Customer...)",
                Value = "0"
            });

            return list;
        }

        public IQueryable GetCustomerName()
        {
            return _context.Pets.Include(p => p.Customer);
        }
        public async Task AddCustomerToPetAsync(PetViewModel model, string userName)
        {
            Guid imageId = Guid.Empty;

            if (model.ImageFile != null && model.ImageFile.Length > 0)
            {
                imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "pets");

            }
            var pet = _converterHelper.ToPet(model, imageId, true);

            var user = await _userHelper.GetUserByEmailAsync(userName);
            if (user == null)
            {
                return;

            }
            var customer = await _context.Customers.FindAsync(model.CustomerId);
            if (customer == null)
            {
                return;
            }
            var petIndex = await _context.Pets
                .Where(p => p.Customer.User == user && p.Customer.Id == customer.Id)
                .FirstOrDefaultAsync();
          

            if (petIndex == null)
            {
                petIndex = new PetViewModel
                {
                    ImageId = imageId,
                    Id = model.Id,
                    Name = model.Name,
                    DateOfBirth = model.DateOfBirth,
                    Type = model.Type,
                    Gender = model.Gender,
                    Customer = customer,
                    CustomerId = customer.Id,
                };
                _context.Pets.Add(petIndex);
            }
            else
            {
                _context.Pets.Update(petIndex);

            }
            await _context.SaveChangesAsync();
        }
    }
}
