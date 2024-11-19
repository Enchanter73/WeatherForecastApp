using Domain.Entities;

namespace Application.Interfaces
{
    public interface IFavoriteRepository
    {
        Task<List<City>> GetFavoriteCitiesAsync(int userId);
        Task AddFavoriteAsync(int userId ,int cityId);
        Task RemoveFavoriteAsync(int userId, int cityId);
    }
}
