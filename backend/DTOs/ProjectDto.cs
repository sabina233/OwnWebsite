namespace backend.DTOs;

/// <summary>
/// 项目数据传输对象 - 用于API请求和响应
/// </summary>
public class ProjectDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? TechStack { get; set; }
    public string? GitHubUrl { get; set; }
    public string? DemoUrl { get; set; }
    public string? CoverImage { get; set; }
    public bool IsFeatured { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

/// <summary>
/// 创建/更新项目请求
/// </summary>
public class CreateProjectDto
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? TechStack { get; set; }
    public string? GitHubUrl { get; set; }
    public string? DemoUrl { get; set; }
    public string? CoverImage { get; set; }
    public bool IsFeatured { get; set; } = false;
}
