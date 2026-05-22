<template>
  <!-- 日记详情页 -->
  <div class="diary-detail">
    <div class="container">
      <div class="diary-card card" v-if="diary">
        <header class="diary-header">
          <h1 class="diary-title">{{ diary.title }}</h1>
          <div class="diary-meta">
            <span>{{ formatDate(diary.createdAt) }}</span>
            <span v-if="diary.mood">心情: {{ diary.mood }}</span>
            <span v-if="diary.weather">天气: {{ diary.weather }}</span>
          </div>
        </header>
        <div class="diary-body markdown-body" v-html="diaryContent"></div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
/**
 * 日记详情页组件
 * 展示日记完整内容（Markdown格式）
 */
import { ref, onMounted, computed } from 'vue'
import { useRoute } from 'vue-router'
import { getDiary } from '@/api'
import type { Diary } from '@/types'

const route = useRoute()

// 日记数据
const diary = ref<Diary | null>(null)

// 日记内容（Markdown转HTML）
const diaryContent = computed(() => {
  return diary.value?.content?.replace(/\n/g, '<br>') || ''
})

/**
 * 格式化日期
 */
const formatDate = (dateStr: string) => {
  const date = new Date(dateStr)
  return date.toLocaleDateString('zh-CN', {
    year: 'numeric',
    month: 'long',
    day: 'numeric',
    weekday: 'long'
  })
}

onMounted(async () => {
  try {
    const id = Number(route.params.id)
    const { data } = await getDiary(id)
    diary.value = data
  } catch {
    // 静默处理
  }
})
</script>

<style scoped>
.diary-header {
  margin-bottom: 32px;
  padding-bottom: 24px;
  border-bottom: 1px solid var(--border-color);
}

.diary-title {
  font-size: 28px;
  margin-bottom: 12px;
  color: var(--text-primary);
}

.diary-meta {
  display: flex;
  gap: 16px;
  font-size: 14px;
  color: var(--text-secondary);
}

.diary-body {
  font-size: 16px;
  line-height: 1.8;
}
</style>
