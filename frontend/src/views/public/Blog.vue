<template>
  <!-- 博客页面 - 文章列表，支持分类筛选和搜索 -->
  <div class="blog">
    <div class="container">
      <h1 class="page-title">博客文章</h1>

      <!-- 搜索和筛选区域 -->
      <div class="filter-bar">
        <div class="search-box">
          <el-input
            v-model="searchQuery"
            placeholder="搜索文章..."
            clearable
            @keyup.enter="handleSearch"
          >
            <template #prefix>
              <el-icon><Search /></el-icon>
            </template>
          </el-input>
        </div>

        <!-- 文章分类 -->
        <div class="category-tags">
          <button
            class="tag"
            :class="{ active: !selectedCategory }"
            @click="selectedCategory = ''"
          >
            全部
          </button>
          <button
            v-for="cat in categories"
            :key="cat.id"
            class="tag"
            :class="{ active: selectedCategory === cat.name }"
            @click="selectedCategory = cat.name"
          >
            {{ cat.name }}
          </button>
        </div>
      </div>

      <!-- 文章列表 -->
      <div class="article-list">
        <div v-for="article in articles" :key="article.id" class="article-item card">
          <div class="article-cover" v-if="article.coverImage">
            <img :src="article.coverImage" :alt="article.title" loading="lazy" />
          </div>
          <div class="article-info">
            <span class="category" v-if="article.category">{{ article.category }}</span>
            <h2 class="article-title">
              <router-link :to="`/article/${article.id}`">{{ article.title }}</router-link>
            </h2>
            <p class="article-summary">{{ article.summary }}</p>
            <div class="article-meta">
              <span>{{ formatDate(article.createdAt) }}</span>
              <span>{{ article.viewCount }} 次阅读</span>
            </div>
          </div>
        </div>
      </div>

      <!-- 空状态 -->
      <div v-if="articles.length === 0 && !loading" class="empty-state">
        暂无文章
      </div>

      <!-- 分页 -->
      <div class="pagination" v-if="totalPages > 1">
        <el-pagination
          v-model:current-page="currentPage"
          :page-size="pageSize"
          :total="totalCount"
          layout="prev, pager, next"
          @current-change="fetchArticles"
        />
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
/**
 * 博客页面组件
 * 展示文章列表，支持分类筛选和关键词搜索
 */
import { ref, onMounted, watch } from 'vue'
import { Search } from '@element-plus/icons-vue'
import { getArticles, getCategories } from '@/api'
import type { ArticleListItem, Category } from '@/types'

// 文章列表
const articles = ref<ArticleListItem[]>([])

// 分类列表
const categories = ref<Category[]>([])

// 搜索和筛选状态
const searchQuery = ref('')
const selectedCategory = ref('')

// 分页状态
const currentPage = ref(1)
const pageSize = 10
const totalCount = ref(0)
const totalPages = ref(0)

// 加载状态
const loading = ref(false)

/**
 * 格式化日期
 */
const formatDate = (dateStr: string) => {
  const date = new Date(dateStr)
  return date.toLocaleDateString('zh-CN', { year: 'numeric', month: 'long', day: 'numeric' })
}

/**
 * 获取文章列表
 */
const fetchArticles = async () => {
  loading.value = true
  try {
    const { data } = await getArticles({
      page: currentPage.value,
      pageSize,
      category: selectedCategory.value || undefined,
      search: searchQuery.value || undefined
    })
    articles.value = data.items
    totalCount.value = data.totalCount
    totalPages.value = data.totalPages
  } catch {
    // 静默处理
  } finally {
    loading.value = false
  }
}

/**
 * 获取分类列表
 */
const fetchCategories = async () => {
  try {
    const { data } = await getCategories()
    categories.value = data
  } catch {
    // 静默处理
  }
}

/**
 * 处理搜索
 */
const handleSearch = () => {
  currentPage.value = 1
  fetchArticles()
}

// 监听分类变化
watch(selectedCategory, () => {
  currentPage.value = 1
  fetchArticles()
})

// 初始化
onMounted(() => {
  fetchCategories()
  fetchArticles()
})
</script>

<style scoped>
.page-title {
  font-size: 28px;
  margin-bottom: 32px;
  color: var(--text-primary);
}

/* 筛选栏 */
.filter-bar {
  margin-bottom: 32px;
}

.search-box {
  margin-bottom: 16px;
}

.category-tags {
  display: flex;
  flex-wrap: wrap;
  gap: 8px;
}

.tag {
  padding: 8px 20px;
  background: var(--bg-card);
  border: 1px solid var(--border-color);
  border-radius: 24px;
  font-size: 14px;
  cursor: pointer;
  transition: all 0.2s ease;
  color: var(--text-secondary);
}

.tag:hover,
.tag.active {
  background: var(--accent-primary);
  color: white;
  border-color: var(--accent-primary);
}

/* 文章列表 */
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
  border-radius: 12px 0 0 12px;
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
  background: var(--accent-light);
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
  text-decoration: none;
}

.article-title a:hover {
  color: var(--accent-primary);
}

.article-summary {
  color: var(--text-secondary);
  font-size: 14px;
  line-height: 1.6;
  margin-bottom: 12px;
  display: -webkit-box;
  -webkit-line-clamp: 2;
  -webkit-box-orient: vertical;
  overflow: hidden;
}

.article-meta {
  display: flex;
  gap: 16px;
  font-size: 12px;
  color: var(--text-muted);
}

/* 空状态 */
.empty-state {
  text-align: center;
  padding: 60px;
  color: var(--text-secondary);
}

/* 分页 */
.pagination {
  margin-top: 40px;
  display: flex;
  justify-content: center;
}

/* 响应式 */
@media (max-width: 768px) {
  .article-item {
    flex-direction: column;
  }

  .article-cover {
    width: 100%;
    min-height: 200px;
    border-radius: 12px 12px 0 0;
  }
}
</style>
