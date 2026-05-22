using System.ComponentModel.DataAnnotations;

namespace backend.Models;

/// <summary>
/// 联系消息实体模型 - 存储访客提交的联系表单
/// </summary>
public class ContactMessage
{
    /// <summary>消息唯一标识</summary>
    public int Id { get; set; }

    /// <summary>发件人姓名</summary>
    [Required(ErrorMessage = "姓名不能为空")]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    /// <summary>发件人邮箱</summary>
    [Required(ErrorMessage = "邮箱不能为空")]
    [MaxLength(200)]
    [EmailAddress(ErrorMessage = "邮箱格式不正确")]
    public string Email { get; set; } = string.Empty;

    /// <summary>消息主题</summary>
    [MaxLength(200)]
    public string? Subject { get; set; }

    /// <summary>消息内容</summary>
    [Required(ErrorMessage = "消息内容不能为空")]
    public string Message { get; set; } = string.Empty;

    /// <summary>是否已读</summary>
    public bool IsRead { get; set; } = false;

    /// <summary>提交时间</summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
