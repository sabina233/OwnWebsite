using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using backend.Data;
using backend.DTOs;
using backend.Models;

namespace backend.Controllers;

/// <summary>
/// 认证控制器 - 处理管理员登录和私密区域验证
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IConfiguration _configuration;

    public AuthController(ApplicationDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    /// <summary>
    /// 管理员登录 - 验证用户名密码并返回JWT Token
    /// </summary>
    [HttpPost("login")]
    public async Task<ActionResult<LoginResponseDto>> Login(LoginDto dto)
    {
        // 查找管理员用户
        var user = await _context.AdminUsers
            .FirstOrDefaultAsync(u => u.Username == dto.Username);

        // 验证用户存在且密码正确
        if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
        {
            return Unauthorized(new { message = "用户名或密码错误" });
        }

        // 生成JWT Token
        var token = GenerateJwtToken(user);
        var expiresAt = DateTime.UtcNow.AddHours(24);

        return Ok(new LoginResponseDto
        {
            Token = token,
            ExpiresAt = expiresAt
        });
    }

    /// <summary>
    /// 验证私密区域密码 - 返回临时访问令牌
    /// </summary>
    [HttpPost("verify-password")]
    public async Task<ActionResult<VerifyPasswordResponseDto>> VerifyPassword(VerifyPasswordDto dto)
    {
        // 获取网站设置中的私密区域密码
        var settings = await _context.SiteSettings.FirstOrDefaultAsync();

        if (settings?.PrivatePassword == null)
        {
            return BadRequest(new { message = "私密区域未设置密码" });
        }

        // 验证密码
        if (!BCrypt.Net.BCrypt.Verify(dto.Password, settings.PrivatePassword))
        {
            return Ok(new VerifyPasswordResponseDto { Success = false });
        }

        // 生成临时访问令牌（有效期2小时）
        var token = GeneratePrivateToken();
        var expiresAt = DateTime.UtcNow.AddHours(2);

        return Ok(new VerifyPasswordResponseDto
        {
            Success = true,
            Token = token,
            ExpiresAt = expiresAt
        });
    }

    /// <summary>
    /// 生成JWT Token
    /// </summary>
    private string GenerateJwtToken(AdminUser user)
    {
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? "YourSuperSecretKeyHere12345678901234"));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, "Admin")
        };

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(24),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    /// <summary>
    /// 生成私密区域临时访问令牌
    /// </summary>
    private string GeneratePrivateToken()
    {
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? "YourSuperSecretKeyHere12345678901234"));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.Role, "PrivateAccess")
        };

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddHours(2),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
