<template>
  <!-- 管理后台仪表盘 -->
  <div class="dashboard">
    <h1 class="page-title">仪表盘</h1>

    <div class="stats-grid">
      <div class="stat-card card">
        <div class="stat-icon articles">
          <el-icon><Document /></el-icon>
        </div>
        <div class="stat-info">
          <span class="stat-value">{{ stats.articles }}</span>
          <span class="stat-label">文章数量</span>
        </div>
      </div>

      <div class="stat-card card">
        <div class="stat-icon projects">
          <el-icon><Folder /></el-icon>
        </div>
        <div class="stat-info">
          <span class="stat-value">{{ stats.projects }}</span>
          <span class="stat-label">项目数量</span>
        </div>
      </div>

      <div class="stat-card card">
        <div class="stat-icon photos">
          <el-icon><Picture /></el-icon>
        </div>
        <div class="stat-info">
          <span class="stat-value">{{ stats.photos }}</span>
          <span class="stat-label">照片数量</span>
        </div>
      </div>

      <div class="stat-card card">
        <div class="stat-icon messages">
          <el-icon><Message /></el-icon>
        </div>
        <div class="stat-info">
          <span class="stat-value">{{ stats.messages }}</span>
          <span class="stat-label">未读消息</span>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
/**
 * 管理后台仪表盘
 * 显示网站统计数据概览
 */
import { ref, onMounted } from 'vue'
import { Document, Folder, Picture, Message } from '@element-plus/icons-vue'
import { getArticles, getProjects, getPhotos, getContactMessages } from '@/api'

// 统计数据
const stats = ref({
  articles: 0,
  projects: 0,
  photos: 0,
  messages: 0
})

onMounted(async () => {
  try {
    const [articlesRes, projectsRes, photosRes, messagesRes] = await Promise.all([
      getArticles({ pageSize: 1 }),
      getProjects(),
      getPhotos({ pageSize: 1 }),
      getContactMessages()
    ])

    stats.value = {
      articles: articlesRes.data.totalCount,
      projects: projectsRes.data.length,
      photos: photosRes.data.totalCount,
      messages: messagesRes.data.filter(m => !m.isRead).length
    }
  } catch {
    // 静默处理
  }
})
</script>

<style scoped>
.page-title {
  font-size: 24px;
  margin-bottom: 24px;
  color: var(--text-primary);
}

.stats-grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(240px, 1fr));
  gap: 24px;
}

.stat-card {
  display: flex;
  align-items: center;
  gap: 16px;
  padding: 24px;
}

.stat-icon {
  width: 48px;
  height: 48px;
  border-radius: 12px;
  display: flex;
  align-items: center;
  justify-content: center;
  font-size: 24px;
}

.stat-icon.articles { background: #e6f7ff; color: #1890ff; }
.stat-icon.projects { background: #f6ffed; color: #52c41a; }
.stat-icon.photos { background: #fff7e6; color: #fa8c16; }
.stat-icon.messages { background: #fff1f0; color: #ff4d4f; }

.stat-value {
  display: block;
  font-size: 28px;
  font-weight: 600;
  color: var(--text-primary);
}

.stat-label {
  display: block;
  font-size: 14px;
  color: var(--text-secondary);
}
</style>
