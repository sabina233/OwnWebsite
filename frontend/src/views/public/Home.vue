<template>
  <!-- 首页 - 展示个人简介和最新内容 -->
  <div class="home">
    <!-- 个人介绍区域 - 英雄区 -->
    <section class="hero">
      <div class="container">
        <div class="hero-content">
          <!-- 头像 -->
          <div class="avatar-wrapper">
            <div class="avatar">
              <img :src="settings.avatar || '/default-avatar.png'" alt="头像" />
            </div>
            <div class="avatar-decoration"></div>
          </div>

          <!-- 个人信息 -->
          <h1 class="hero-title">{{ settings.siteName || '欢迎来到我的网站' }}</h1>
          <p class="hero-subtitle">{{ settings.siteDescription || '这里是我的个人空间' }}</p>

          <!-- 社交链接 -->
          <div class="social-links">
            <a v-if="settings.gitHub" :href="settings.gitHub" target="_blank" class="social-link">
              <span>GitHub</span>
            </a>
            <a v-if="settings.twitter" :href="settings.twitter" target="_blank" class="social-link">
              <span>Twitter</span>
            </a>
            <a v-if="settings.email" :href="`mailto:${settings.email}`" class="social-link">
              <span>邮箱</span>
            </a>
          </div>
        </div>
      </div>

      <!-- 装饰背景 -->
      <div class="hero-bg">
        <div class="hero-circle hero-circle--1"></div>
        <div class="hero-circle hero-circle--2"></div>
      </div>
    </section>

    <!-- 最新文章 -->
    <section class="section" v-if="articles.length > 0">
      <div class="container">
        <div class="section-header">
          <h2 class="section-title">最新文章</h2>
          <router-link to="/blog" class="section-link">查看全部 →</router-link>
        </div>

        <div class="article-grid">
          <div v-for="article in articles" :key="article.id" class="article-card card">
            <div class="card-cover" v-if="article.coverImage">
              <img :src="article.coverImage" :alt="article.title" loading="lazy" />
            </div>
            <div class="card-body">
              <span class="card-category" v-if="article.category">{{ article.category }}</span>
              <h3 class="card-title">
                <router-link :to="`/article/${article.id}`">{{ article.title }}</router-link>
              </h3>
              <p class="card-summary">{{ article.summary }}</p>
              <div class="card-meta">
                <span class="card-date">{{ formatDate(article.createdAt) }}</span>
                <span class="card-views">{{ article.viewCount }} 次阅读</span>
              </div>
            </div>
          </div>
        </div>
      </div>
    </section>

    <!-- 精选项目 -->
    <section class="section section--alt" v-if="projects.length > 0">
      <div class="container">
        <div class="section-header">
          <h2 class="section-title">精选项目</h2>
          <router-link to="/projects" class="section-link">查看全部 →</router-link>
        </div>

        <div class="project-grid">
          <div v-for="project in projects" :key="project.id" class="project-card card">
            <div class="card-cover" v-if="project.coverImage">
              <img :src="project.coverImage" :alt="project.title" loading="lazy" />
            </div>
            <div class="card-body">
              <h3 class="card-title">{{ project.title }}</h3>
              <p class="card-summary">{{ project.description }}</p>
              <div class="tech-stack" v-if="project.techStack">
                <span v-for="tech in parseTechStack(project.techStack)" :key="tech" class="tech-tag">
                  {{ tech }}
                </span>
              </div>
              <div class="project-links" v-if="project.gitHubUrl || project.demoUrl">
                <a v-if="project.gitHubUrl" :href="project.gitHubUrl" target="_blank" class="project-link">
                  GitHub
                </a>
                <a v-if="project.demoUrl" :href="project.demoUrl" target="_blank" class="project-link project-link--primary">
                  演示
                </a>
              </div>
            </div>
          </div>
        </div>
      </div>
    </section>
  </div>
</template>

<script setup lang="ts">
/**
 * 首页组件
 * 展示个人简介、最新文章和精选项目
 */
import { ref, onMounted } from 'vue'
import { getArticles, getProjects, getSettings } from '@/api'
import type { ArticleListItem, Project, SiteSettings } from '@/types'

// 网站设置
const settings = ref<SiteSettings>({})

// 最新文章列表
const articles = ref<ArticleListItem[]>([])

// 精选项目列表
const projects = ref<Project[]>([])

/**
 * 格式化日期
 */
const formatDate = (dateStr: string) => {
  const date = new Date(dateStr)
  return date.toLocaleDateString('zh-CN', { year: 'numeric', month: 'long', day: 'numeric' })
}

/**
 * 解析技术栈标签
 */
const parseTechStack = (techStack: string): string[] => {
  try {
    return JSON.parse(techStack)
  } catch {
    return techStack.split(',').map(t => t.trim())
  }
}

// 初始化加载数据
onMounted(async () => {
  try {
    const [settingsRes, articlesRes, projectsRes] = await Promise.all([
      getSettings(),
      getArticles({ pageSize: 3 }),
      getProjects()
    ])

    settings.value = settingsRes.data
    articles.value = articlesRes.data.items
    projects.value = projectsRes.data.filter(p => p.isFeatured).slice(0, 3)
  } catch {
    // 静默处理
  }
})
</script>

<style scoped>
/* ========== 英雄区域 ========== */
.hero {
  position: relative;
  padding: 100px 0 80px;
  text-align: center;
  overflow: hidden;
  background: linear-gradient(135deg, var(--bg-secondary), var(--bg-primary));
}

.hero-content {
  position: relative;
  z-index: 2;
}

/* 头像 */
.avatar-wrapper {
  position: relative;
  display: inline-block;
  margin-bottom: 32px;
}

.avatar {
  width: 140px;
  height: 140px;
  border-radius: 50%;
  overflow: hidden;
  border: 4px solid var(--bg-card);
  box-shadow: var(--shadow-lg);
  position: relative;
  z-index: 2;
}

.avatar img {
  width: 100%;
  height: 100%;
  object-fit: cover;
  display: block;
}

.avatar-decoration {
  position: absolute;
  top: -8px;
  left: -8px;
  right: -8px;
  bottom: -8px;
  border-radius: 50%;
  border: 2px dashed var(--accent-primary);
  opacity: 0.5;
  animation: rotate 20s linear infinite;
}

@keyframes rotate {
  from { transform: rotate(0deg); }
  to { transform: rotate(360deg); }
}

/* 标题和描述 */
.hero-title {
  font-size: 2.5rem;
  font-weight: 700;
  margin-bottom: 16px;
  background: linear-gradient(135deg, var(--text-primary), var(--accent-primary));
  -webkit-background-clip: text;
  -webkit-text-fill-color: transparent;
  background-clip: text;
}

.hero-subtitle {
  font-size: 1.125rem;
  color: var(--text-secondary);
  max-width: 500px;
  margin: 0 auto 32px;
  line-height: 1.7;
}

/* 社交链接 */
.social-links {
  display: flex;
  justify-content: center;
  gap: 12px;
  flex-wrap: wrap;
}

.social-link {
  display: inline-flex;
  align-items: center;
  padding: 10px 20px;
  background: var(--bg-card);
  color: var(--text-secondary);
  border-radius: var(--radius-md);
  font-size: 14px;
  font-weight: 500;
  transition: all var(--transition-fast);
  border: 1px solid var(--border-color);
  text-decoration: none;
}

.social-link:hover {
  color: var(--accent-primary);
  border-color: var(--accent-primary);
  transform: translateY(-2px);
  box-shadow: var(--shadow-md);
}

/* 装饰背景 */
.hero-bg {
  position: absolute;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  z-index: 1;
  overflow: hidden;
}

.hero-circle {
  position: absolute;
  border-radius: 50%;
  opacity: 0.1;
}

.hero-circle--1 {
  width: 400px;
  height: 400px;
  background: var(--accent-primary);
  top: -100px;
  right: -100px;
}

.hero-circle--2 {
  width: 300px;
  height: 300px;
  background: var(--accent-secondary);
  bottom: -50px;
  left: -50px;
}

/* ========== 通用区域 ========== */
.section {
  padding: 80px 0;
}

.section--alt {
  background: var(--bg-secondary);
}

.section-header {
  display: flex;
  align-items: center;
  justify-content: space-between;
  margin-bottom: 40px;
}

.section-title {
  font-size: 1.75rem;
  font-weight: 600;
  color: var(--text-primary);
}

.section-link {
  color: var(--accent-primary);
  font-size: 14px;
  font-weight: 500;
  transition: color var(--transition-fast);
  text-decoration: none;
}

.section-link:hover {
  color: var(--accent-secondary);
}

/* ========== 文章网格 ========== */
.article-grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(320px, 1fr));
  gap: 24px;
}

.article-card {
  overflow: hidden;
  display: flex;
  flex-direction: column;
}

.card-cover {
  height: 200px;
  overflow: hidden;
  margin: -24px -24px 0;
  margin-bottom: 0;
}

.card-cover img {
  width: 100%;
  height: 100%;
  object-fit: cover;
  transition: transform var(--transition-slow);
}

.article-card:hover .card-cover img {
  transform: scale(1.05);
}

.card-body {
  padding: 20px 0;
  flex: 1;
  display: flex;
  flex-direction: column;
}

.article-card .card-body {
  padding: 20px;
}

.card-category {
  display: inline-block;
  padding: 4px 12px;
  background: var(--accent-light);
  color: var(--accent-primary);
  border-radius: 20px;
  font-size: 12px;
  font-weight: 500;
  margin-bottom: 12px;
  width: fit-content;
}

.card-title {
  font-size: 1.125rem;
  font-weight: 600;
  margin-bottom: 8px;
  line-height: 1.4;
}

.card-title a {
  color: var(--text-primary);
  text-decoration: none;
  transition: color var(--transition-fast);
}

.card-title a:hover {
  color: var(--accent-primary);
}

.card-summary {
  color: var(--text-secondary);
  font-size: 14px;
  line-height: 1.6;
  margin-bottom: 16px;
  flex: 1;
  display: -webkit-box;
  -webkit-line-clamp: 3;
  -webkit-box-orient: vertical;
  overflow: hidden;
}

.card-meta {
  display: flex;
  justify-content: space-between;
  font-size: 13px;
  color: var(--text-muted);
}

/* ========== 项目网格 ========== */
.project-grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(350px, 1fr));
  gap: 24px;
}

.project-card {
  overflow: hidden;
}

.tech-stack {
  display: flex;
  flex-wrap: wrap;
  gap: 8px;
  margin: 12px 0;
}

.tech-tag {
  padding: 4px 10px;
  background: var(--bg-secondary);
  color: var(--text-secondary);
  border-radius: 12px;
  font-size: 12px;
  font-weight: 500;
}

.project-links {
  display: flex;
  gap: 12px;
  margin-top: auto;
  padding-top: 16px;
}

.project-link {
  padding: 8px 16px;
  border-radius: var(--radius-md);
  font-size: 13px;
  font-weight: 500;
  transition: all var(--transition-fast);
  text-decoration: none;
  color: var(--text-secondary);
  border: 1px solid var(--border-color);
}

.project-link:hover {
  color: var(--accent-primary);
  border-color: var(--accent-primary);
}

.project-link--primary {
  background: var(--accent-primary);
  color: white;
  border-color: var(--accent-primary);
}

.project-link--primary:hover {
  background: var(--accent-secondary);
  color: white;
}

/* ========== 响应式布局 ========== */
@media (max-width: 768px) {
  .hero {
    padding: 60px 0 50px;
  }

  .hero-title {
    font-size: 1.75rem;
  }

  .hero-subtitle {
    font-size: 1rem;
  }

  .section {
    padding: 50px 0;
  }

  .section-header {
    margin-bottom: 24px;
  }

  .article-grid,
  .project-grid {
    grid-template-columns: 1fr;
  }
}

@media (max-width: 480px) {
  .hero {
    padding: 40px 0;
  }

  .avatar {
    width: 120px;
    height: 120px;
  }

  .hero-title {
    font-size: 1.5rem;
  }

  .social-links {
    flex-direction: column;
    align-items: center;
  }

  .social-link {
    width: 200px;
    justify-content: center;
  }
}
</style>
