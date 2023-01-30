using WebAPI.Models;

namespace WebAPI.Data.Repository.IRepository
{
    public interface ICityRepository
    {
        Task<IEnumerable<City>> GetCitiesAsync();
        void AddCity(City city);
        void DeleteCity(int id);
        Task<bool> SaveAsync();
    }
}