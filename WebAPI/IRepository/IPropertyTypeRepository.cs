using WebAPI.Models;

namespace WebAPI.IRepository
{
    public interface IPropertyTypeRepository
    {
        Task<IEnumerable<PropertyType>> GetPropertyTypeAsync();
    }
}