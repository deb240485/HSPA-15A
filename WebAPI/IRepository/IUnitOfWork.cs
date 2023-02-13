namespace WebAPI.IRepository
{
    public interface IUnitOfWork
    {
        ICityRepository CityRepository { get; }

        IUserRepository UserRepository { get; }

        IPropertyRepository propertyRepository { get; } 

        IPropertyTypeRepository propertyTypeRepository { get; }

        IFurnishingTypeRepository furnishingTypeRepository { get; }

        Task<bool> SaveAsync();
    }
}