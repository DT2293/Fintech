using Infrastructure.Entities;
using Infrastructure.Repositories.Generic;

namespace Infrastructure.Repositories.AccountRepo
{
    public interface IAccountRepository : IGenericRepository<Account>
    {
        Task<Account?> GetByAccountIdAsync(Guid accountId);
        Task<List<Account>> GetWalletsByUserIdAsync(Guid userId);
        Task UpdateBalanceAsync(Account account);
    }
}
