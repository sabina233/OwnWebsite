using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Data;
using backend.DTOs;
using backend.Models;

namespace backend.Controllers;

/// <summary>
/// 日记控制器 - 处理私密日记的增删改查
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class DiaryController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public DiaryController(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// 获取日记列表 - 需要私密区域访问权限，按日期倒序排列
    /// </summary>
    [HttpGet]
    [Authorize(Roles = "PrivateAccess,Admin")]
    public async Task<ActionResult<PagedResult<DiaryListItemDto>>> GetDiaries(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
    {
        var query = _context.Diaries.AsQueryable();

        var totalCount = await query.CountAsync();

        // 按创建时间倒序排列（最新的在前面）
        var diaries = await query
            .OrderByDescending(d => d.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(d => new DiaryListItemDto
            {
                Id = d.Id,
                Title = d.Title,
                Mood = d.Mood,
                Weather = d.Weather,
                CreatedAt = d.CreatedAt
            })
            .ToListAsync();

        return Ok(new PagedResult<DiaryListItemDto>
        {
            Items = diaries,
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount
        });
    }

    /// <summary>
    /// 获取日记详情 - 需要私密区域访问权限
    /// </summary>
    [HttpGet("{id}")]
    [Authorize(Roles = "PrivateAccess,Admin")]
    public async Task<ActionResult<DiaryDto>> GetDiary(int id)
    {
        var diary = await _context.Diaries.FindAsync(id);

        if (diary == null)
        {
            return NotFound(new { message = "日记不存在" });
        }

        return Ok(MapToDto(diary));
    }

    /// <summary>
    /// 创建日记 - 需要管理员权限
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<DiaryDto>> CreateDiary(CreateDiaryDto dto)
    {
        var diary = new Diary
        {
            Title = dto.Title,
            Content = dto.Content,
            Mood = dto.Mood,
            Weather = dto.Weather,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.Diaries.Add(diary);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetDiary), new { id = diary.Id }, MapToDto(diary));
    }

    /// <summary>
    /// 更新日记 - 需要管理员权限
    /// </summary>
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<DiaryDto>> UpdateDiary(int id, CreateDiaryDto dto)
    {
        var diary = await _context.Diaries.FindAsync(id);

        if (diary == null)
        {
            return NotFound(new { message = "日记不存在" });
        }

        diary.Title = dto.Title;
        diary.Content = dto.Content;
        diary.Mood = dto.Mood;
        diary.Weather = dto.Weather;
        diary.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return Ok(MapToDto(diary));
    }

    /// <summary>
    /// 删除日记 - 需要管理员权限
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteDiary(int id)
    {
        var diary = await _context.Diaries.FindAsync(id);

        if (diary == null)
        {
            return NotFound(new { message = "日记不存在" });
        }

        _context.Diaries.Remove(diary);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    /// <summary>
    /// 实体转DTO映射
    /// </summary>
    private static DiaryDto MapToDto(Diary diary)
    {
        return new DiaryDto
        {
            Id = diary.Id,
            Title = diary.Title,
            Content = diary.Content,
            Mood = diary.Mood,
            Weather = diary.Weather,
            CreatedAt = diary.CreatedAt,
            UpdatedAt = diary.UpdatedAt
        };
    }
}
