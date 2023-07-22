using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VeterinaryClinic.Data.Entities;
using VeterinaryClinic.Helpers;
using VeterinaryClinic.Migrations;

namespace VeterinaryClinic.Data
{
    public class SeedDb
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;
        private Random _random;
        public SeedDb(DataContext context, IUserHelper userHelper)
        {
            _context= context;
            _userHelper= userHelper;
            _random = new Random();
        }
        public async Task SeedAsync()
        {
            await _context.Database.MigrateAsync();

            await _userHelper.CheckRoleAsync("Admin");

            await _userHelper.CheckRoleAsync("Customer");

            await _userHelper.CheckRoleAsync("Vet");

            await _userHelper.CheckRoleAsync("Anonymous");

            if (!_context.Countries.Any())
            {
                var cities = new List<City>();
                cities.Add(new City { Name = "Lisboa" });
                cities.Add(new City { Name = "Porto" });
                cities.Add(new City { Name = "Faro" });

                _context.Countries.Add(new Country
                {
                    Cities = cities,
                    Name = "Portugal"
                });

                await _context.SaveChangesAsync();
            }

            var userAdmin = await _userHelper.GetUserByEmailAsync("daiane.farias@cinel.pt");

            if (userAdmin == null)
            {
                userAdmin = new User
                {
                    FirstName = "Daiane",
                    LastName = "Farias",
                    Email = "daiane.farias@cinel.pt",
                    UserName = "daiane.farias@cinel.pt",
                    PhoneNumber = GenerateRandomNumbers(9),
                    Address = GenerateRandomAddress(),
                    CityId = _context.Countries.FirstOrDefault().Cities.FirstOrDefault().Id,
                    City = _context.Countries.FirstOrDefault().Cities.FirstOrDefault(),
                };

                var result = await _userHelper.AddUserAsync(userAdmin, "123456");

                if (result != IdentityResult.Success)
                {
                    throw new InvalidOperationException("Could not create the user in seeder");
                }
                await _userHelper.AddUserToRoleAsync(userAdmin, "Admin");

            }
            var userCustomer = await _userHelper.GetUserByEmailAsync("reinaldo.pires@cinel.pt");

            if (userCustomer == null)
            {
                userCustomer = new User
                {
                    FirstName = "Reinaldo",
                    LastName = "Pires",
                    UserName = "reinaldo.pires@cinel.pt",
                    Email = "reinaldo.pires@cinel.pt",
                    PhoneNumber = GenerateRandomNumbers(9),
                    Address = GenerateRandomAddress(),
                    CityId = _context.Countries.FirstOrDefault().Cities.FirstOrDefault().Id,
                    City = _context.Countries.FirstOrDefault().Cities.FirstOrDefault(),
                };
                var result = await _userHelper.AddUserAsync(userCustomer, "123456");

                if (result != IdentityResult.Success)
                {
                    throw new InvalidOperationException("Could not create the user in seeder");
                }
                await _userHelper.AddUserToRoleAsync(userCustomer, "Customer");

            }
            var userVet = await _userHelper.GetUserByEmailAsync("oliviaborba@cinel.pt");

            if (userVet == null)
            {
                userVet = new User
                {
                    FirstName = "Olivia",
                    LastName = "Borba",
                    UserName = "oliviaborba@cinel.pt",
                    Email = "oliviaborba@cinel.pt",
                    PhoneNumber = GenerateRandomNumbers(9),
                    Address = GenerateRandomAddress(),
                    CityId = _context.Countries.FirstOrDefault().Cities.FirstOrDefault().Id,
                    City = _context.Countries.FirstOrDefault().Cities.FirstOrDefault(),
                };
                var result = await _userHelper.AddUserAsync(userVet, "123456");

                if (result != IdentityResult.Success)
                {
                    throw new InvalidOperationException("Could not create the user in seeder");
                }
                await _userHelper.AddUserToRoleAsync(userVet, "Vet");

            }
            var userAnonymous = await _userHelper.GetUserByEmailAsync("rafael.alves@cinel.pt");

            if (userAnonymous == null)
            {
                userAnonymous = new User
                {
                    FirstName = "Rafael",
                    LastName = "Alves",
                    UserName = "rafael.alves@cinel.pt",
                    Email = "rafael.alves@cinel.pt",
                    PhoneNumber = GenerateRandomNumbers(9),
                    Address = GenerateRandomAddress(),
                    CityId = _context.Countries.FirstOrDefault().Cities.FirstOrDefault().Id,
                    City = _context.Countries.FirstOrDefault().Cities.FirstOrDefault(),
                };
                var result = await _userHelper.AddUserAsync(userAnonymous, "123456");

                if (result != IdentityResult.Success)
                {
                    throw new InvalidOperationException("Could not create the user in seeder");
                }
                await _userHelper.AddUserToRoleAsync(userAnonymous, "Vet");

            }

            var isInRole = await _userHelper.IsUserInRoleAsync(userAdmin, "Admin");
            var isInRoleCustomer = await _userHelper.IsUserInRoleAsync(userCustomer, "Customer");
            var isInRoleVet = await _userHelper.IsUserInRoleAsync(userVet, "Vet");
            var isInRoleAnonymous = await _userHelper.IsUserInRoleAsync(userAnonymous, "Anonymous");

            if (!isInRole)
            {
                await _userHelper.AddUserToRoleAsync(userAdmin, "Admin");

            }
            if (!isInRoleCustomer)
            {
                await _userHelper.AddUserToRoleAsync(userCustomer, "Customer");

            }
            if (!isInRoleVet)
            {
                await _userHelper.AddUserToRoleAsync(userVet, "Vet");

            }
            if (!isInRoleAnonymous)
            {
                await _userHelper.AddUserToRoleAsync(userAnonymous, "Anonymous");

            }

            if (!_context.Customers.Any())
            {
                AddCustomer("Carmelita Alves", userCustomer);
                AddCustomer("Reinaldo Pires", userCustomer);
                AddCustomer("Cecilia Borba", userCustomer);
                AddCustomer("Romeu Bezerra", userCustomer);
                AddCustomer("Clara Dias", userCustomer);

                await _context.SaveChangesAsync();
            }
            var customer = _context.Customers.FirstOrDefault();

            if (!_context.Pets.Any())
            {
                AddPet("Lola", customer);
                AddPet("Max", customer);
                AddPet("Zeus", customer);
                AddPet("Bob", customer);
                AddPet("Kiara", customer);

                await _context.SaveChangesAsync();
            }
          
            if (!_context.Vets.Any())
            {
                AddVet("Carlos Nobrega", userVet);
                AddVet("Maria Batista", userVet);
                AddVet("Francisco Magalhães", userVet);
                AddVet("Carla Nascimento", userVet);
                AddVet("Eduardo Santos", userVet);

                await _context.SaveChangesAsync();
            }

        }
        private void AddCustomer(string name, User user)
        {
            _context.Customers.Add(new Customer
            {
                Name = name,
                Document = GenerateRandomNumbers(9),
                Address = GenerateRandomAddress(),
                Phone = GenerateRandomNumbers(9),
                Email = name.Replace(" ", "_") + "@cinel.com",
                User = user

            });
        }

        private void AddPet(string name, Customer customer)
        {
            _context.Pets.Add(new Pet 
            {            
                Name = name,
                DateOfBirth= DateTime.Now,
                Type = GenerateRandomType(),
                Gender = GenerateRandomGender(),
                Customer = customer,           
            
            });
        }
        private void AddVet(string name, User user) 
        {
            _context.Vets.Add(new Vet
            {
                Name = name,
                Address = GenerateRandomAddress(),
                Phone = GenerateRandomNumbers(9),
                Email = name.Replace(" ", "_") + "@cinel.com",
                Room = GenerateRandomNumbers(2),
                User= user,
            });
        
        
        }

    

        private string GenerateRandomNumbers(int value)
        {
            string phoneNumber = "";
            for (int i = 0; i < value; i++)
            {
                phoneNumber += _random.Next(10).ToString();
            }
            return phoneNumber;
        }
        private string GenerateRandomAddress()
        {
            string[] streetSuffixes = { "Rua", "Avenida", "Praceta", "Calçada" };
            string[] streets = { "José", "Brasil", "Angola", "Carlos", "Rodrigues", "Tody", "Santos", "Silva", "Conceição", "Teles" };
            string number = _random.Next(1, 1000).ToString();
            string streetSuffix = streetSuffixes[_random.Next(streetSuffixes.Length)];
            string street = streets[_random.Next(streets.Length)];

            return $" {streetSuffix}: {street}, {number}";
        }
        private string GenerateRandomType()
        {
            string[] types = { "Dog", "Cat", "Hamster", "Guinea Pig", "Rabbit", "Chinchilla","Lizard"};
            string petType = types[_random.Next(types.Length)];

            return petType; 
        }
        private string GenerateRandomGender() 
        {
            string[] gender = { "Female", "Male" };
            string petGender = gender[_random.Next(gender.Length)];

            return petGender;
        }
        private string GenerateRandomCustomer()
        {
            string[] names = { "Olivia Santos ", "Romeo Alves", "Liz Silva", "Mariana Farias", "Felipe Santos", "Alice Marques", "Marcos Oliveira" };
            string customerName = names[_random.Next(names.Length)];

            return customerName;
        }

    }
}
