using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Data;
using backend.DTOs;
using backend.Models;

namespace backend.Controllers;

/// <summary>
/// 文章控制器 - 处理文章的增删改查
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ArticlesController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public ArticlesController(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// 获取公开文章列表 - 支持分页和分类筛选
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<PagedResult<ArticleListItemDto>>> GetArticles(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? category = null,
        [FromQuery] string? search = null)
    {
        // 构建查询：只返回已发布且非私密的文章
        var query = _context.Articles
            .Where(a => a.IsPublished && !a.IsPrivate)
            .AsQueryable();

        // 按分类筛选
        if (!string.IsNullOrEmpty(category))
        {
            query = query.Where(a => a.Category == category);
        }

        // 按关键词搜索标题和摘要
        if (!string.IsNullOrEmpty(search))
        {
            query = query.Where(a => a.Title.Contains(search) || (a.Summary != null && a.Summary.Contains(search)));
        }

        // 计算总数
        var totalCount = await query.CountAsync();

        // 分页查询并映射为DTO
        var articles = await query
            .OrderByDescending(a => a.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(a => new ArticleListItemDto
            {
                Id = a.Id,
                Title = a.Title,
                Summary = a.Summary,
                CoverImage = a.CoverImage,
                Category = a.Category,
                Tags = a.Tags,
                ViewCount = a.ViewCount,
                CreatedAt = a.CreatedAt
            })
            .ToListAsync();

        return Ok(new PagedResult<ArticleListItemDto>
        {
            Items = articles,
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount
        });
    }

    /// <summary>
    /// 获取文章详情 - 同时增加浏览次数
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<ArticleDto>> GetArticle(int id)
    {
        var article = await _context.Articles.FindAsync(id);

        if (article == null)
        {
            return NotFound(new { message = "文章不存在" });
        }

        // 只有公开文章才能被非管理员查看
        if (!article.IsPublished || article.IsPrivate)
        {
            return NotFound(new { message = "文章不存在" });
        }

        // 增加浏览次数
        article.ViewCount++;
        await _context.SaveChangesAsync();

        return Ok(MapToDto(article));
    }

    /// <summary>
    /// 创建文章 - 需要管理员权限
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ArticleDto>> CreateArticle(CreateArticleDto dto)
    {
        var article = new Article
        {
            Title = dto.Title,
            Content = dto.Content,
            Summary = dto.Summary,
            CoverImage = dto.CoverImage,
            Category = dto.Category,
            Tags = dto.Tags,
            IsPublished = dto.IsPublished,
            IsPrivate = dto.IsPrivate,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.Articles.Add(article);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetArticle), new { id = article.Id }, MapToDto(article));
    }

    /// <summary>
    /// 更新文章 - 需要管理员权限
    /// </summary>
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ArticleDto>> UpdateArticle(int id, CreateArticleDto dto)
    {
        var article = await _context.Articles.FindAsync(id);

        if (article == null)
        {
            return NotFound(new { message = "文章不存在" });
        }

        // 更新文章属性
        article.Title = dto.Title;
        article.Content = dto.Content;
        article.Summary = dto.Summary;
        article.CoverImage = dto.CoverImage;
        article.Category = dto.Category;
        article.Tags = dto.Tags;
        article.IsPublished = dto.IsPublished;
        article.IsPrivate = dto.IsPrivate;
        article.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return Ok(MapToDto(article));
    }

    /// <summary>
    /// 删除文章 - 需要管理员权限
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteArticle(int id)
    {
        var article = await _context.Articles.FindAsync(id);

        if (article == null)
        {
            return NotFound(new { message = "文章不存在" });
        }

        _context.Articles.Remove(article);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    /// <summary>
    /// 实体转DTO映射
    /// </summary>
    private static ArticleDto MapToDto(Article article)
    {
        return new ArticleDto
        {
            Id = article.Id,
            Title = article.Title,
            Content = article.Content,
            Summary = article.Summary,
            CoverImage = article.CoverImage,
            Category = article.Category,
            Tags = article.Tags,
            IsPublished = article.IsPublished,
            IsPrivate = article.IsPrivate,
            ViewCount = article.ViewCount,
            CreatedAt = article.CreatedAt,
            UpdatedAt = article.UpdatedAt
        };
    }
}
