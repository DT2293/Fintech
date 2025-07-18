using Infrastructure.Entities;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services
{
    public class PermissionService
    {
        private readonly IGenericRepository<Function> _funcRepo;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public PermissionService(IGenericRepository<Function> funcRepo, IHttpContextAccessor httpContextAccessor)
        {
            _funcRepo = funcRepo;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Function> CreateFuncAsync(string name, string description, bool isactive)
        {
            var usernameCreator = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "system";

            if (await _funcRepo.Query().AnyAsync(u => u.Key == name))
                throw new InvalidOperationException("Permission key already exists");

            var function = new Function
            {
                Id = Guid.NewGuid(),
                Key = "fuction:" + name,
                Description = description,
                CreatedBy = usernameCreator,
                IsActive = true,
            };

            await _funcRepo.AddAsync(function);
            await _funcRepo.SaveChangesAsync();

            return function;
        }



    }
}
