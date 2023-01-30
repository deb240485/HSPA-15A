namespace WebAPI.IRepository
{
    public interface IUnitOfWork
    {
        ICityRepository CityRepository { get; }

        Task<bool> SaveAsync();
    }
}