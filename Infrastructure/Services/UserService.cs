using System.Security.Cryptography;
using System.Text;
using Infrastructure.Entities;
using Infrastructure.Repositories;
using Infrastructure.Repositories.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration.UserSecrets;

namespace Infrastructure.Services
{
    public class UserService
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IGenericRepository<User> _userRepo;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserService(IGenericRepository<User> userRepo, IHttpContextAccessor httpContextAccessor, IUnitOfWork unitOfWork)
        {
            _userRepo = userRepo;
            _httpContextAccessor = httpContextAccessor;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<User>> GetUsersWithRolesAndFunctionsAsync()
        {

            return await _userRepo.Query()
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                        .ThenInclude(r => r.RolePermissions)
                            .ThenInclude(rp => rp.Function)
                .ToListAsync();
        }
        public async Task<User> CreateUserAsync(string username, string password, List<Guid> roleIds)
        {
            var usernameCreator = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "system";

            if (await _userRepo.Query().AnyAsync(u => u.Username == username))
                throw new InvalidOperationException("Username đã tồn tại");

            var user = new User
            {
                Id = Guid.NewGuid(),
                Username = username,
                PasswordHash = HashPassword(password),
                CreatedAt = DateTime.UtcNow,
                CreatedBy = usernameCreator,
                UserRoles = roleIds.Select(r => new UserRole { RoleId = r }).ToList()
            };
            await _userRepo.AddAsync(user);
            await _unitOfWork.SaveChangesAsync();

            return user;
        }
        public async Task<bool> CheckUsernameExistsAsync(string username)
        {
            return await _userRepo.Query().AnyAsync(u => u.Username == username);
        }

        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            return Convert.ToBase64String(sha256.ComputeHash(Encoding.UTF8.GetBytes(password)));
        }

        //public async Task<ApiResponse> CreateUserWithRolesAsync(CreateUserDto dto)
        //{
        //    // 1. Check duplicate username (phòng tránh trùng)
        //    var existed = await _userRepo.Query().AnyAsync(u => u.Username == dto.Username);
        //    if (existed)
        //        return ApiResponse.Failure("Username đã tồn tại");

        //    // 2. Create new user object
        //    var user = new User
        //    {
        //        Id = Guid.NewGuid(),
        //        Username = dto.Username,
        //        PasswordHash = HashHelper.HashPassword(dto.Password),
        //        UserRoles = dto.RoleIds.Select(roleId => new UserRole
        //        {
        //            RoleId = roleId
        //        }).ToList()
        //    };

        //    // 3. Save to DB
        //    await _userRepo.AddAsync(user);
        //    await _userRepo.SaveChangesAsync();

        //    return ApiResponse.Success("Tạo user thành công");
        //}


    }
}
