using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Data;
using backend.DTOs;
using backend.Models;

namespace backend.Controllers;

/// <summary>
/// 文章分类控制器 - 处理分类的增删改查
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public CategoriesController(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// 获取所有分类 - 公开接口
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<List<CategoryDto>>> GetCategories()
    {
        var categories = await _context.Categories
            .OrderBy(c => c.SortOrder)
            .ThenBy(c => c.Name)
            .Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name,
                Slug = c.Slug,
                Description = c.Description,
                ParentId = c.ParentId,
                SortOrder = c.SortOrder,
                CreatedAt = c.CreatedAt
            })
            .ToListAsync();

        return Ok(categories);
    }

    /// <summary>
    /// 创建分类 - 需要管理员权限
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<CategoryDto>> CreateCategory(CreateCategoryDto dto)
    {
        // 检查Slug是否已存在
        if (await _context.Categories.AnyAsync(c => c.Slug == dto.Slug))
        {
            return BadRequest(new { message = "该分类标识已存在" });
        }

        var category = new Category
        {
            Name = dto.Name,
            Slug = dto.Slug,
            Description = dto.Description,
            ParentId = dto.ParentId,
            SortOrder = dto.SortOrder,
            CreatedAt = DateTime.UtcNow
        };

        _context.Categories.Add(category);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetCategories), new { id = category.Id }, new CategoryDto
        {
            Id = category.Id,
            Name = category.Name,
            Slug = category.Slug,
            Description = category.Description,
            ParentId = category.ParentId,
            SortOrder = category.SortOrder,
            CreatedAt = category.CreatedAt
        });
    }

    /// <summary>
    /// 更新分类 - 需要管理员权限
    /// </summary>
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<CategoryDto>> UpdateCategory(int id, CreateCategoryDto dto)
    {
        var category = await _context.Categories.FindAsync(id);

        if (category == null)
        {
            return NotFound(new { message = "分类不存在" });
        }

        // 检查Slug是否与其他分类冲突
        if (await _context.Categories.AnyAsync(c => c.Slug == dto.Slug && c.Id != id))
        {
            return BadRequest(new { message = "该分类标识已被使用" });
        }

        category.Name = dto.Name;
        category.Slug = dto.Slug;
        category.Description = dto.Description;
        category.ParentId = dto.ParentId;
        category.SortOrder = dto.SortOrder;

        await _context.SaveChangesAsync();

        return Ok(new CategoryDto
        {
            Id = category.Id,
            Name = category.Name,
            Slug = category.Slug,
            Description = category.Description,
            ParentId = category.ParentId,
            SortOrder = category.SortOrder,
            CreatedAt = category.CreatedAt
        });
    }

    /// <summary>
    /// 删除分类 - 需要管理员权限
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        var category = await _context.Categories.FindAsync(id);

        if (category == null)
        {
            return NotFound(new { message = "分类不存在" });
        }

        _context.Categories.Remove(category);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
