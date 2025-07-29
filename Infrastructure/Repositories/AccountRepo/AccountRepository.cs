using Infrastructure.Data;
using Infrastructure.Entities;
using Infrastructure.Repositories.Generic;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.AccountRepo
{
    public class AccountRepository : GenericRepository<Account>, IAccountRepository
    {
        public AccountRepository(AppDbContext context) : base(context) { }

        public async Task<Account?> GetByAccountIdAsync(Guid accountId)
        {
            return await _context.Accounts.FirstOrDefaultAsync(a => a.Id == accountId);
        }

        public async Task<List<Account>> GetWalletsByUserIdAsync(Guid userId)
        {
            return await _context.Accounts.Where(a => a.UserId == userId).ToListAsync();
        }
       


        public Task UpdateBalanceAsync(Account account)
        {
            _context.Attach(account);
            _context.Entry(account).Property(a => a.Balance).IsModified = true;
            return Task.CompletedTask;
        }
    }
}
