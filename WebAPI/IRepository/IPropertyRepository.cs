using WebAPI.Models;

namespace WebAPI.IRepository
{
    public interface IPropertyRepository
    {
        Task<IEnumerable<Property>>GetPropertiesAsync(int sellRent);

        Task<Property?> GetPropertyAsync(int id);

        void AddProperty (Property property);

        void DeleteProperty(int id);

        Task<Property?> GetPropertyPhotoAsync(int id);
    }
}