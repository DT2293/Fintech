using System.Security.Claims;
using Infrastructure.Entities;
using Infrastructure.Repositories.AccountRepo;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Services
{
    public class AccountWalletService
    {
        private readonly AccountRepository _accountRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AccountWalletService(AccountRepository accountRepository,
                                     IHttpContextAccessor httpContextAccessor)
        {
            _accountRepository = accountRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<List<Account>> GetAllWalletsByCurrentUserAsync()
        {
            var user = _httpContextAccessor.HttpContext?.User;
            if (user == null || !user.Identity.IsAuthenticated)
                throw new UnauthorizedAccessException("User not authenticated");

            var userIdClaim = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier || c.Type == "nameid");

            if (userIdClaim == null)
                throw new UnauthorizedAccessException("UserId not found in token");

            Guid userId = Guid.Parse(userIdClaim.Value);
            return await _accountRepository.GetWalletsByUserIdAsync(userId);
        }

    }
}
