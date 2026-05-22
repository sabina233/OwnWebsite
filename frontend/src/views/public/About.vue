<template>
  <!-- 关于我页面 -->
  <div class="about">
    <div class="container">
      <h1 class="page-title">关于我</h1>
      <div class="about-content card">
        <div class="markdown-body" v-html="aboutContent"></div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
/**
 * 关于我页面
 * 展示个人介绍（Markdown格式）
 */
import { ref, onMounted, computed } from 'vue'
import { getSettings } from '@/api'

// 网站设置
const settings = ref<any>({})

// 将Markdown转换为HTML（简单处理）
const aboutContent = computed(() => {
  // 实际项目中应使用markdown-it等库
  return settings.value.aboutContent?.replace(/\n/g, '<br>') || '暂无介绍'
})

onMounted(async () => {
  try {
    const { data } = await getSettings()
    settings.value = data
  } catch {
    // 静默处理
  }
})
</script>

<style scoped>
.page-title {
  font-size: 28px;
  margin-bottom: 32px;
  color: var(--text-primary);
}

.about-content {
  padding: 40px;
  line-height: 1.8;
  font-size: 16px;
}
</style>
