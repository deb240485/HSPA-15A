using WebAPI.Models;

namespace WebAPI.IRepository
{
    public interface IPropertyRepository
    {
        Task<IEnumerable<Property>>GetPropertiesAsync(int sellRent);

        void AddProperty (Property property);

        void DeleteProperty(int id);
    }
}