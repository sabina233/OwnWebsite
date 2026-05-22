using System.ComponentModel.DataAnnotations;

namespace backend.Models;

/// <summary>
/// 文章分类实体模型 - 支持层级结构的分类管理
/// </summary>
public class Category
{
    /// <summary>分类唯一标识</summary>
    public int Id { get; set; }

    /// <summary>分类名称</summary>
    [Required(ErrorMessage = "分类名称不能为空")]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    /// <summary>URL友好的标识符（用于路由）</summary>
    [Required]
    [MaxLength(100)]
    public string Slug { get; set; } = string.Empty;

    /// <summary>分类描述</summary>
    [MaxLength(500)]
    public string? Description { get; set; }

    /// <summary>父分类ID（null表示顶级分类）</summary>
    public int? ParentId { get; set; }

    /// <summary>排序顺序</summary>
    public int SortOrder { get; set; } = 0;

    /// <summary>创建时间</summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
