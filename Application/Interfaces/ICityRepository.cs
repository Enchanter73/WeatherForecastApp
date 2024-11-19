
using Domain.Entities;

namespace Application.Interfaces
{
    public interface ICityRepository
    {
        Task<City> GetCityByNameAsync(string cityName);
    }
}
