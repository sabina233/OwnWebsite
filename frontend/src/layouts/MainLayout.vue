<template>
  <!-- 主布局 - 公开页面和私密页面共用 -->
  <div class="main-layout">
    <!-- 顶部导航栏 -->
    <header class="header">
      <div class="container header-content">
        <!-- 网站Logo -->
        <router-link to="/" class="logo">
          {{ settings.siteName || '我的网站' }}
        </router-link>

        <!-- 导航菜单 -->
        <nav class="nav-menu">
          <router-link to="/" class="nav-link">首页</router-link>
          <router-link to="/about" class="nav-link">关于我</router-link>
          <router-link to="/blog" class="nav-link">博客</router-link>
          <router-link to="/projects" class="nav-link">项目</router-link>
          <router-link to="/gallery" class="nav-link">摄影</router-link>
          <router-link to="/contact" class="nav-link">联系</router-link>
          <router-link to="/private" class="nav-link private-link">
            <el-icon><Lock /></el-icon>
            日记
          </router-link>
        </nav>

        <!-- 主题切换按钮 -->
        <button class="theme-toggle" @click="themeStore.toggleTheme()">
          <el-icon v-if="themeStore.isDark"><Sunny /></el-icon>
          <el-icon v-else><Moon /></el-icon>
        </button>
      </div>
    </header>

    <!-- 页面内容区域 -->
    <main class="main-content">
      <router-view />
    </main>

    <!-- 底部信息 -->
    <footer class="footer">
      <div class="container">
        <p>&copy; {{ new Date().getFullYear() }} {{ settings.siteName || '我的网站' }} · 温馨的小角落</p>
      </div>
    </footer>
  </div>
</template>

<script setup lang="ts">
/**
 * 主布局组件
 * 包含导航栏、内容区域和底部信息
 */
import { ref, onMounted } from 'vue'
import { Lock, Sunny, Moon } from '@element-plus/icons-vue'
import { useThemeStore } from '@/stores/theme'
import { getSettings } from '@/api'
import type { SiteSettings } from '@/types'

// 主题状态
const themeStore = useThemeStore()

// 网站设置
const settings = ref<SiteSettings>({})

// 初始化：获取网站设置
onMounted(async () => {
  themeStore.initTheme()
  try {
    const { data } = await getSettings()
    settings.value = data
  } catch {
    // 静默处理，使用默认值
  }
})
</script>

<style scoped>
.main-layout {
  min-height: 100vh;
  display: flex;
  flex-direction: column;
}

/* 头部样式 */
.header {
  background: var(--bg-card);
  box-shadow: var(--shadow);
  position: sticky;
  top: 0;
  z-index: 100;
}

.header-content {
  display: flex;
  align-items: center;
  justify-content: space-between;
  height: 64px;
}

.logo {
  font-size: 20px;
  font-weight: 600;
  color: var(--accent-primary);
}

.nav-menu {
  display: flex;
  gap: 24px;
}

.nav-link {
  color: var(--text-secondary);
  font-size: 15px;
  display: flex;
  align-items: center;
  gap: 4px;
  padding: 8px 0;
  border-bottom: 2px solid transparent;
  transition: all 0.2s ease;
}

.nav-link:hover,
.nav-link.router-link-active {
  color: var(--accent-primary);
  border-bottom-color: var(--accent-primary);
}

.private-link {
  color: var(--text-secondary);
}

.theme-toggle {
  background: none;
  border: none;
  cursor: pointer;
  font-size: 20px;
  color: var(--text-secondary);
  padding: 8px;
  border-radius: 50%;
  transition: background 0.2s ease;
}

.theme-toggle:hover {
  background: var(--bg-secondary);
}

/* 内容区域 */
.main-content {
  flex: 1;
  padding: 40px 0;
}

/* 底部样式 */
.footer {
  background: var(--bg-card);
  padding: 20px 0;
  text-align: center;
  color: var(--text-secondary);
  font-size: 14px;
}
</style>
