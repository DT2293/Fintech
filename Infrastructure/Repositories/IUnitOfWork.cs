using Infrastructure.Entities;
using Infrastructure.Repositories.AccountRepo;
using Infrastructure.Repositories.Generic;

namespace Infrastructure.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
       IGenericRepository<WalletTransaction> Transactions { get; }
        //  ITransactionRepository Transactions { get; }
        public AccountRepository Accounts { get; }
        Task BeginTransactionAsync();
        Task CommitAsync();
        Task RollbackAsync();
        Task<int> SaveChangesAsync();
    }
}
