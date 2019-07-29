using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using FKPI_API.Models;

namespace FKPI_API.DAL
{
    public interface IAccountRepository
    {
        Task<IEnumerable<Account>> GetAsync(Expression<Func<Account, bool>> predicate);
        Task<IEnumerable<Account>> ChildrenAsync(int? AccountId);
        Task<Account> FindAsync(int AccountId);
    }
}