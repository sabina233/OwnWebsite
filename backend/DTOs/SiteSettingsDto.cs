namespace backend.DTOs;

/// <summary>
/// 网站设置数据传输对象 - 用于API请求和响应
/// </summary>
public class SiteSettingsDto
{
    public int Id { get; set; }
    public string? SiteName { get; set; }
    public string? SiteDescription { get; set; }
    public string? Avatar { get; set; }
    public string? AboutContent { get; set; }
    public string? Email { get; set; }
    public string? GitHub { get; set; }
    public string? Twitter { get; set; }
    public string? WeChat { get; set; }
    public string? QQ { get; set; }
    public string? GiscusRepo { get; set; }
    public string? GiscusRepoId { get; set; }
    public string? GiscusCategory { get; set; }
    public string? GiscusCategoryId { get; set; }
}

/// <summary>
/// 更新网站设置请求
/// </summary>
public class UpdateSiteSettingsDto
{
    public string? SiteName { get; set; }
    public string? SiteDescription { get; set; }
    public string? Avatar { get; set; }
    public string? AboutContent { get; set; }
    public string? Email { get; set; }
    public string? GitHub { get; set; }
    public string? Twitter { get; set; }
    public string? WeChat { get; set; }
    public string? QQ { get; set; }
    public string? PrivatePassword { get; set; }
    public string? GiscusRepo { get; set; }
    public string? GiscusRepoId { get; set; }
    public string? GiscusCategory { get; set; }
    public string? GiscusCategoryId { get; set; }
}
