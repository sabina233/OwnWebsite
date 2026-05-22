using System.ComponentModel.DataAnnotations;

namespace backend.Models;

/// <summary>
/// 日记实体模型 - 存储私密日记信息
/// </summary>
public class Diary
{
    /// <summary>日记唯一标识</summary>
    public int Id { get; set; }

    /// <summary>日记标题</summary>
    [Required(ErrorMessage = "日记标题不能为空")]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    /// <summary>日记内容（Markdown格式）</summary>
    [Required(ErrorMessage = "日记内容不能为空")]
    public string Content { get; set; } = string.Empty;

    /// <summary>心情标签</summary>
    [MaxLength(50)]
    public string? Mood { get; set; }

    /// <summary>天气情况</summary>
    [MaxLength(50)]
    public string? Weather { get; set; }

    /// <summary>创建时间（用于日期排序）</summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>最后更新时间</summary>
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
