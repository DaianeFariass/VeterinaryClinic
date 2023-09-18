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
    public class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;
        private readonly IConverterHelper _converterHelper;
        private readonly IBlobHelper _blobHelper;

        public EmployeeRepository(DataContext context,
            IUserHelper userHelper,
            IConverterHelper converterHelper,
            IBlobHelper blobHelper) : base(context)
        {
            _context = context;
            _userHelper = userHelper;
            _converterHelper = converterHelper;
            _blobHelper = blobHelper;
        }

        public async Task AddRoleToEmployeeAsync(EmployeeViewModel model, string userName)
        {
            Guid imageId = Guid.Empty;

            if (model.ImageFile != null && model.ImageFile.Length > 0)
            {
                imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "employees");

            }
            var vet = _converterHelper.ToEmployee(model, imageId, true);
            var user = await _userHelper.GetUserByEmailAsync(userName);
            if (user == null)
            {
                return;

            }       
            var employeeIndex = await _context.Employees
                .Where(v => v.User == user)
                .FirstOrDefaultAsync();

            employeeIndex = new EmployeeViewModel
            {
                ImageId = imageId,
                Id = model.Id,
                Name = model.Name,
                Address = model.Address,
                Phone = model.Phone,
                Email = model.Email,
                Role = model.RoleId,
                RoleId = model.RoleId,
                User = user,

            };
            _context.Employees.Add(employeeIndex);

            await _context.SaveChangesAsync();
        }

        public IQueryable GetAllWithUsers()
        {
            return _context.Employees.Include(v => v.User);
        }

        public IEnumerable<SelectListItem> GetComboEmployess()
        {
            var list = _context.Employees.Select(p => new SelectListItem
            {
                Text = p.Name,
                Value = p.Id.ToString(),

            }).ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "(Select the Employee...)",
                Value = "0"
            });

            return list;
        }

        public IEnumerable<SelectListItem> GetComboRoles()
        {
            var model = new EmployeeViewModel
            {
                Roles = new List<SelectListItem>
                {
                    new SelectListItem{Text = "Select the Role...",Value = "" },
                    new SelectListItem{Text = "Director", Value = "Director"},
                    new SelectListItem{Text = "Admin", Value = "Admin"},
                    new SelectListItem{Text = "Vet", Value = "Vet"},
                    new SelectListItem{Text = "Assistant", Value = "Assistant"},
                    new SelectListItem{Text = "Receptionist", Value = "Receptionist"},
      
                },
            };
            return model.Roles;
        }
    }
}
