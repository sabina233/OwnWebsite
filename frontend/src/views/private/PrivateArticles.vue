<template>
  <!-- 私密文章列表页面 -->
  <div class="private-articles">
    <div class="container">
      <h1 class="page-title">私密文章</h1>

      <!-- 分类筛选 -->
      <div class="category-filter" v-if="categories.length > 0">
        <span
          class="tag"
          :class="{ active: !selectedCategory }"
          @click="selectedCategory = ''"
        >
          全部
        </span>
        <span
          v-for="cat in categories"
          :key="cat"
          class="tag"
          :class="{ active: selectedCategory === cat }"
          @click="selectedCategory = cat"
        >
          {{ cat }}
        </span>
      </div>

      <!-- 文章列表 -->
      <div class="article-list">
        <div v-for="article in articles" :key="article.id" class="article-item card">
          <div class="article-cover" v-if="article.coverImage">
            <img :src="article.coverImage" :alt="article.title" />
          </div>
          <div class="article-info">
            <span class="category" v-if="article.category">{{ article.category }}</span>
            <h2 class="article-title">
              <router-link :to="`/private/articles/${article.id}`">{{ article.title }}</router-link>
            </h2>
            <p class="article-summary">{{ article.summary }}</p>
            <div class="article-meta">
              <span>{{ formatDate(article.createdAt) }}</span>
              <span>{{ article.viewCount }} 次阅读</span>
            </div>
          </div>
        </div>
      </div>

      <div v-if="articles.length === 0" class="empty-tip">
        暂无私密文章
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
/**
 * 私密文章列表页面
 * 展示标记为私密的文章
 */
import { ref, onMounted, watch } from 'vue'
import { getArticles } from '@/api'
import type { ArticleListItem } from '@/types'

// 文章列表
const articles = ref<ArticleListItem[]>([])

// 分类筛选
const selectedCategory = ref('')
const categories = ref<string[]>([])

/**
 * 格式化日期
 */
const formatDate = (dateStr: string) => {
  const date = new Date(dateStr)
  return date.toLocaleDateString('zh-CN', { year: 'numeric', month: 'long', day: 'numeric' })
}

/**
 * 获取私密文章列表
 */
const fetchArticles = async () => {
  try {
    const { data } = await getArticles({
      pageSize: 50,
      category: selectedCategory.value || undefined
    })
    // 过滤出私密文章
    articles.value = data.items

    // 提取分类
    const cats = new Set(data.items.map(a => a.category).filter(Boolean))
    categories.value = Array.from(cats) as string[]
  } catch {
    // 静默处理
  }
}

watch(selectedCategory, fetchArticles)

onMounted(fetchArticles)
</script>

<style scoped>
.page-title {
  font-size: 28px;
  margin-bottom: 32px;
  color: var(--text-primary);
}

.category-filter {
  display: flex;
  flex-wrap: wrap;
  gap: 8px;
  margin-bottom: 24px;
}

.tag {
  padding: 6px 16px;
  background: var(--bg-card);
  border: 1px solid var(--border-color);
  border-radius: 20px;
  font-size: 14px;
  cursor: pointer;
  transition: all 0.2s ease;
}

.tag:hover,
.tag.active {
  background: var(--accent-primary);
  color: white;
  border-color: var(--accent-primary);
}

.article-list {
  display: flex;
  flex-direction: column;
  gap: 24px;
}

.article-item {
  display: flex;
  gap: 24px;
  overflow: hidden;
}

.article-cover {
  width: 240px;
  min-height: 160px;
  overflow: hidden;
}

.article-cover img {
  width: 100%;
  height: 100%;
  object-fit: cover;
}

.article-info {
  flex: 1;
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

.article-title {
  font-size: 20px;
  margin-bottom: 8px;
}

.article-title a {
  color: var(--text-primary);
}

.article-title a:hover {
  color: var(--accent-primary);
}

.article-summary {
  color: var(--text-secondary);
  font-size: 14px;
  line-height: 1.6;
  margin-bottom: 12px;
}

.article-meta {
  display: flex;
  gap: 16px;
  font-size: 12px;
  color: var(--text-secondary);
}

.empty-tip {
  text-align: center;
  padding: 60px;
  color: var(--text-secondary);
}
</style>
