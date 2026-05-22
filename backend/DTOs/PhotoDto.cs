namespace backend.DTOs;

/// <summary>
/// 照片数据传输对象 - 用于API请求和响应
/// </summary>
public class PhotoDto
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string FilePath { get; set; } = string.Empty;
    public string? ThumbnailPath { get; set; }
    public string? Category { get; set; }
    public DateTime CreatedAt { get; set; }
}
