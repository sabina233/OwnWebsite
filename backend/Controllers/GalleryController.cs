using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Data;
using backend.DTOs;
using backend.Models;

namespace backend.Controllers;

/// <summary>
/// 相册控制器 - 处理摄影作品的展示和管理
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class GalleryController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public GalleryController(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// 获取照片列表 - 支持分页和分类筛选
    /// </summary>
    [HttpGet("photos")]
    public async Task<ActionResult<PagedResult<PhotoDto>>> GetPhotos(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] string? category = null)
    {
        var query = _context.Photos.AsQueryable();

        // 按分类筛选
        if (!string.IsNullOrEmpty(category))
        {
            query = query.Where(p => p.Category == category);
        }

        var totalCount = await query.CountAsync();

        // 按创建时间倒序排列
        var photos = await query
            .OrderByDescending(p => p.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(p => new PhotoDto
            {
                Id = p.Id,
                Title = p.Title,
                Description = p.Description,
                FilePath = p.FilePath,
                ThumbnailPath = p.ThumbnailPath,
                Category = p.Category,
                CreatedAt = p.CreatedAt
            })
            .ToListAsync();

        return Ok(new PagedResult<PhotoDto>
        {
            Items = photos,
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount
        });
    }

    /// <summary>
    /// 删除照片 - 需要管理员权限
    /// </summary>
    [HttpDelete("photos/{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeletePhoto(int id)
    {
        var photo = await _context.Photos.FindAsync(id);

        if (photo == null)
        {
            return NotFound(new { message = "照片不存在" });
        }

        // 删除物理文件
        if (!string.IsNullOrEmpty(photo.FilePath))
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", photo.FilePath.TrimStart('/'));
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }
        }

        // 删除缩略图
        if (!string.IsNullOrEmpty(photo.ThumbnailPath))
        {
            var thumbPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", photo.ThumbnailPath.TrimStart('/'));
            if (System.IO.File.Exists(thumbPath))
            {
                System.IO.File.Delete(thumbPath);
            }
        }

        _context.Photos.Remove(photo);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
