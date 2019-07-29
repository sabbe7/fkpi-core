using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using FKPI_API.Models;
using Microsoft.EntityFrameworkCore;

namespace FKPI_API.DAL
{
    public class AccountRepository : IAccountRepository
    {
        private readonly ApplicationDbContext context;

        public AccountRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<Account>> GetAsync(Expression<Func<Account, bool>> predicate)
        {
            return await context.Account
                .AsNoTracking()
                .Where(predicate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Account>> ChildrenAsync(int? accountId)
        {
            return await context.Account
                .AsNoTracking()
                .Where(x => x.ParentAccountId == accountId)
                .ToListAsync();
        }
        
        public async Task<Account> FindAsync(int AccountId)
        {
            return await context.Account.FirstOrDefaultAsync(x => x.AccountId == AccountId);
        }
    }
}