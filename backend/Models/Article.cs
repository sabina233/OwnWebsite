using System.ComponentModel.DataAnnotations;

namespace backend.Models;

/// <summary>
/// 文章实体模型 - 存储博客文章信息
/// </summary>
public class Article
{
    /// <summary>文章唯一标识</summary>
    public int Id { get; set; }

    /// <summary>文章标题</summary>
    [Required(ErrorMessage = "文章标题不能为空")]
    [MaxLength(200, ErrorMessage = "文章标题最长200个字符")]
    public string Title { get; set; } = string.Empty;

    /// <summary>文章内容（Markdown格式）</summary>
    [Required(ErrorMessage = "文章内容不能为空")]
    public string Content { get; set; } = string.Empty;

    /// <summary>文章摘要，用于列表展示</summary>
    [MaxLength(500)]
    public string? Summary { get; set; }

    /// <summary>封面图片路径</summary>
    [MaxLength(500)]
    public string? CoverImage { get; set; }

    /// <summary>文章分类名称</summary>
    [MaxLength(100)]
    public string? Category { get; set; }

    /// <summary>标签列表（JSON数组格式存储）</summary>
    [MaxLength(500)]
    public string? Tags { get; set; }

    /// <summary>是否已发布</summary>
    public bool IsPublished { get; set; } = true;

    /// <summary>是否为私密文章</summary>
    public bool IsPrivate { get; set; } = false;

    /// <summary>浏览次数</summary>
    public int ViewCount { get; set; } = 0;

    /// <summary>创建时间</summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>最后更新时间</summary>
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
