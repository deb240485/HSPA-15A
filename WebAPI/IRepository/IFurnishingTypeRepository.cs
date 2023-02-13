using WebAPI.Models;

namespace WebAPI.IRepository
{
    public interface IFurnishingTypeRepository
    {
        Task<IEnumerable<FurnishingType>> FurnishingTypeAsync();
    }
}