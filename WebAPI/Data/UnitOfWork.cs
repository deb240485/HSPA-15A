using WebAPI.Data.Repository;
using WebAPI.IRepository;

namespace WebAPI.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataContext _conContext;

        public UnitOfWork(DataContext conContext)
        {
            _conContext = conContext;
        }

        public ICityRepository CityRepository => new CityRepository(_conContext);

        public IUserRepository UserRepository => new UserRepository(_conContext);

        public IPropertyRepository propertyRepository => new PropertyRepository(_conContext);

        public IPropertyTypeRepository propertyTypeRepository => new PropertyTypeRepository(_conContext);

        public IFurnishingTypeRepository furnishingTypeRepository => new FurnishingTypeRepository(_conContext);

        public async Task<bool> SaveAsync()
        {
            return await _conContext.SaveChangesAsync() > 0;
        }
    }
}