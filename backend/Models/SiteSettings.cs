using System.ComponentModel.DataAnnotations;

namespace backend.Models;

/// <summary>
/// 网站设置实体模型 - 存储网站全局配置信息
/// </summary>
public class SiteSettings
{
    /// <summary>设置唯一标识</summary>
    public int Id { get; set; }

    /// <summary>网站名称</summary>
    [MaxLength(100)]
    public string? SiteName { get; set; }

    /// <summary>网站描述</summary>
    [MaxLength(500)]
    public string? SiteDescription { get; set; }

    /// <summary>头像图片路径</summary>
    [MaxLength(500)]
    public string? Avatar { get; set; }

    /// <summary>关于我页面内容（Markdown格式）</summary>
    public string? AboutContent { get; set; }

    /// <summary>联系邮箱</summary>
    [MaxLength(200)]
    public string? Email { get; set; }

    /// <summary>GitHub主页地址</summary>
    [MaxLength(200)]
    public string? GitHub { get; set; }

    /// <summary>Twitter主页地址</summary>
    [MaxLength(200)]
    public string? Twitter { get; set; }

    /// <summary>微信号</summary>
    [MaxLength(200)]
    public string? WeChat { get; set; }

    /// <summary>QQ号</summary>
    [MaxLength(200)]
    public string? QQ { get; set; }

    /// <summary>私密区域访问密码（哈希存储）</summary>
    [MaxLength(200)]
    public string? PrivatePassword { get; set; }

    /// <summary>Giscus评论 - 仓库名称（如 username/repo）</summary>
    [MaxLength(200)]
    public string? GiscusRepo { get; set; }

    /// <summary>Giscus评论 - 仓库ID</summary>
    [MaxLength(200)]
    public string? GiscusRepoId { get; set; }

    /// <summary>Giscus评论 - 分类名称</summary>
    [MaxLength(200)]
    public string? GiscusCategory { get; set; }

    /// <summary>Giscus评论 - 分类ID</summary>
    [MaxLength(200)]
    public string? GiscusCategoryId { get; set; }
}
