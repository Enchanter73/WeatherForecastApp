using Application.Interfaces;
using Domain.Entities;
using Infastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infastructure.Repositories
{
    public class FavoriteRepository : IFavoriteRepository
    {
        private readonly AppDbContext _context;

        public FavoriteRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<City>> GetFavoriteCitiesAsync(int userId)
        {
            return await _context.Favorites
                .Where(f => f.UserId == userId)
                .Where(f => f.City != null)
                .Select(f => f.City)
                .ToListAsync();
        }

        public async Task AddFavoriteAsync(int userId, int cityId)
        {
            var favorite = new Favorite { UserId = userId, CityId = cityId };
            await _context.Favorites.AddAsync(favorite);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveFavoriteAsync(int userId, int cityId)
        {
            var favorite = await _context.Favorites
                .FirstOrDefaultAsync(f => f.UserId == userId && f.CityId == cityId);

            if (favorite != null)
            {
                _context.Favorites.Remove(favorite);
                await _context.SaveChangesAsync();
            }
        }
    }
}
