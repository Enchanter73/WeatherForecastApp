using Application.Interfaces;
using Domain.Entities;
using Infastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infastructure.Repositories
{
    public class CityRepository : ICityRepository
    {
        private readonly AppDbContext _context;

        public CityRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<City> GetCityByNameAsync(string cityName)
        {
            return await _context.Cities.FirstOrDefaultAsync(c => c.CityName == cityName);
        }

        public async Task<List<City>> GetAllCitiesAsync()
        {
            return await _context.Cities.ToListAsync();
        }

        public async Task AddCityAsync(City city)
        {
            await _context.Cities.AddAsync(city);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateCityTemperatureAsync(City city)
        {
            _context.Cities.Update(city);
            await _context.SaveChangesAsync();
        }
    }
}
