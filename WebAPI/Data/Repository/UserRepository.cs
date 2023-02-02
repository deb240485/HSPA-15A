using System.Security.Cryptography;
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

        public async Task<User?> Authenticate(string userName, string passwordText)
        {
            var user = await _conContext.Users!.FirstOrDefaultAsync(u => u.userName == userName);
            
            if(user == null || user.passwordKey == null)
               return null;

            if(!MatchPasswordHash(passwordText,user.password!,user.passwordKey!))
                return null;

            return user;
        }

        private bool MatchPasswordHash(string passwordText, byte[] password, byte[] passwordKey)
        {
            using (var hmac = new HMACSHA512(passwordKey))
            {
                var passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(passwordText));

                for(int i=0;i<passwordHash.Length;i++)
                {
                    if (passwordHash[i] != password[i])
                        return false;
                }

                return true;
            }
        }

        public void Register(string userName, string password)
        {
            byte[] passwordHash , passwordKey;

            using (var hmac = new HMACSHA512())
            {
                passwordKey = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }

            User user = new User();
            user.userName = userName;
            user.password = passwordHash;
            user.passwordKey = passwordKey;

            _conContext.Users!.Add(user);

        }

        public async Task<bool> UserAlreadyExists(string userName)
        {
            return await _conContext.Users!.AnyAsync(u => u.userName == userName);
        }


    }
}