# 后端项目文档

## 项目目标

为个人网站提供RESTful API后端服务，支持文章管理、日记管理、项目展示、相册管理、联系表单等功能。

## 技术栈

- ASP.NET Core 8 Web API
- Entity Framework Core
- SQL Server
- JWT认证
- BCrypt密码加密

## 已实现功能

### API接口

- [x] 管理员登录认证
- [x] 文章CRUD（创建、读取、更新、删除）
- [x] 日记CRUD（需要私密区域权限）
- [x] 项目CRUD
- [x] 照片上传和管理
- [x] 分类管理
- [x] 联系表单提交
- [x] 网站设置管理
- [x] 文件上传（头像、封面、照片）

### 数据库

- [x] Articles表 - 文章存储
- [x] Diaries表 - 日记存储
- [x] Projects表 - 项目存储
- [x] Photos表 - 照片存储
- [x] Categories表 - 分类管理
- [x] SiteSettings表 - 网站设置
- [x] AdminUsers表 - 管理员账号
- [x] ContactMessages表 - 联系消息

## 当前进度

项目已完成基础功能开发，可以启动运行。

## 启动方式

```bash
cd backend
dotnet run
```

默认管理员账号：admin / admin123
