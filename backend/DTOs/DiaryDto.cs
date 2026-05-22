namespace backend.DTOs;

/// <summary>
/// 日记数据传输对象 - 用于API请求和响应
/// </summary>
public class DiaryDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string? Mood { get; set; }
    public string? Weather { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

/// <summary>
/// 日记列表项 - 不包含完整内容
/// </summary>
public class DiaryListItemDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Mood { get; set; }
    public string? Weather { get; set; }
    public DateTime CreatedAt { get; set; }
}

/// <summary>
/// 创建/更新日记请求
/// </summary>
public class CreateDiaryDto
{
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string? Mood { get; set; }
    public string? Weather { get; set; }
}
