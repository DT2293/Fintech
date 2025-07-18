using Infrastructure.Entities;
using Infrastructure.Repositories;
using Infrastructure.Services;
using ManageUserSystem.Common;
using ManageUserSystem.Dtos;
using ManageUserSystem.Filter;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;

namespace ManageUserSystem.Controllers
{
    [Authorize]
    [ServiceFilter(typeof(DynamicPermissionFilter))]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IGenericRepository<User> _repo;
        private readonly IGenericRepository<UserRole> _userRoleRepo;

        private readonly UserService _userService;
        public UserController(IGenericRepository<User> repo, IGenericRepository<UserRole> userRoleRepo, UserService userService)
        {
            _repo = repo;
            _userRoleRepo = userRoleRepo;
            _userService = userService;
        }
 
        [HttpGet]
        public async Task<IActionResult> GetAllUser()
        {
            var data = await _userService.GetUsersWithRolesAndFunctionsAsync();

            var dtos = data.Select(u => new UserDto
            {
                Id = u.Id,
                Username = u.Username,
                RoleName = u.UserRoles.Select(ur => ur.Role.Name).FirstOrDefault(),
                FunctionNames = u.UserRoles
                    .SelectMany(ur => ur.Role.RolePermissions)
                    .Select(rp => rp.Function.Description)
                    .Distinct()
                    .ToList()
            });

            return Ok(ApiResponse<IEnumerable<UserDto>>.SuccessResponse(dtos));
        }

        //[HttpGet("{id}")]
        //public async Task<IActionResult> GetById(Guid id)
        //{
        //    var user = await _repo.GetByIdAsync(id);
        //    if (user == null)
        //        return NotFound(ApiResponse<string>.Fail("Không tìm thấy người dùng"));

        //    var dto = new UserDto
        //    {
        //        Id = user.Id,
        //        Username = user.Username
        //    };

        //    return Ok(ApiResponse<UserDto>.SuccessResponse(dto));
        //}

        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserDto dto)
        {
            var existed = await _userService.CheckUsernameExistsAsync(dto.Username);
            if (existed)
                return BadRequest(ApiResponse<string>.Fail("Username đã tồn tại"));

            var user = await _userService.CreateUserAsync(dto.Username, dto.Password, dto.RoleIds );
            return Ok(ApiResponse<string>.SuccessResponse("Tạo user thành công"));
        }
        [HttpPost("assign-role")]
        public async Task<IActionResult> AssignRole([FromBody] UserRoleDto dto)
        {
            var entity = new UserRole
            {
                UserId = dto.UserId,
                RoleId = dto.RoleId
            };

            await _userRoleRepo.AddAsync(entity);
            await _userRoleRepo.SaveChangesAsync();

            return Ok(ApiResponse<string>.SuccessResponse("Gán vai trò thành công"));
        }
     


    }
}
