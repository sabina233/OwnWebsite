using System.ComponentModel.DataAnnotations;

namespace backend.Models;

/// <summary>
/// 照片实体模型 - 存储摄影作品信息
/// </summary>
public class Photo
{
    /// <summary>照片唯一标识</summary>
    public int Id { get; set; }

    /// <summary>照片标题</summary>
    [MaxLength(200)]
    public string? Title { get; set; }

    /// <summary>照片描述</summary>
    [MaxLength(500)]
    public string? Description { get; set; }

    /// <summary>原图文件路径</summary>
    [Required]
    [MaxLength(500)]
    public string FilePath { get; set; } = string.Empty;

    /// <summary>缩略图文件路径</summary>
    [MaxLength(500)]
    public string? ThumbnailPath { get; set; }

    /// <summary>照片分类</summary>
    [MaxLength(100)]
    public string? Category { get; set; }

    /// <summary>上传时间</summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
