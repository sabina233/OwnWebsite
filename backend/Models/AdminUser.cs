using System.ComponentModel.DataAnnotations;

namespace backend.Models;

/// <summary>
/// 管理员用户实体模型 - 存储后台管理员账号信息
/// </summary>
public class AdminUser
{
    /// <summary>用户唯一标识</summary>
    public int Id { get; set; }

    /// <summary>用户名</summary>
    [Required(ErrorMessage = "用户名不能为空")]
    [MaxLength(50)]
    public string Username { get; set; } = string.Empty;

    /// <summary>密码哈希值（使用BCrypt加密）</summary>
    [Required]
    [MaxLength(200)]
    public string PasswordHash { get; set; } = string.Empty;
}
