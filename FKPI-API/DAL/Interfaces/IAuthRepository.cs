using System.Collections.Generic;
using System.Threading.Tasks;
using FKPI_API.Models;

namespace FKPI_API.DAL
{
    public interface IAuthRepository
    {
        Task<User> Register(string username, string password);
        Task<User> Login(string username, string password);
        Task<bool> AdminExists();
    }
}