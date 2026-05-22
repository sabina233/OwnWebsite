<template>
  <!-- 管理后台布局 - 侧边栏 + 内容区 -->
  <div class="admin-layout">
    <!-- 侧边栏 -->
    <aside class="sidebar">
      <div class="sidebar-header">
        <h2>管理后台</h2>
      </div>

      <nav class="sidebar-nav">
        <router-link to="/admin" class="nav-item">
          <el-icon><DataBoard /></el-icon>
          <span>仪表盘</span>
        </router-link>
        <router-link to="/admin/articles" class="nav-item">
          <el-icon><Document /></el-icon>
          <span>文章管理</span>
        </router-link>
        <router-link to="/admin/diary" class="nav-item">
          <el-icon><Notebook /></el-icon>
          <span>日记管理</span>
        </router-link>
        <router-link to="/admin/projects" class="nav-item">
          <el-icon><Folder /></el-icon>
          <span>项目管理</span>
        </router-link>
        <router-link to="/admin/photos" class="nav-item">
          <el-icon><Picture /></el-icon>
          <span>照片管理</span>
        </router-link>
        <router-link to="/admin/settings" class="nav-item">
          <el-icon><Setting /></el-icon>
          <span>网站设置</span>
        </router-link>
      </nav>

      <div class="sidebar-footer">
        <button class="btn-back" @click="goHome">
          <el-icon><Back /></el-icon>
          返回前台
        </button>
        <button class="btn-logout" @click="handleLogout">
          <el-icon><SwitchButton /></el-icon>
          退出登录
        </button>
      </div>
    </aside>

    <!-- 内容区域 -->
    <main class="admin-content">
      <router-view />
    </main>
  </div>
</template>

<script setup lang="ts">
/**
 * 管理后台布局组件
 * 包含侧边导航和内容区域
 */
import { useRouter } from 'vue-router'
import { useAuthStore } from '@/stores/auth'
import { DataBoard, Document, Notebook, Folder, Picture, Setting, Back, SwitchButton } from '@element-plus/icons-vue'

const router = useRouter()
const authStore = useAuthStore()

/** 返回前台首页 */
const goHome = () => {
  router.push('/')
}

/** 退出登录 */
const handleLogout = () => {
  authStore.logout()
  router.push('/admin/login')
}
</script>

<style scoped>
.admin-layout {
  display: flex;
  min-height: 100vh;
}

/* 侧边栏样式 */
.sidebar {
  width: 240px;
  background: var(--bg-card);
  box-shadow: var(--shadow);
  display: flex;
  flex-direction: column;
}

.sidebar-header {
  padding: 20px;
  border-bottom: 1px solid var(--border-color);
}

.sidebar-header h2 {
  font-size: 18px;
  color: var(--accent-primary);
}

.sidebar-nav {
  flex: 1;
  padding: 16px 0;
}

.nav-item {
  display: flex;
  align-items: center;
  gap: 12px;
  padding: 12px 20px;
  color: var(--text-secondary);
  transition: all 0.2s ease;
}

.nav-item:hover,
.nav-item.router-link-active {
  background: var(--bg-secondary);
  color: var(--accent-primary);
}

.sidebar-footer {
  padding: 16px;
  border-top: 1px solid var(--border-color);
  display: flex;
  flex-direction: column;
  gap: 8px;
}

.btn-back,
.btn-logout {
  display: flex;
  align-items: center;
  gap: 8px;
  padding: 10px;
  border: none;
  border-radius: var(--radius);
  cursor: pointer;
  font-size: 14px;
  transition: background 0.2s ease;
}

.btn-back {
  background: var(--bg-secondary);
  color: var(--text-secondary);
}

.btn-back:hover {
  background: var(--accent-primary);
  color: white;
}

.btn-logout {
  background: transparent;
  color: var(--text-secondary);
}

.btn-logout:hover {
  background: #ff4d4f;
  color: white;
}

/* 内容区域 */
.admin-content {
  flex: 1;
  padding: 24px;
  background: var(--bg-primary);
  overflow-y: auto;
}
</style>
