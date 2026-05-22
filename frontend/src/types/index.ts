/**
 * TypeScript类型定义 - 定义所有数据模型和API响应类型
 */

/** 文章类型 */
export interface Article {
  id: number
  title: string
  content: string
  summary?: string
  coverImage?: string
  category?: string
  tags?: string
  isPublished: boolean
  isPrivate: boolean
  viewCount: number
  createdAt: string
  updatedAt: string
}

/** 文章列表项（不含完整内容） */
export interface ArticleListItem {
  id: number
  title: string
  summary?: string
  coverImage?: string
  category?: string
  tags?: string
  viewCount: number
  createdAt: string
}

/** 日记类型 */
export interface Diary {
  id: number
  title: string
  content: string
  mood?: string
  weather?: string
  createdAt: string
  updatedAt: string
}

/** 日记列表项 */
export interface DiaryListItem {
  id: number
  title: string
  mood?: string
  weather?: string
  createdAt: string
}

/** 项目类型 */
export interface Project {
  id: number
  title: string
  description: string
  techStack?: string
  gitHubUrl?: string
  demoUrl?: string
  coverImage?: string
  isFeatured: boolean
  createdAt: string
  updatedAt: string
}

/** 照片类型 */
export interface Photo {
  id: number
  title?: string
  description?: string
  filePath: string
  thumbnailPath?: string
  category?: string
  createdAt: string
}

/** 网站设置类型 */
export interface SiteSettings {
  id: number
  siteName?: string
  siteDescription?: string
  avatar?: string
  aboutContent?: string
  email?: string
  gitHub?: string
  twitter?: string
  weChat?: string
  qq?: string
  giscusRepo?: string
  giscusRepoId?: string
  giscusCategory?: string
  giscusCategoryId?: string
}

/** 文章分类类型 */
export interface Category {
  id: number
  name: string
  slug: string
  description?: string
  parentId?: number
  sortOrder: number
  createdAt: string
}

/** 联系消息类型 */
export interface ContactMessage {
  id: number
  name: string
  email: string
  subject?: string
  message: string
  isRead: boolean
  createdAt: string
}

/** 分页结果类型 */
export interface PagedResult<T> {
  items: T[]
  page: number
  pageSize: number
  totalCount: number
  totalPages: number
  hasPreviousPage: boolean
  hasNextPage: boolean
}

/** 登录请求类型 */
export interface LoginRequest {
  username: string
  password: string
}

/** 登录响应类型 */
export interface LoginResponse {
  token: string
  expiresAt: string
}

/** 联系表单请求类型 */
export interface ContactRequest {
  name: string
  email: string
  subject?: string
  message: string
}
