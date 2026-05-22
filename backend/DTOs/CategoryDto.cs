namespace backend.DTOs;

/// <summary>
/// 文章分类数据传输对象
/// </summary>
public class CategoryDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int? ParentId { get; set; }
    public int SortOrder { get; set; }
    public DateTime CreatedAt { get; set; }
}

/// <summary>
/// 创建/更新分类请求
/// </summary>
public class CreateCategoryDto
{
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int? ParentId { get; set; }
    public int SortOrder { get; set; } = 0;
}
