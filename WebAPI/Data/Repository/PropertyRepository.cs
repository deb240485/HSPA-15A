using Microsoft.EntityFrameworkCore;
using WebAPI.IRepository;
using WebAPI.Models;

namespace WebAPI.Data.Repository
{
    public class PropertyRepository : IPropertyRepository
    {
        private readonly DataContext _conContext;
        public PropertyRepository(DataContext conContext)
        {
            _conContext = conContext;
        }
        public void AddProperty(Property property)
        {
            throw new NotImplementedException();
        }

        public void DeleteProperty(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Property>> GetPropertiesAsync(int sellRent)
        {
            var properties = await _conContext.Properties!
            .Include(p => p.PropertyType)
            .Include(p => p.City)
            .Include(p => p.FurnishingType)
            .Where(p => p.SellRent == sellRent)
            .ToListAsync();
            return properties;
        }
    }
}