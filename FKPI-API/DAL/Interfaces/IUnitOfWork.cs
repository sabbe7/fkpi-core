using System;
using System.Threading.Tasks;
using FKPI_API.Models;

namespace FKPI_API.DAL
{
    public interface IUnitOfWork : IDisposable
    {
        IAuthRepository AuthRepository { get; }
        IAccountRepository AccountRepository { get; }
        IKPIRepository KPIRepository { get; }

        Task<int> SaveAsync();
        void Dispose(bool disposing);
    }
}