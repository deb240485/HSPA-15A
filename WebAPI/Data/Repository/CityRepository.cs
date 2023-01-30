using Microsoft.EntityFrameworkCore;
using WebAPI.IRepository;
using WebAPI.Models;

namespace WebAPI.Data.Repository
{
    public class CityRepository : ICityRepository
    {
        private readonly DataContext _conContext;
        public CityRepository(DataContext conContext)
        {
            _conContext = conContext;
        }
        public void AddCity(City city)
        {
           _conContext.Cities!.AddAsync(city);
        }

        public void DeleteCity(int id)
        {
            var city = _conContext.Cities!.Find(id);
            _conContext.Cities.Remove(city!);
        }

        public async Task<IEnumerable<City>> GetCitiesAsync()
        {
            return await _conContext.Cities!.ToListAsync();
        }
    }
}