/**
 * API请求封装 - 统一管理所有HTTP请求
 */
import axios from 'axios'
import type {
  Article, ArticleListItem, Diary, DiaryListItem, Project, Photo,
  SiteSettings, Category, ContactMessage, PagedResult,
  LoginRequest, LoginResponse, ContactRequest
} from '@/types'

// 创建axios实例
const api = axios.create({
  baseURL: '/api',
  timeout: 10000,
  headers: {
    'Content-Type': 'application/json'
  }
})

// 请求拦截器：自动添加认证Token
api.interceptors.request.use((config) => {
  // 管理员Token
  const token = localStorage.getItem('admin_token')
  if (token) {
    config.headers.Authorization = `Bearer ${token}`
  }
  return config
})

// 响应拦截器：处理401错误
api.interceptors.response.use(
  (response) => response,
  (error) => {
    if (error.response?.status === 401) {
      // Token过期，清除本地存储
      localStorage.removeItem('admin_token')
      localStorage.removeItem('private_token')
    }
    return Promise.reject(error)
  }
)

// ========== 认证相关API ==========

/** 管理员登录 */
export const login = (data: LoginRequest) =>
  api.post<LoginResponse>('/auth/login', data)

/** 验证私密区域密码 */
export const verifyPassword = (password: string) =>
  api.post<{ success: boolean; token?: string; expiresAt?: string }>('/auth/verify-password', { password })

// ========== 文章相关API ==========

/** 获取文章列表 */
export const getArticles = (params?: {
  page?: number
  pageSize?: number
  category?: string
  search?: string
}) => api.get<PagedResult<ArticleListItem>>('/articles', { params })

/** 获取文章详情 */
export const getArticle = (id: number) =>
  api.get<Article>(`/articles/${id}`)

/** 创建文章（管理员） */
export const createArticle = (data: Partial<Article>) =>
  api.post<Article>('/articles', data)

/** 更新文章（管理员） */
export const updateArticle = (id: number, data: Partial<Article>) =>
  api.put<Article>(`/articles/${id}`, data)

/** 删除文章（管理员） */
export const deleteArticle = (id: number) =>
  api.delete(`/articles/${id}`)

// ========== 日记相关API ==========

/** 获取日记列表（需要私密权限） */
export const getDiaries = (params?: { page?: number; pageSize?: number }) =>
  api.get<PagedResult<DiaryListItem>>('/diary', { params })

/** 获取日记详情（需要私密权限） */
export const getDiary = (id: number) =>
  api.get<Diary>(`/diary/${id}`)

/** 创建日记（管理员） */
export const createDiary = (data: Partial<Diary>) =>
  api.post<Diary>('/diary', data)

/** 更新日记（管理员） */
export const updateDiary = (id: number, data: Partial<Diary>) =>
  api.put<Diary>(`/diary/${id}`, data)

/** 删除日记（管理员） */
export const deleteDiary = (id: number) =>
  api.delete(`/diary/${id}`)

// ========== 项目相关API ==========

/** 获取项目列表 */
export const getProjects = () =>
  api.get<Project[]>('/projects')

/** 获取项目详情 */
export const getProject = (id: number) =>
  api.get<Project>(`/projects/${id}`)

/** 创建项目（管理员） */
export const createProject = (data: Partial<Project>) =>
  api.post<Project>('/projects', data)

/** 更新项目（管理员） */
export const updateProject = (id: number, data: Partial<Project>) =>
  api.put<Project>(`/projects/${id}`, data)

/** 删除项目（管理员） */
export const deleteProject = (id: number) =>
  api.delete(`/projects/${id}`)

// ========== 相册相关API ==========

/** 获取照片列表 */
export const getPhotos = (params?: {
  page?: number
  pageSize?: number
  category?: string
}) => api.get<PagedResult<Photo>>('/gallery/photos', { params })

/** 删除照片（管理员） */
export const deletePhoto = (id: number) =>
  api.delete(`/gallery/photos/${id}`)

// ========== 分类相关API ==========

/** 获取所有分类 */
export const getCategories = () =>
  api.get<Category[]>('/categories')

/** 创建分类（管理员） */
export const createCategory = (data: Partial<Category>) =>
  api.post<Category>('/categories', data)

/** 更新分类（管理员） */
export const updateCategory = (id: number, data: Partial<Category>) =>
  api.put<Category>(`/categories/${id}`, data)

/** 删除分类（管理员） */
export const deleteCategory = (id: number) =>
  api.delete(`/categories/${id}`)

// ========== 联系相关API ==========

/** 提交联系表单 */
export const submitContact = (data: ContactRequest) =>
  api.post('/contact', data)

/** 获取联系消息列表（管理员） */
export const getContactMessages = () =>
  api.get<ContactMessage[]>('/contact/messages')

/** 标记消息为已读（管理员） */
export const markMessageAsRead = (id: number) =>
  api.put(`/contact/messages/${id}/read`)

/** 删除联系消息（管理员） */
export const deleteContactMessage = (id: number) =>
  api.delete(`/contact/messages/${id}`)

// ========== 设置相关API ==========

/** 获取网站设置 */
export const getSettings = () =>
  api.get<SiteSettings>('/settings')

/** 更新网站设置（管理员） */
export const updateSettings = (data: Partial<SiteSettings>) =>
  api.put<SiteSettings>('/settings', data)

// ========== 文件上传API ==========

/** 上传照片 */
export const uploadPhoto = (file: File, data?: { title?: string; description?: string; category?: string }) => {
  const formData = new FormData()
  formData.append('file', file)
  if (data?.title) formData.append('title', data.title)
  if (data?.description) formData.append('description', data.description)
  if (data?.category) formData.append('category', data.category)
  return api.post<Photo>('/upload/photo', formData, {
    headers: { 'Content-Type': 'multipart/form-data' }
  })
}

/** 上传封面图片 */
export const uploadCover = (file: File) => {
  const formData = new FormData()
  formData.append('file', file)
  return api.post<{ url: string }>('/upload/cover', formData, {
    headers: { 'Content-Type': 'multipart/form-data' }
  })
}

/** 上传头像 */
export const uploadAvatar = (file: File) => {
  const formData = new FormData()
  formData.append('file', file)
  return api.post<{ url: string }>('/upload/avatar', formData, {
    headers: { 'Content-Type': 'multipart/form-data' }
  })
}

export default api
