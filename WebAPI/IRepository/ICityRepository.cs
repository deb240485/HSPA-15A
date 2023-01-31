using WebAPI.Models;

namespace WebAPI.IRepository
{
    public interface ICityRepository
    {
        Task<IEnumerable<City>> GetCitiesAsync();
        void AddCity(City city);
        void DeleteCity(int id);
        Task<City?> FindCity(int id);
    }
}