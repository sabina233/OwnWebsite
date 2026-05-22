using Microsoft.EntityFrameworkCore;
using backend.Models;

namespace backend.Data;

/// <summary>
/// 应用程序数据库上下文 - 管理所有实体的数据访问
/// </summary>
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    /// <summary>文章表</summary>
    public DbSet<Article> Articles => Set<Article>();

    /// <summary>日记表</summary>
    public DbSet<Diary> Diaries => Set<Diary>();

    /// <summary>项目表</summary>
    public DbSet<Project> Projects => Set<Project>();

    /// <summary>照片表</summary>
    public DbSet<Photo> Photos => Set<Photo>();

    /// <summary>网站设置表</summary>
    public DbSet<SiteSettings> SiteSettings => Set<SiteSettings>();

    /// <summary>管理员用户表</summary>
    public DbSet<AdminUser> AdminUsers => Set<AdminUser>();

    /// <summary>文章分类表</summary>
    public DbSet<Category> Categories => Set<Category>();

    /// <summary>联系消息表</summary>
    public DbSet<ContactMessage> ContactMessages => Set<ContactMessage>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // 配置文章实体
        modelBuilder.Entity<Article>(entity =>
        {
            entity.HasIndex(e => e.Category);  // 为分类字段创建索引
            entity.HasIndex(e => e.IsPublished); // 为发布状态创建索引
            entity.HasIndex(e => e.CreatedAt);  // 为创建时间创建索引
        });

        // 配置日记实体
        modelBuilder.Entity<Diary>(entity =>
        {
            entity.HasIndex(e => e.CreatedAt); // 按日期排序需要索引
        });

        // 配置项目实体
        modelBuilder.Entity<Project>(entity =>
        {
            entity.HasIndex(e => e.IsFeatured); // 置顶查询需要索引
        });

        // 配置照片实体
        modelBuilder.Entity<Photo>(entity =>
        {
            entity.HasIndex(e => e.Category); // 按分类筛选需要索引
        });

        // 配置分类实体
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasIndex(e => e.Slug).IsUnique(); // Slug必须唯一
            entity.HasIndex(e => e.ParentId);          // 父分类查询需要索引
        });

        // 配置网站设置（单例模式，只有一条记录）
        modelBuilder.Entity<SiteSettings>(entity =>
        {
            entity.HasIndex(e => e.Id);
        });
    }
}
