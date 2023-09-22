using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VeterinaryClinic.Data;
using VeterinaryClinic.Data.Entities;
using VeterinaryClinic.Models;

namespace VeterinaryClinic.Repositories
{
    public class CountryRepository : GenericRepository<Country>, ICountryRepository
    {
        private readonly DataContext _context;

        public CountryRepository(DataContext context) : base(context)
        {
            _context = context;
        }
        /// <summary>
        /// Método que adiciona as citys no country.
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Country with Cities</returns>
        public async Task AddCityAsync(CityViewModel model)
        {
            var country = await this.GetCountryWithCitiesAsync(model.CountryId);
            if (country == null)
            {
                return;
            }

            country.Cities.Add(new City { Name = model.Name });
            _context.Countries.Update(country);
            await _context.SaveChangesAsync();
        }
        /// <summary>
        /// Método que deleta a city.
        /// </summary>
        /// <param name="city"></param>
        /// <returns>Delete</returns>
        public async Task<int> DeleteCityAsync(City city)
        {
            var country = await _context.Countries
               .Where(c => c.Cities.Any(ci => ci.Id == city.Id))
               .FirstOrDefaultAsync();
            if (country == null)
            {
                return 0;
            }

            _context.Cities.Remove(city);
            await _context.SaveChangesAsync();
            return country.Id;
        }
        /// <summary>
        /// Método que retorna a city por id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>City</returns>
        public async Task<City> GetCityAsync(int id)
        {
            return await _context.Cities.FindAsync(id);
        }
        /// <summary>
        /// Método para preencher a combo cities.
        /// </summary>
        /// <param name="countryId"></param>
        /// <returns>Cities List</returns>
        public IEnumerable<SelectListItem> GetComboCities(int countryId)
        {
            var country = _context.Countries.Find(countryId);
            var list = new List<SelectListItem>();
            if (country != null)
            {
                list = _context.Cities.Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id.ToString(),

                }).OrderBy(l => l.Text).ToList();
                list.Insert(0, new SelectListItem
                {
                    Text = "Select a citie...",
                    Value = "0"

                });
            }

            return list;
        }
        /// <summary>
        /// Método para preencher a combo countries.
        /// </summary>
        /// <returns>Countries List</returns>
        public IEnumerable<SelectListItem> GetComboCountries()
        {
            var list = _context.Countries.Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Id.ToString(),

            }).OrderBy(l => l.Text).ToList();
            list.Insert(0, new SelectListItem
            {
                Text = "Select a country...",
                Value = "0"

            });

            return list;
        }
        /// <summary>
        /// Método que retona as cities.
        /// </summary>
        /// <returns></returns>
        public IQueryable GetCountriesWithCities()
        {
            return _context.Countries
          .Include(c => c.Cities)
          .OrderBy(c => c.Name);
        }
        /// <summary>
        /// Método que retorna o country por id.
        /// </summary>
        /// <param name="city"></param>
        /// <returns>Country</returns>
        public async Task<Country> GetCountryAsync(City city)
        {
            return await _context.Countries
                .Where(c => c.Cities.Any(ci => ci.Id == city.Id))
                .FirstOrDefaultAsync();
        }
        /// <summary>
        /// Método que retorna country com as cities por id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Countries with cities</returns>
        public async Task<Country> GetCountryWithCitiesAsync(int id)
        {
            return await _context.Countries
                .Include(c => c.Cities)
                .Where(c => c.Id == id)
                .FirstOrDefaultAsync();
        }
        /// <summary>
        /// Método para editar a City.
        /// </summary>
        /// <param name="city"></param>
        /// <returns>City</returns>
        public async Task<int> UpdateCityAsync(City city)
        {
            var country = await _context.Countries
                .Where(c => c.Cities.Any(ci => ci.Id == city.Id)).FirstOrDefaultAsync();
            if (country == null)
            {
                return 0;
            }

            _context.Cities.Update(city);
            await _context.SaveChangesAsync();
            return country.Id;
        }
    }
}
