using Microsoft.EntityFrameworkCore;
using WebAPI.IRepository;
using WebAPI.Models;

namespace WebAPI.Data.Repository
{
    public class FurnishingTypeRepository : IFurnishingTypeRepository
    {
        private readonly DataContext _conContext;
        public FurnishingTypeRepository(DataContext conContext)
        {
            _conContext = conContext;
        }
        public async Task<IEnumerable<FurnishingType>> FurnishingTypeAsync()
        {
            return await _conContext.FurnishingTypes!.ToListAsync();
        }
    }
}