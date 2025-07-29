using Infrastructure.Entities;
using Infrastructure.Services;
using ManageUserSystem.Common;
using ManageUserSystem.Dtos.Wallet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ManageUserSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // bắt buộc phải đăng nhập mới gọi được
    public class AccountWalletController : ControllerBase
    {
        private readonly AccountWalletService _accountWalletService;

        public AccountWalletController(AccountWalletService accountWalletService)
        {
            _accountWalletService = accountWalletService;
        }
        private string GetCurrentUsername()
        {
            return User?.Identity?.Name ?? "Unknown";
        }

        [HttpGet("my-wallets")]
        public async Task<IActionResult> GetMyWallets()
        {
            try
            {
                var wallets = await _accountWalletService.GetAllWalletsByCurrentUserAsync();

                // Lấy username từ Claims
                var username = User.Identity?.Name ?? "Unknown";

                // Tạo DTO
                var dto = new UserWalletsDto
                {
                    Username = username,
                    WalletIds = wallets.Select(w => w.Id).ToList()
                };

                return Ok(ApiResponse<UserWalletsDto>.SuccessResponse(dto));
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ApiResponse<string>.Fail(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Fail("Server error: " + ex.Message));
            }
        }


    }
}
