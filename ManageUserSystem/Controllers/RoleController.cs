using Infrastructure.Entities;
using Infrastructure.Repositories.Generic;
using Infrastructure.Services;
using ManageUserSystem.Common;
using ManageUserSystem.Dtos.Role;
using ManageUserSystem.Dtos.User;
using ManageUserSystem.Filter;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ManageUserSystem.Controllers
{

    [Authorize]
    [ServiceFilter(typeof(DynamicPermissionFilter))]
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IGenericRepository<Role> _repo;
        private readonly IGenericRepository<RolePermission> _rolePermRepo;
        private readonly RoleService _roleService;

        public RoleController(IGenericRepository<Role> repo, IGenericRepository<RolePermission> rolePermRepo)
        {
            _repo = repo;
            _rolePermRepo = rolePermRepo;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllRole()
        {
            var data = await _repo.GetAllAsync();

            var dtos = data.Select(f => new RoleDto
            {

                Name = f.Name
            });

            return Ok(ApiResponse<IEnumerable<RoleDto>>.SuccessResponse(dtos));
        }

        //[HttpGet("{id}")]
        //public async Task<IActionResult> GetById(Guid id)
        //{
        //    var item = await _repo.GetByIdAsync(id);
        //    if (item == null)
        //        return NotFound(ApiResponse<string>.Fail("Không tìm thấy quyền"));

        //    var dto = new RoleDto
        //    {

        //        Id = item.Id,
        //        Name = item.Name
        //    };

        //    return Ok(ApiResponse<RoleDto>.SuccessResponse(dto));
        //}


        [HttpPost]
        public async Task<IActionResult> CreateRole(CreateRoleDto dto)
        {
          //  var existed = await _userService.CheckUsernameExistsAsync(dto.Username);
           // ..if (existed)
             //   return BadRequest(ApiResponse<string>.Fail("Username đã tồn tại"));

            var role = await _roleService.CreateRoleAsync(dto.Name);
            return Ok(ApiResponse<string>.SuccessResponse("Tạo Role thành công"));
        }

        //[HttpPost]
        //public async Task<IActionResult> CreateRole([FromBody] RoleDto dto)
        //{
        //    var role = new Role
        //    {
        //        Id = Guid.NewGuid(),
        //        Name = dto.Name
        //    };

        //    await _repo.AddAsync(role);
        //    await _repo.SaveChangesAsync();

        //    return Ok(ApiResponse<RoleDto>.SuccessResponse(dto, "Tạo thành công"));
        //}

        //[HttpPut("{id}")]
        //public async Task<IActionResult> Update(Guid id, [FromBody] RoleDto dto)
        //{
        //    var existing = await _repo.GetByIdAsync(id);
        //    if (existing == null)
        //        return NotFound(ApiResponse<string>.Fail("Không tìm thấy chức năng"));

        //    existing.Name = dto.Name;
        //    _repo.Update(existing);
        //    await _repo.SaveChangesAsync();

        //    return Ok(ApiResponse<RoleDto>.SuccessResponse(dto, "Cập nhật thành công"));
        //}

        //[HttpDelete("{id}")]
        //public async Task<IActionResult> Delete(Guid id)
        //{
        //    var existing = await _repo.GetByIdAsync(id);
        //    if (existing == null)
        //        return NotFound(ApiResponse<string>.Fail("Không tìm thấy chức năng"));

        //    _repo.Delete(existing);
        //    await _repo.SaveChangesAsync();

        //    return Ok(ApiResponse<string>.SuccessResponse("Xoá thành công"));
        //}


        //[HttpPost("assign-permission")]
        //public async Task<IActionResult> AssignPermission([FromBody] RolePermissionDto dto)
        //{
        //    var entity = new RolePermission
        //    {
        //        RoleId = dto.RoleId,
        //        FunctionId = dto.FunctionId
        //    };

        //    await _rolePermRepo.AddAsync(entity);
        //    await _rolePermRepo.SaveChangesAsync();

        //    return Ok(ApiResponse<string>.SuccessResponse("Gán quyền thành công"));
        //}
    }
}
