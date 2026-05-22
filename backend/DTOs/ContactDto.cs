namespace backend.DTOs;

/// <summary>
/// 联系表单提交请求
/// </summary>
public class ContactDto
{
    /// <summary>发件人姓名</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>发件人邮箱</summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>消息主题</summary>
    public string? Subject { get; set; }

    /// <summary>消息内容</summary>
    public string Message { get; set; } = string.Empty;
}

/// <summary>
/// 联系消息响应（管理后台使用）
/// </summary>
public class ContactMessageDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Subject { get; set; }
    public string Message { get; set; } = string.Empty;
    public bool IsRead { get; set; }
    public DateTime CreatedAt { get; set; }
}
