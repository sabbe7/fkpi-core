using System;
using System.Threading.Tasks;
using FKPI_API.Core;
using SqlKata.Execution;

namespace FKPI_API.DAL
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext context;
        private readonly QueryFactory db;
        private readonly QueryParser queryParser;
        private AuthRepository authRepository;
        private AccountRepository accountRepository;
        private KPIRepository kpiRepository;

        public IKPIRepository KPIRepository
        {
            get
            {
                if (this.kpiRepository == null)
                {
                    this.kpiRepository = new KPIRepository(context, db, queryParser);
                }
                return kpiRepository;
            }
        }

        public IAuthRepository AuthRepository
        {
            get
            {
                if (this.authRepository == null)
                {
                    this.authRepository = new AuthRepository(context);
                }
                return authRepository;
            }
        }

        public IAccountRepository AccountRepository
        {
            get
            {
                if (this.accountRepository == null)
                {
                    this.accountRepository = new AccountRepository(context);
                }
                return accountRepository;
            }
        }

        public UnitOfWork(ApplicationDbContext context, QueryFactory db, QueryParser formulaParser)
        {
            this.context = context;
            this.db = db;
            this.queryParser = formulaParser;
        }

        public async Task<int> SaveAsync()
        {
            return await context.SaveChangesAsync();
        }

        private bool disposed = false;

        public void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}