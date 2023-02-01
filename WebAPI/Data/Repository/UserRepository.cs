using Microsoft.EntityFrameworkCore;
using WebAPI.IRepository;
using WebAPI.Models;

namespace WebAPI.Data.Repository
{
    public class UserRepository: IUserRepository
    {
        private readonly DataContext _conContext;
        public UserRepository(DataContext conContext)
        {
            _conContext = conContext;
        }

        public async Task<User?> Authenticate(string userName, string password)
        {
            return await _conContext.Users!.FirstOrDefaultAsync(u => u.userName == userName && u.password == password);
        }
    }
}