<template>
  <!-- 私密文章详情页 -->
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
 * 私密文章详情页组件
 */
import { ref, onMounted, computed } from 'vue'
import { useRoute } from 'vue-router'
import { Loading } from '@element-plus/icons-vue'
import { getArticle } from '@/api'
import type { Article } from '@/types'

const route = useRoute()

// 文章数据
const article = ref<Article | null>(null)

// 文章内容
const articleContent = computed(() => {
  return article.value?.content?.replace(/\n/g, '<br>') || ''
})

/**
 * 格式化日期
 */
const formatDate = (dateStr: string) => {
  const date = new Date(dateStr)
  return date.toLocaleDateString('zh-CN', { year: 'numeric', month: 'long', day: 'numeric' })
}

onMounted(async () => {
  try {
    const id = Number(route.params.id)
    const { data } = await getArticle(id)
    article.value = data
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

.loading {
  text-align: center;
  padding: 60px;
  color: var(--text-secondary);
}
</style>
