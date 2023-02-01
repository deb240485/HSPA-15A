using WebAPI.Models;

namespace WebAPI.IRepository
{
    public interface IUserRepository
    {
        Task<User?> Authenticate(string userName, string password);
    }
}