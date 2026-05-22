<template>
  <!-- 首页 - 展示个人简介和最新内容 -->
  <div class="home">
    <!-- 个人介绍区域 -->
    <section class="hero">
      <div class="container">
        <div class="hero-content">
          <div class="avatar">
            <img :src="settings.avatar || '/default-avatar.png'" alt="头像" />
          </div>
          <h1 class="title">{{ settings.siteName || '欢迎来到我的网站' }}</h1>
          <p class="subtitle">{{ settings.siteDescription || '这里是我的个人空间' }}</p>
          <div class="social-links">
            <a v-if="settings.gitHub" :href="settings.gitHub" target="_blank">GitHub</a>
            <a v-if="settings.twitter" :href="settings.twitter" target="_blank">Twitter</a>
            <a v-if="settings.email" :href="`mailto:${settings.email}`">邮箱</a>
          </div>
        </div>
      </div>
    </section>

    <!-- 最新文章 -->
    <section class="section">
      <div class="container">
        <h2 class="section-title">最新文章</h2>
        <div class="article-grid">
          <div v-for="article in articles" :key="article.id" class="article-card card">
            <div class="card-cover" v-if="article.coverImage">
              <img :src="article.coverImage" :alt="article.title" />
            </div>
            <div class="card-body">
              <span class="category" v-if="article.category">{{ article.category }}</span>
              <h3 class="card-title">
                <router-link :to="`/article/${article.id}`">{{ article.title }}</router-link>
              </h3>
              <p class="card-summary">{{ article.summary }}</p>
              <div class="card-meta">
                <span>{{ formatDate(article.createdAt) }}</span>
                <span>{{ article.viewCount }} 次阅读</span>
              </div>
            </div>
          </div>
        </div>
        <div class="more-link">
          <router-link to="/blog">查看更多文章 →</router-link>
        </div>
      </div>
    </section>

    <!-- 精选项目 -->
    <section class="section" v-if="projects.length > 0">
      <div class="container">
        <h2 class="section-title">精选项目</h2>
        <div class="project-grid">
          <div v-for="project in projects" :key="project.id" class="project-card card">
            <div class="card-cover" v-if="project.coverImage">
              <img :src="project.coverImage" :alt="project.title" />
            </div>
            <div class="card-body">
              <h3 class="card-title">{{ project.title }}</h3>
              <p class="card-summary">{{ project.description }}</p>
              <div class="tech-stack" v-if="project.techStack">
                <span v-for="tech in parseTechStack(project.techStack)" :key="tech" class="tech-tag">
                  {{ tech }}
                </span>
              </div>
            </div>
          </div>
        </div>
        <div class="more-link">
          <router-link to="/projects">查看所有项目 →</router-link>
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
 * 解析技术栈标签（JSON字符串转数组）
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
    // 并行请求数据
    const [settingsRes, articlesRes, projectsRes] = await Promise.all([
      getSettings(),
      getArticles({ pageSize: 3 }),
      getProjects()
    ])

    settings.value = settingsRes.data
    articles.value = articlesRes.data.items
    // 只显示置顶项目
    projects.value = projectsRes.data.filter(p => p.isFeatured).slice(0, 3)
  } catch {
    // 静默处理错误
  }
})
</script>

<style scoped>
/* 英雄区域 */
.hero {
  background: linear-gradient(135deg, var(--bg-secondary), var(--bg-primary));
  padding: 80px 0;
  text-align: center;
}

.hero-content {
  max-width: 600px;
  margin: 0 auto;
}

.avatar {
  width: 120px;
  height: 120px;
  margin: 0 auto 24px;
  border-radius: 50%;
  overflow: hidden;
  border: 4px solid var(--accent-primary);
}

.avatar img {
  width: 100%;
  height: 100%;
  object-fit: cover;
}

.title {
  font-size: 32px;
  margin-bottom: 12px;
  color: var(--text-primary);
}

.subtitle {
  font-size: 18px;
  color: var(--text-secondary);
  margin-bottom: 24px;
}

.social-links {
  display: flex;
  justify-content: center;
  gap: 16px;
}

.social-links a {
  padding: 8px 16px;
  background: var(--accent-primary);
  color: white;
  border-radius: var(--radius);
  transition: background 0.2s ease;
}

.social-links a:hover {
  background: var(--accent-secondary);
}

/* 通用区域样式 */
.section {
  padding: 60px 0;
}

.section-title {
  font-size: 24px;
  margin-bottom: 32px;
  color: var(--text-primary);
}

/* 文章网格 */
.article-grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(300px, 1fr));
  gap: 24px;
}

.article-card {
  overflow: hidden;
}

.card-cover {
  height: 200px;
  overflow: hidden;
}

.card-cover img {
  width: 100%;
  height: 100%;
  object-fit: cover;
  transition: transform 0.3s ease;
}

.card-cover:hover img {
  transform: scale(1.05);
}

.card-body {
  padding: 20px;
}

.category {
  display: inline-block;
  padding: 4px 12px;
  background: var(--bg-secondary);
  color: var(--accent-primary);
  border-radius: 16px;
  font-size: 12px;
  margin-bottom: 12px;
}

.card-title {
  font-size: 18px;
  margin-bottom: 8px;
}

.card-title a {
  color: var(--text-primary);
}

.card-title a:hover {
  color: var(--accent-primary);
}

.card-summary {
  color: var(--text-secondary);
  font-size: 14px;
  line-height: 1.6;
  margin-bottom: 12px;
  display: -webkit-box;
  -webkit-line-clamp: 3;
  -webkit-box-orient: vertical;
  overflow: hidden;
}

.card-meta {
  display: flex;
  justify-content: space-between;
  font-size: 12px;
  color: var(--text-secondary);
}

/* 项目网格 */
.project-grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(350px, 1fr));
  gap: 24px;
}

.tech-stack {
  display: flex;
  flex-wrap: wrap;
  gap: 8px;
  margin-top: 12px;
}

.tech-tag {
  padding: 4px 10px;
  background: var(--bg-secondary);
  color: var(--accent-primary);
  border-radius: 12px;
  font-size: 12px;
}

/* 更多链接 */
.more-link {
  text-align: center;
  margin-top: 32px;
}

.more-link a {
  color: var(--accent-primary);
  font-size: 16px;
}
</style>
