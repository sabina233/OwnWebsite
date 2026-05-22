namespace backend.DTOs;

/// <summary>
/// 文章数据传输对象 - 用于API请求和响应
/// </summary>
public class ArticleDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string? Summary { get; set; }
    public string? CoverImage { get; set; }
    public string? Category { get; set; }
    public string? Tags { get; set; }
    public bool IsPublished { get; set; }
    public bool IsPrivate { get; set; }
    public int ViewCount { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

/// <summary>
/// 文章列表项 - 不包含完整内容，用于列表展示
/// </summary>
public class ArticleListItemDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Summary { get; set; }
    public string? CoverImage { get; set; }
    public string? Category { get; set; }
    public string? Tags { get; set; }
    public int ViewCount { get; set; }
    public DateTime CreatedAt { get; set; }
}

/// <summary>
/// 创建/更新文章请求
/// </summary>
public class CreateArticleDto
{
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string? Summary { get; set; }
    public string? CoverImage { get; set; }
    public string? Category { get; set; }
    public string? Tags { get; set; }
    public bool IsPublished { get; set; } = true;
    public bool IsPrivate { get; set; } = false;
}
