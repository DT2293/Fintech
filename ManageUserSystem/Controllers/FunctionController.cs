using Infrastructure.Entities;
using Infrastructure.Repositories;
using ManageUserSystem.Common;
using ManageUserSystem.Dtos;
using ManageUserSystem.Filter;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace ManageUserSystem.Controllers
{
    [Authorize]
    [ServiceFilter(typeof(DynamicPermissionFilter))]
    [Route("api/[controller]")]
    [ApiController]
    public class FunctionController : ControllerBase
    {
        private readonly IGenericRepository<Function> _repo;

        public FunctionController(IGenericRepository<Function> repo)
        {
            _repo = repo;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllFunction()
        {
            var data = await _repo.GetAllAsync();

            var dtos = data.Select(f => new FunctionDto
            {
                Key = f.Key,
                Description = f.Description
            });

            return Ok(ApiResponse<IEnumerable<FunctionDto>>.SuccessResponse(dtos));
        }

        //[HttpGet("{id}")]
        //public async Task<IActionResult> GetById(Guid id)
        //{
        //    var item = await _repo.GetByIdAsync(id);
        //    if (item == null)
        //        return NotFound(ApiResponse<string>.Fail("Không tìm thấy chức năng"));

        //    var dto = new FunctionDto
        //    {
        //        Key = item.Key,
        //        Description = item.Description
        //    };

        //    return Ok(ApiResponse<FunctionDto>.SuccessResponse(dto));
        //}
        [HttpPost]
        public async Task<IActionResult> CreateFuntion([FromBody] FunctionDto dto)
        {
            var function = new Function
            {
                Id = Guid.NewGuid(),
                Key = dto.Key,
                Description = dto.Description
            };

            await _repo.AddAsync(function);
            await _repo.SaveChangesAsync();

            return Ok(ApiResponse<FunctionDto>.SuccessResponse(dto, "Tạo thành công"));
        }

        //[HttpPut("{id}")]
        //public async Task<IActionResult> Update(Guid id, [FromBody] FunctionDto dto)
        //{
        //    var existing = await _repo.GetByIdAsync(id);
        //    if (existing == null)
        //        return NotFound(ApiResponse<string>.Fail("Không tìm thấy chức năng"));

        //    existing.Key = dto.Key;
        //    existing.Description = dto.Description;
        //    _repo.Update(existing);
        //    await _repo.SaveChangesAsync();

        //    return Ok(ApiResponse<FunctionDto>.SuccessResponse(dto, "Cập nhật thành công"));
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
    }
}
