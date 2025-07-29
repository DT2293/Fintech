using Azure.Core;
using ManageUserSystem.Common;
using ManageUserSystem.Dtos.Transactions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ManageUserSystem.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly TransactionService _transactionService;

        public TransactionController(TransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [HttpPost("transfer")]
        public async Task<IActionResult> TransferAsync([FromBody] TransferRequestDto request)
        {
            try
            {
                await _transactionService.TransferAsync(
                    request.FromAccountId,
                    request.ToAccountId,
                    request.Amount,
                    request.Description
                );

                return Ok(ApiResponse<string>.SuccessResponse(null, "Chuyển khoản thành công"));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ApiResponse<string>.Fail(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Fail("Lỗi hệ thống: " + ex.Message));
            }
        }
    }
}
