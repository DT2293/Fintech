using Infrastructure.Entities;
using Infrastructure.Repositories;
using Infrastructure.Repositories.Generic;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services
{
    public class RoleService
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IGenericRepository<Role> _roleRepo;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public RoleService(IGenericRepository<Role> roleRepo, IHttpContextAccessor httpContextAccessor,IUnitOfWork unitOfWork)
        {
            _roleRepo = roleRepo;
            _httpContextAccessor = httpContextAccessor;
            _unitOfWork = unitOfWork;
        }

        public async Task<Role> CreateRoleAsync(string name)
        {
            var usernameCreator = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "system";

            if (await _roleRepo.Query().AnyAsync(u => u.Name == name))
                throw new InvalidOperationException("Permission key already exists");

            var role = new Role
            {
                Id = Guid.NewGuid(),
                Name = name,
                CreatedBy = usernameCreator,

            };

            await _roleRepo.AddAsync(role);
            await _unitOfWork.SaveChangesAsync();

            return role;
        }
    }
}
