using Infrastructure.Entities;
using Infrastructure.Data;
using ManageUserSystem.Common;
using ManageUserSystem.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ManageUserSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AuthController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var user = await _context.Users
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(u => u.Username == dto.Username);

            if (user == null)
                return Unauthorized(ApiResponse<string>.Fail("Sai tài khoản hoặc mật khẩu"));

            var hashedInput = ToSha256(dto.Password);

            if (user.PasswordHash != hashedInput)
                return Unauthorized(ApiResponse<string>.Fail("Sai tài khoản hoặc mật khẩu"));

            var roles = user.UserRoles.Select(ur => ur.Role.Name).ToList();
            user.LastLogin = DateTime.UtcNow;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes("this_is_a_super_secret_key_with_32_characters");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, roles.First())
        }),
                Expires = DateTime.UtcNow.AddHours(1),
                Issuer = "your-app",
                Audience = "your-app",
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return Ok(ApiResponse<object>.SuccessResponse(new
            {
                user.Id,
                user.Username,
                Roles = roles,
                AccessToken = tokenString
            }, "Đăng nhập thành công"));
        }


        // Helper để hash SHA256
        private string ToSha256(string input)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(input);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
    }
}
