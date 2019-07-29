using System.Collections.Generic;
using System.Threading.Tasks;
using FKPI_API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FKPI_API.DAL
{
    public class AuthRepository : IAuthRepository
    {
        private readonly ApplicationDbContext context;

        public AuthRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<User> Login(string username, string password)
        {
            var user = await context.User.FirstOrDefaultAsync(x => x.Username == username);

            if (user == null)
            {
                return null;
            }

            if (!verifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
            {
                return null;
            }

            return user;
        }

        public async Task<User> Register(string username, string password)
        {
            byte[] passwordHash, passwordSalt;
            createPasswordHash(password, out passwordHash, out passwordSalt);

            var user = new User()
            {
                Username = username,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt
            };

            await context.User.AddAsync(user);
            await context.SaveChangesAsync();

            return user;
        }

        public async Task<bool> AdminExists()
        {
            var admin = await context.User.FirstOrDefaultAsync(x => x.Username.ToLower() == "admin");

            return admin != null;
        }

        private bool verifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != passwordHash[i])
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        private void createPasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
    }
}