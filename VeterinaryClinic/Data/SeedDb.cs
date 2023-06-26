using Microsoft.AspNetCore.Identity;
using System;
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
            await _context.Database.EnsureCreatedAsync();

            var user = await _userHelper.GetUserByEmailAsync("daiane.farias@cinel.pt");

            if (user == null)
            {
                user = new User
                {
                    FirstName = "Daiane",
                    LastName = "Farias",
                    Email = "daiane.farias@cinel.pt",
                    UserName = "daiane.farias@cinel.pt",
                    PhoneNumber = GenerateRandomNumbers(9),
                };

                var result = await _userHelper.AddUserAsync(user, "123456");

                if (result != IdentityResult.Success)
                {
                    throw new InvalidOperationException("Could not create the user in seeder");
                }

            }
            if(!_context.Customers.Any())
            {
                AddCustomer("Carmelita Alves", user);
                AddCustomer("Reinaldo Pires", user);
                AddCustomer("Cecilia Borba", user);
                AddCustomer("Romeu Bezerra", user);
                AddCustomer("Clara Dias", user);

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
                AddVet("Carlos Nobrega", user);
                AddVet("Maria Batista", user);
                AddVet("Francisco Magalhães", user);
                AddVet("Carla Nascimento", user);
                AddVet("Eduardo Santos", user);


                await _context.SaveChangesAsync();
            }

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

        private void AddCustomer(string name, User user)
        {
            _context.Customers.Add(new Customer
            {
                Name = name,
                Document = GenerateRandomNumbers(9),
                Address = GenerateRandomAddress(),
                Phone = GenerateRandomNumbers(9),
                Email = name.Replace(" ", "_") + "@cinel.com",
                User= user

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
