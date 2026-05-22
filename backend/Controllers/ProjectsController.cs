using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Data;
using backend.DTOs;
using backend.Models;

namespace backend.Controllers;

/// <summary>
/// 项目控制器 - 处理项目展示的增删改查
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ProjectsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public ProjectsController(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// 获取项目列表 - 置顶项目优先展示
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<List<ProjectDto>>> GetProjects()
    {
        var projects = await _context.Projects
            .OrderByDescending(p => p.IsFeatured)
            .ThenByDescending(p => p.CreatedAt)
            .Select(p => new ProjectDto
            {
                Id = p.Id,
                Title = p.Title,
                Description = p.Description,
                TechStack = p.TechStack,
                GitHubUrl = p.GitHubUrl,
                DemoUrl = p.DemoUrl,
                CoverImage = p.CoverImage,
                IsFeatured = p.IsFeatured,
                CreatedAt = p.CreatedAt,
                UpdatedAt = p.UpdatedAt
            })
            .ToListAsync();

        return Ok(projects);
    }

    /// <summary>
    /// 获取项目详情
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<ProjectDto>> GetProject(int id)
    {
        var project = await _context.Projects.FindAsync(id);

        if (project == null)
        {
            return NotFound(new { message = "项目不存在" });
        }

        return Ok(MapToDto(project));
    }

    /// <summary>
    /// 创建项目 - 需要管理员权限
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ProjectDto>> CreateProject(CreateProjectDto dto)
    {
        var project = new Project
        {
            Title = dto.Title,
            Description = dto.Description,
            TechStack = dto.TechStack,
            GitHubUrl = dto.GitHubUrl,
            DemoUrl = dto.DemoUrl,
            CoverImage = dto.CoverImage,
            IsFeatured = dto.IsFeatured,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.Projects.Add(project);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetProject), new { id = project.Id }, MapToDto(project));
    }

    /// <summary>
    /// 更新项目 - 需要管理员权限
    /// </summary>
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ProjectDto>> UpdateProject(int id, CreateProjectDto dto)
    {
        var project = await _context.Projects.FindAsync(id);

        if (project == null)
        {
            return NotFound(new { message = "项目不存在" });
        }

        project.Title = dto.Title;
        project.Description = dto.Description;
        project.TechStack = dto.TechStack;
        project.GitHubUrl = dto.GitHubUrl;
        project.DemoUrl = dto.DemoUrl;
        project.CoverImage = dto.CoverImage;
        project.IsFeatured = dto.IsFeatured;
        project.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return Ok(MapToDto(project));
    }

    /// <summary>
    /// 删除项目 - 需要管理员权限
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteProject(int id)
    {
        var project = await _context.Projects.FindAsync(id);

        if (project == null)
        {
            return NotFound(new { message = "项目不存在" });
        }

        _context.Projects.Remove(project);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    /// <summary>
    /// 实体转DTO映射
    /// </summary>
    private static ProjectDto MapToDto(Project project)
    {
        return new ProjectDto
        {
            Id = project.Id,
            Title = project.Title,
            Description = project.Description,
            TechStack = project.TechStack,
            GitHubUrl = project.GitHubUrl,
            DemoUrl = project.DemoUrl,
            CoverImage = project.CoverImage,
            IsFeatured = project.IsFeatured,
            CreatedAt = project.CreatedAt,
            UpdatedAt = project.UpdatedAt
        };
    }
}
