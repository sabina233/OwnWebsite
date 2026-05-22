<template>
  <!-- 文章详情页 -->
  <div class="article-detail">
    <div class="container">
      <div class="article-card card" v-if="article">
        <!-- 文章头部 -->
        <header class="article-header">
          <span class="category" v-if="article.category">{{ article.category }}</span>
          <h1 class="article-title">{{ article.title }}</h1>
          <div class="article-meta">
            <span>{{ formatDate(article.createdAt) }}</span>
            <span>{{ article.viewCount }} 次阅读</span>
          </div>
        </header>

        <!-- 文章内容 -->
        <div class="article-body markdown-body" v-html="articleContent"></div>

        <!-- Giscus评论区 -->
        <div class="comments-section" v-if="settings.giscusRepo">
          <h3>评论</h3>
          <div id="giscus-comments"></div>
        </div>
      </div>

      <!-- 加载状态 -->
      <div v-else class="loading">
        <el-icon class="is-loading"><Loading /></el-icon>
        加载中...
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
/**
 * 文章详情页组件
 * 展示文章内容和Giscus评论
 */
import { ref, onMounted, computed } from 'vue'
import { useRoute } from 'vue-router'
import { Loading } from '@element-plus/icons-vue'
import { getArticle, getSettings } from '@/api'
import type { Article, SiteSettings } from '@/types'

const route = useRoute()

// 文章数据
const article = ref<Article | null>(null)

// 网站设置
const settings = ref<SiteSettings>({})

// 文章内容（Markdown转HTML）
const articleContent = computed(() => {
  // 实际项目中应使用markdown-it等库
  return article.value?.content?.replace(/\n/g, '<br>') || ''
})

/**
 * 格式化日期
 */
const formatDate = (dateStr: string) => {
  const date = new Date(dateStr)
  return date.toLocaleDateString('zh-CN', { year: 'numeric', month: 'long', day: 'numeric' })
}

/**
 * 加载Giscus评论组件
 */
const loadGiscus = () => {
  if (!settings.value.giscusRepo) return

  const script = document.createElement('script')
  script.src = 'https://giscus.app/client.js'
  script.setAttribute('data-repo', settings.value.giscusRepo)
  script.setAttribute('data-repo-id', settings.value.giscusRepoId || '')
  script.setAttribute('data-category', settings.value.giscusCategory || '')
  script.setAttribute('data-category-id', settings.value.giscusCategoryId || '')
  script.setAttribute('data-mapping', 'pathname')
  script.setAttribute('data-theme', 'preferred_color_scheme')
  script.setAttribute('data-lang', 'zh-CN')
  script.crossOrigin = 'anonymous'
  script.async = true

  document.getElementById('giscus-comments')?.appendChild(script)
}

onMounted(async () => {
  try {
    const id = Number(route.params.id)
    const [articleRes, settingsRes] = await Promise.all([
      getArticle(id),
      getSettings()
    ])
    article.value = articleRes.data
    settings.value = settingsRes.data
    loadGiscus()
  } catch {
    // 静默处理
  }
})
</script>

<style scoped>
.article-header {
  margin-bottom: 32px;
  padding-bottom: 24px;
  border-bottom: 1px solid var(--border-color);
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

.article-title {
  font-size: 32px;
  margin-bottom: 12px;
  color: var(--text-primary);
}

.article-meta {
  display: flex;
  gap: 16px;
  font-size: 14px;
  color: var(--text-secondary);
}

.article-body {
  font-size: 16px;
  line-height: 1.8;
}

.comments-section {
  margin-top: 48px;
  padding-top: 24px;
  border-top: 1px solid var(--border-color);
}

.comments-section h3 {
  margin-bottom: 24px;
  color: var(--text-primary);
}

.loading {
  text-align: center;
  padding: 60px;
  color: var(--text-secondary);
}
</style>
