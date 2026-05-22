using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Data;
using backend.DTOs;
using backend.Models;

namespace backend.Controllers;

/// <summary>
/// 网站设置控制器 - 处理网站全局配置
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class SettingsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public SettingsController(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// 获取网站设置 - 公开接口（不返回敏感信息）
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<SiteSettingsDto>> GetSettings()
    {
        var settings = await _context.SiteSettings.FirstOrDefaultAsync();

        if (settings == null)
        {
            // 如果没有设置，返回空对象
            return Ok(new SiteSettingsDto());
        }

        return Ok(new SiteSettingsDto
        {
            Id = settings.Id,
            SiteName = settings.SiteName,
            SiteDescription = settings.SiteDescription,
            Avatar = settings.Avatar,
            AboutContent = settings.AboutContent,
            Email = settings.Email,
            GitHub = settings.GitHub,
            Twitter = settings.Twitter,
            WeChat = settings.WeChat,
            QQ = settings.QQ,
            GiscusRepo = settings.GiscusRepo,
            GiscusRepoId = settings.GiscusRepoId,
            GiscusCategory = settings.GiscusCategory,
            GiscusCategoryId = settings.GiscusCategoryId
        });
    }

    /// <summary>
    /// 更新网站设置 - 需要管理员权限
    /// </summary>
    [HttpPut]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<SiteSettingsDto>> UpdateSettings(UpdateSiteSettingsDto dto)
    {
        var settings = await _context.SiteSettings.FirstOrDefaultAsync();

        if (settings == null)
        {
            // 首次设置，创建新记录
            settings = new SiteSettings();
            _context.SiteSettings.Add(settings);
        }

        // 更新基本设置
        settings.SiteName = dto.SiteName;
        settings.SiteDescription = dto.SiteDescription;
        settings.Avatar = dto.Avatar;
        settings.AboutContent = dto.AboutContent;
        settings.Email = dto.Email;
        settings.GitHub = dto.GitHub;
        settings.Twitter = dto.Twitter;
        settings.WeChat = dto.WeChat;
        settings.QQ = dto.QQ;

        // 更新Giscus配置
        settings.GiscusRepo = dto.GiscusRepo;
        settings.GiscusRepoId = dto.GiscusRepoId;
        settings.GiscusCategory = dto.GiscusCategory;
        settings.GiscusCategoryId = dto.GiscusCategoryId;

        // 更新私密区域密码（如果提供）
        if (!string.IsNullOrEmpty(dto.PrivatePassword))
        {
            settings.PrivatePassword = BCrypt.Net.BCrypt.HashPassword(dto.PrivatePassword);
        }

        await _context.SaveChangesAsync();

        return Ok(new SiteSettingsDto
        {
            Id = settings.Id,
            SiteName = settings.SiteName,
            SiteDescription = settings.SiteDescription,
            Avatar = settings.Avatar,
            AboutContent = settings.AboutContent,
            Email = settings.Email,
            GitHub = settings.GitHub,
            Twitter = settings.Twitter,
            WeChat = settings.WeChat,
            QQ = settings.QQ,
            GiscusRepo = settings.GiscusRepo,
            GiscusRepoId = settings.GiscusRepoId,
            GiscusCategory = settings.GiscusCategory,
            GiscusCategoryId = settings.GiscusCategoryId
        });
    }
}
