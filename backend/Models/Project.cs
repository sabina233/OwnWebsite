using System.ComponentModel.DataAnnotations;

namespace backend.Models;

/// <summary>
/// 项目实体模型 - 存储个人项目展示信息
/// </summary>
public class Project
{
    /// <summary>项目唯一标识</summary>
    public int Id { get; set; }

    /// <summary>项目名称</summary>
    [Required(ErrorMessage = "项目名称不能为空")]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    /// <summary>项目描述（Markdown格式）</summary>
    [Required(ErrorMessage = "项目描述不能为空")]
    public string Description { get; set; } = string.Empty;

    /// <summary>技术栈标签（JSON数组格式存储）</summary>
    [MaxLength(500)]
    public string? TechStack { get; set; }

    /// <summary>GitHub仓库链接</summary>
    [MaxLength(500)]
    public string? GitHubUrl { get; set; }

    /// <summary>在线演示链接</summary>
    [MaxLength(500)]
    public string? DemoUrl { get; set; }

    /// <summary>项目封面图片路径</summary>
    [MaxLength(500)]
    public string? CoverImage { get; set; }

    /// <summary>是否置顶展示</summary>
    public bool IsFeatured { get; set; } = false;

    /// <summary>创建时间</summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>最后更新时间</summary>
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
