using Microsoft.EntityFrameworkCore;
using WebAPI.IRepository;
using WebAPI.Models;

namespace WebAPI.Data.Repository
{
    public class PropertyTypeRepository : IPropertyTypeRepository
    {
        private readonly DataContext _conContext;
        public PropertyTypeRepository(DataContext conContext)
        {
            _conContext = conContext;
        }
        public async Task<IEnumerable<PropertyType>> GetPropertyTypeAsync()
        {
            return await _conContext.PropertyTypes!.ToListAsync();
        }
    }
}