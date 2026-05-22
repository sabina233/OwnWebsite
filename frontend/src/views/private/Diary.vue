<template>
  <!-- 日记列表页面 - 按日期倒序排列 -->
  <div class="diary">
    <div class="container">
      <h1 class="page-title">我的日记</h1>

      <div class="diary-list">
        <div v-for="diary in diaries" :key="diary.id" class="diary-item card">
          <div class="diary-date">
            <span class="day">{{ getDay(diary.createdAt) }}</span>
            <span class="month">{{ getMonth(diary.createdAt) }}</span>
          </div>
          <div class="diary-info">
            <h3 class="diary-title">
              <router-link :to="`/private/diary/${diary.id}`">{{ diary.title }}</router-link>
            </h3>
            <div class="diary-meta">
              <span v-if="diary.mood" class="mood">{{ diary.mood }}</span>
              <span v-if="diary.weather" class="weather">{{ diary.weather }}</span>
            </div>
          </div>
        </div>
      </div>

      <!-- 分页 -->
      <div class="pagination" v-if="totalPages > 1">
        <el-pagination
          v-model:current-page="currentPage"
          :page-size="pageSize"
          :total="totalCount"
          layout="prev, pager, next"
          @current-change="fetchDiaries"
        />
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
/**
 * 日记列表页面
 * 按日期倒序展示日记，需要私密区域访问权限
 */
import { ref, onMounted } from 'vue'
import { getDiaries } from '@/api'
import type { DiaryListItem } from '@/types'

// 日记列表
const diaries = ref<DiaryListItem[]>([])

// 分页状态
const currentPage = ref(1)
const pageSize = 20
const totalCount = ref(0)
const totalPages = ref(0)

/**
 * 获取日期中的日
 */
const getDay = (dateStr: string) => {
  return new Date(dateStr).getDate()
}

/**
 * 获取日期中的月
 */
const getMonth = (dateStr: string) => {
  return new Date(dateStr).toLocaleDateString('zh-CN', { month: 'short' })
}

/**
 * 获取日记列表
 */
const fetchDiaries = async () => {
  try {
    const { data } = await getDiaries({ page: currentPage.value, pageSize })
    diaries.value = data.items
    totalCount.value = data.totalCount
    totalPages.value = data.totalPages
  } catch {
    // 静默处理
  }
}

onMounted(fetchDiaries)
</script>

<style scoped>
.page-title {
  font-size: 28px;
  margin-bottom: 32px;
  color: var(--text-primary);
}

.diary-list {
  display: flex;
  flex-direction: column;
  gap: 16px;
}

.diary-item {
  display: flex;
  gap: 24px;
  align-items: center;
  padding: 20px;
}

.diary-date {
  text-align: center;
  min-width: 60px;
}

.diary-date .day {
  display: block;
  font-size: 32px;
  font-weight: 600;
  color: var(--accent-primary);
}

.diary-date .month {
  display: block;
  font-size: 14px;
  color: var(--text-secondary);
}

.diary-info {
  flex: 1;
}

.diary-title {
  font-size: 18px;
  margin-bottom: 8px;
}

.diary-title a {
  color: var(--text-primary);
}

.diary-title a:hover {
  color: var(--accent-primary);
}

.diary-meta {
  display: flex;
  gap: 12px;
  font-size: 14px;
  color: var(--text-secondary);
}

.pagination {
  margin-top: 40px;
  display: flex;
  justify-content: center;
}
</style>
