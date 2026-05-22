using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using backend.Data;
using backend.Models;

namespace backend.Controllers;

/// <summary>
/// 文件上传控制器 - 处理图片和文件上传
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class UploadController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IWebHostEnvironment _environment;

    // 允许上传的图片格式
    private static readonly string[] AllowedImageExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".webp" };

    public UploadController(ApplicationDbContext context, IWebHostEnvironment environment)
    {
        _context = context;
        _environment = environment;
    }

    /// <summary>
    /// 上传照片到相册 - 需要管理员权限
    /// </summary>
    [HttpPost("photo")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<Photo>> UploadPhoto(
        IFormFile file,
        [FromForm] string? title = null,
        [FromForm] string? description = null,
        [FromForm] string? category = null)
    {
        // 验证文件
        if (file == null || file.Length == 0)
        {
            return BadRequest(new { message = "请选择要上传的文件" });
        }

        // 验证文件类型
        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
        if (!AllowedImageExtensions.Contains(extension))
        {
            return BadRequest(new { message = "不支持的文件格式，请上传 JPG、PNG、GIF 或 WebP 格式的图片" });
        }

        // 限制文件大小（10MB）
        if (file.Length > 10 * 1024 * 1024)
        {
            return BadRequest(new { message = "文件大小不能超过10MB" });
        }

        // 创建上传目录
        var uploadDir = Path.Combine(_environment.WebRootPath, "uploads", "photos");
        if (!Directory.Exists(uploadDir))
        {
            Directory.CreateDirectory(uploadDir);
        }

        // 生成唯一文件名
        var fileName = $"{Guid.NewGuid()}{extension}";
        var filePath = Path.Combine(uploadDir, fileName);

        // 保存文件
        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        // 创建缩略图目录
        var thumbDir = Path.Combine(_environment.WebRootPath, "uploads", "thumbnails");
        if (!Directory.Exists(thumbDir))
        {
            Directory.CreateDirectory(thumbDir);
        }

        // 保存照片信息到数据库
        var photo = new Photo
        {
            Title = title,
            Description = description,
            FilePath = $"/uploads/photos/{fileName}",
            ThumbnailPath = $"/uploads/photos/{fileName}", // 简化处理，实际可生成缩略图
            Category = category,
            CreatedAt = DateTime.UtcNow
        };

        _context.Photos.Add(photo);
        await _context.SaveChangesAsync();

        return Ok(photo);
    }

    /// <summary>
    /// 上传文章封面图片
    /// </summary>
    [HttpPost("cover")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<object>> UploadCover(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest(new { message = "请选择要上传的文件" });
        }

        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
        if (!AllowedImageExtensions.Contains(extension))
        {
            return BadRequest(new { message = "不支持的文件格式" });
        }

        var uploadDir = Path.Combine(_environment.WebRootPath, "uploads", "covers");
        if (!Directory.Exists(uploadDir))
        {
            Directory.CreateDirectory(uploadDir);
        }

        var fileName = $"{Guid.NewGuid()}{extension}";
        var filePath = Path.Combine(uploadDir, fileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        return Ok(new { url = $"/uploads/covers/{fileName}" });
    }

    /// <summary>
    /// 上传头像
    /// </summary>
    [HttpPost("avatar")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<object>> UploadAvatar(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest(new { message = "请选择要上传的文件" });
        }

        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
        if (!AllowedImageExtensions.Contains(extension))
        {
            return BadRequest(new { message = "不支持的文件格式" });
        }

        var uploadDir = Path.Combine(_environment.WebRootPath, "uploads", "avatars");
        if (!Directory.Exists(uploadDir))
        {
            Directory.CreateDirectory(uploadDir);
        }

        // 头像使用固定文件名，方便更新
        var fileName = $"avatar{extension}";
        var filePath = Path.Combine(uploadDir, fileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        return Ok(new { url = $"/uploads/avatars/{fileName}" });
    }
}
