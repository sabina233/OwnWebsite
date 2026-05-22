using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using backend.Data;

var builder = WebApplication.CreateBuilder(args);

// ========== 配置数据库连接 ==========
// 使用SQL Server数据库
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ========== 配置JWT认证 ==========
var jwtKey = builder.Configuration["Jwt:Key"] ?? "YourSuperSecretKeyHere12345678901234";
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,              // 验证发行者
            ValidateAudience = true,            // 验证受众
            ValidateLifetime = true,            // 验证有效期
            ValidateIssuerSigningKey = true,    // 验证签名密钥
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });

builder.Services.AddAuthorization();

// ========== 配置控制器 ==========
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // 配置JSON序列化：使用驼峰命名
        options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
    });

// ========== 配置Swagger/OpenAPI ==========
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ========== 配置CORS跨域 ==========
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()     // 允许任何来源（开发环境）
              .AllowAnyMethod()     // 允许任何HTTP方法
              .AllowAnyHeader();    // 允许任何请求头
    });
});

var app = builder.Build();

// ========== 配置中间件管道 ==========

// 开发环境启用Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// 启用CORS
app.UseCors("AllowAll");

// 启用静态文件服务（用于访问上传的图片）
app.UseStaticFiles();

// 启用认证和授权
app.UseAuthentication();
app.UseAuthorization();

// 映射控制器路由
app.MapControllers();

// ========== 初始化数据库 ==========
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    // 确保数据库已创建（开发环境自动迁移）
    context.Database.EnsureCreated();

    // 初始化默认管理员账号（如果不存在）
    if (!context.AdminUsers.Any())
    {
        context.AdminUsers.Add(new backend.Models.AdminUser
        {
            Username = "admin",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123")
        });
        context.SaveChanges();
    }

    // 初始化默认网站设置（如果不存在）
    if (!context.SiteSettings.Any())
    {
        context.SiteSettings.Add(new backend.Models.SiteSettings
        {
            SiteName = "我的个人网站",
            SiteDescription = "欢迎来到我的个人网站"
        });
        context.SaveChanges();
    }
}

app.Run();
