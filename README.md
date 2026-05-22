# 个人网站 - Personal Website

一个开箱即用的开源个人网站，采用前后端分离架构。

## 功能特性

- **首页** - 个人简介、最新文章和精选项目展示
- **关于我** - 个人详细介绍（Markdown格式）
- **博客** - 文章列表，支持分类筛选和搜索
- **项目展示** - 项目卡片，支持GitHub链接和在线演示
- **摄影作品** - 照片画廊，支持分类和灯箱预览
- **联系方式** - 联系信息和在线表单
- **私密日记** - 需要密码访问的日记功能
- **管理后台** - 完整的内容管理系统
- **主题切换** - 支持深色和浅色主题
- **评论系统** - 集成Giscus评论

## 技术栈

### 前端
- Vue 3 + TypeScript
- Vite构建工具
- Pinia状态管理
- Vue Router路由
- Element Plus UI组件库
- md-editor-v3 Markdown编辑器

### 后端
- ASP.NET Core 8 Web API
- Entity Framework Core
- SQL Server数据库
- JWT认证
- BCrypt密码加密

## 快速开始

### 环境要求

- Node.js 18+
- .NET 8 SDK
- SQL Server（或LocalDB）

### 启动后端

```bash
cd backend
dotnet run
```

后端将在 http://localhost:5000 启动

### 启动前端

```bash
cd frontend
npm install
npm run dev
```

前端将在 http://localhost:5173 启动

### 访问网站

- 前台首页：http://localhost:5173
- 管理后台：http://localhost:5173/admin
- API文档：http://localhost:5000/swagger

## 默认账号

- 管理员用户名：admin
- 管理员密码：admin123

## 项目结构

```
OwnWebSite/
├── frontend/              # Vue 3前端项目
│   ├── src/
│   │   ├── api/          # API请求封装
│   │   ├── components/   # 通用组件
│   │   ├── layouts/      # 布局组件
│   │   ├── router/       # 路由配置
│   │   ├── stores/       # 状态管理
│   │   ├── styles/       # 样式文件
│   │   ├── types/        # TypeScript类型
│   │   └── views/        # 页面视图
│   └── package.json
├── backend/               # ASP.NET Core后端
│   ├── Controllers/      # API控制器
│   ├── Data/             # 数据库上下文
│   ├── DTOs/             # 数据传输对象
│   ├── Models/           # 数据模型
│   └── Program.cs        # 入口文件
├── .claude/              # 项目文档
└── README.md
```

## 内容管理

所有内容都可以通过管理后台进行管理，无需修改源码：

1. **文章管理** - 创建、编辑、删除文章
2. **日记管理** - 写日记，支持心情和天气标签
3. **项目管理** - 添加和管理项目
4. **照片管理** - 上传和管理摄影作品
5. **网站设置** - 配置个人信息、联系方式等

## 主题定制

网站支持深色和浅色两种主题：

- 浅色主题：温馨的暖色调
- 深色主题：护眼的暗色模式

主题切换状态会保存在浏览器本地存储中。

## 开源协议

MIT License
