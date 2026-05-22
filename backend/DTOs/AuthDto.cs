namespace backend.DTOs;

/// <summary>
/// 登录请求
/// </summary>
public class LoginDto
{
    /// <summary>用户名</summary>
    public string Username { get; set; } = string.Empty;

    /// <summary>密码</summary>
    public string Password { get; set; } = string.Empty;
}

/// <summary>
/// 登录响应 - 包含JWT Token
/// </summary>
public class LoginResponseDto
{
    /// <summary>JWT访问令牌</summary>
    public string Token { get; set; } = string.Empty;

    /// <summary>过期时间</summary>
    public DateTime ExpiresAt { get; set; }
}

/// <summary>
/// 私密区域密码验证请求
/// </summary>
public class VerifyPasswordDto
{
    /// <summary>访问密码</summary>
    public string Password { get; set; } = string.Empty;
}

/// <summary>
/// 私密区域验证响应
/// </summary>
public class VerifyPasswordResponseDto
{
    /// <summary>验证是否成功</summary>
    public bool Success { get; set; }

    /// <summary>临时访问令牌</summary>
    public string? Token { get; set; }

    /// <summary>过期时间</summary>
    public DateTime? ExpiresAt { get; set; }
}
