# 个人网站项目文档

## 项目目标

创建一个开箱即用的开源个人网站，用于自我介绍、写文章、项目展示、摄影作品展示和联系方式展示。

## 核心特性

- 前后端分离架构
- 内容管理无需修改源码
- 支持深浅主题切换
- 温馨的视觉风格
- 私密日记功能
- Giscus评论系统

## 技术栈

### 前端
- Vue 3 + TypeScript
- Vite + Pinia + Vue Router
- Element Plus UI
- md-editor-v3 Markdown编辑器

### 后端
- ASP.NET Core 8 Web API
- Entity Framework Core
- SQL Server
- JWT认证

## 项目结构

```
OwnWebSite/
├── frontend/          # Vue 3前端项目
├── backend/           # ASP.NET Core后端项目
├── .claude/           # 项目文档
└── README.md          # 项目说明
```

## 快速开始

### 启动后端

```bash
cd backend
dotnet run
```

### 启动前端

```bash
cd frontend
npm install
npm run dev
```

### 访问网站

- 前台首页：http://localhost:5173
- 管理后台：http://localhost:5173/admin
- API文档：http://localhost:5000/swagger

### 默认账号

- 管理员：admin / admin123

## 功能模块

1. **首页** - 个人简介和最新内容
2. **关于我** - 个人详细介绍
3. **博客** - 文章列表和详情
4. **项目展示** - 项目卡片
5. **摄影作品** - 照片画廊
6. **联系方式** - 联系表单
7. **私密日记** - 需要密码访问
8. **管理后台** - 内容管理

## 当前进度

- [x] 后端API开发完成
- [x] 前端界面开发完成
- [x] 数据库设计完成
- [x] 认证系统完成
- [ ] 部署配置
