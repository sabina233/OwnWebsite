<template>
  <!-- 主布局 - 公开页面和私密页面共用 -->
  <div class="main-layout">
    <!-- 顶部导航栏 -->
    <header class="header">
      <div class="container header-content">
        <!-- 网站Logo -->
        <router-link to="/" class="logo">
          <span class="logo-icon">✦</span>
          <span class="logo-text">{{ settings.siteName || '我的网站' }}</span>
        </router-link>

        <!-- 桌面端导航菜单 -->
        <nav class="nav-menu desktop-nav">
          <router-link to="/" class="nav-link">首页</router-link>
          <router-link to="/about" class="nav-link">关于我</router-link>
          <router-link to="/blog" class="nav-link">博客</router-link>
          <router-link to="/projects" class="nav-link">项目</router-link>
          <router-link to="/gallery" class="nav-link">摄影</router-link>
          <router-link to="/contact" class="nav-link">联系</router-link>
          <router-link to="/private" class="nav-link nav-link--private">
            <el-icon><Lock /></el-icon>
            <span>秘密</span>
          </router-link>
        </nav>

        <!-- 右侧操作区 -->
        <div class="header-actions">
          <!-- 主题切换按钮 -->
          <button class="theme-toggle" @click="themeStore.toggleTheme()" :title="themeStore.isDark ? '切换到浅色' : '切换到深色'">
            <el-icon v-if="themeStore.isDark"><Sunny /></el-icon>
            <el-icon v-else><Moon /></el-icon>
          </button>

          <!-- 移动端菜单按钮 -->
          <button class="mobile-menu-btn" @click="toggleMobileMenu">
            <el-icon v-if="!isMobileMenuOpen"><Menu /></el-icon>
            <el-icon v-else><Close /></el-icon>
          </button>
        </div>
      </div>

      <!-- 移动端导航菜单 -->
      <Transition name="slide-down">
        <nav v-if="isMobileMenuOpen" class="mobile-nav">
          <router-link to="/" class="mobile-nav-link" @click="closeMobileMenu">首页</router-link>
          <router-link to="/about" class="mobile-nav-link" @click="closeMobileMenu">关于我</router-link>
          <router-link to="/blog" class="mobile-nav-link" @click="closeMobileMenu">博客</router-link>
          <router-link to="/projects" class="mobile-nav-link" @click="closeMobileMenu">项目</router-link>
          <router-link to="/gallery" class="mobile-nav-link" @click="closeMobileMenu">摄影</router-link>
          <router-link to="/contact" class="mobile-nav-link" @click="closeMobileMenu">联系</router-link>
          <router-link to="/private" class="mobile-nav-link mobile-nav-link--private" @click="closeMobileMenu">
            <el-icon><Lock /></el-icon>
            秘密
          </router-link>
        </nav>
      </Transition>
    </header>

    <!-- 页面内容区域 -->
    <main class="main-content">
      <router-view v-slot="{ Component }">
        <Transition name="fade" mode="out-in">
          <component :is="Component" />
        </Transition>
      </router-view>
    </main>

    <!-- 底部信息 -->
    <footer class="footer">
      <div class="container footer-content">
        <p class="footer-text">
          &copy; {{ new Date().getFullYear() }} {{ settings.siteName || '我的网站' }}
        </p>
        <p class="footer-sub">用热爱搭建的小角落</p>
      </div>
    </footer>
  </div>
</template>

<script setup lang="ts">
/**
 * 主布局组件
 * 包含导航栏、内容区域和底部信息
 * 支持响应式布局和移动端菜单
 */
import { ref, onMounted, watch } from 'vue'
import { useRoute } from 'vue-router'
import { Lock, Sunny, Moon, Menu, Close } from '@element-plus/icons-vue'
import { useThemeStore } from '@/stores/theme'
import { getSettings } from '@/api'
import type { SiteSettings } from '@/types'

const route = useRoute()
const themeStore = useThemeStore()

// 网站设置
const settings = ref<SiteSettings>({})

// 移动端菜单状态
const isMobileMenuOpen = ref(false)

/**
 * 切换移动端菜单
 */
const toggleMobileMenu = () => {
  isMobileMenuOpen.value = !isMobileMenuOpen.value
}

/**
 * 关闭移动端菜单
 */
const closeMobileMenu = () => {
  isMobileMenuOpen.value = false
}

// 路由变化时关闭菜单
watch(() => route.path, closeMobileMenu)

// 初始化
onMounted(async () => {
  themeStore.initTheme()
  try {
    const { data } = await getSettings()
    settings.value = data
  } catch {
    // 静默处理
  }
})
</script>

<style scoped>
.main-layout {
  min-height: 100vh;
  display: flex;
  flex-direction: column;
}

/* ========== 头部样式 ========== */
.header {
  background: var(--bg-card);
  box-shadow: var(--shadow-sm);
  position: sticky;
  top: 0;
  z-index: 100;
  border-bottom: 1px solid var(--border-light);
}

.header-content {
  display: flex;
  align-items: center;
  justify-content: space-between;
  height: 64px;
}

/* Logo 样式 */
.logo {
  display: flex;
  align-items: center;
  gap: 10px;
  text-decoration: none;
}

.logo-icon {
  font-size: 24px;
  color: var(--accent-primary);
}

.logo-text {
  font-size: 20px;
  font-weight: 600;
  color: var(--text-primary);
  transition: color var(--transition-fast);
}

.logo:hover .logo-text {
  color: var(--accent-primary);
}

/* 桌面端导航 */
.desktop-nav {
  display: flex;
  align-items: center;
  gap: 8px;
}

.nav-link {
  display: flex;
  align-items: center;
  gap: 6px;
  padding: 8px 16px;
  color: var(--text-secondary);
  font-size: 15px;
  font-weight: 500;
  border-radius: var(--radius-md);
  transition: all var(--transition-fast);
  text-decoration: none;
}

.nav-link:hover {
  color: var(--accent-primary);
  background: var(--accent-light);
}

.nav-link.router-link-active {
  color: var(--accent-primary);
  background: var(--accent-light);
}

.nav-link--private {
  color: var(--text-muted);
  border: 1px solid var(--border-color);
  margin-left: 8px;
}

.nav-link--private:hover {
  color: var(--accent-primary);
  border-color: var(--accent-primary);
}

/* 右侧操作区 */
.header-actions {
  display: flex;
  align-items: center;
  gap: 12px;
}

/* 主题切换按钮 */
.theme-toggle {
  width: 40px;
  height: 40px;
  display: flex;
  align-items: center;
  justify-content: center;
  background: var(--bg-secondary);
  border: none;
  border-radius: var(--radius-md);
  cursor: pointer;
  color: var(--text-secondary);
  font-size: 18px;
  transition: all var(--transition-fast);
}

.theme-toggle:hover {
  background: var(--accent-light);
  color: var(--accent-primary);
}

/* 移动端菜单按钮 */
.mobile-menu-btn {
  display: none;
  width: 40px;
  height: 40px;
  align-items: center;
  justify-content: center;
  background: var(--bg-secondary);
  border: none;
  border-radius: var(--radius-md);
  cursor: pointer;
  color: var(--text-secondary);
  font-size: 20px;
  transition: all var(--transition-fast);
}

/* 移动端导航 */
.mobile-nav {
  display: none;
  flex-direction: column;
  padding: 16px;
  background: var(--bg-card);
  border-top: 1px solid var(--border-light);
}

.mobile-nav-link {
  display: flex;
  align-items: center;
  gap: 12px;
  padding: 14px 16px;
  color: var(--text-secondary);
  font-size: 16px;
  font-weight: 500;
  border-radius: var(--radius-md);
  transition: all var(--transition-fast);
  text-decoration: none;
}

.mobile-nav-link:hover {
  color: var(--accent-primary);
  background: var(--accent-light);
}

.mobile-nav-link.router-link-active {
  color: var(--accent-primary);
  background: var(--accent-light);
}

.mobile-nav-link--private {
  margin-top: 8px;
  border-top: 1px solid var(--border-light);
  padding-top: 16px;
}

/* ========== 内容区域 ========== */
.main-content {
  flex: 1;
  padding: 40px 0;
  min-height: calc(100vh - 64px - 120px);
}

/* ========== 底部样式 ========== */
.footer {
  background: var(--bg-card);
  padding: 32px 0;
  border-top: 1px solid var(--border-light);
}

.footer-content {
  text-align: center;
}

.footer-text {
  color: var(--text-secondary);
  font-size: 14px;
  margin-bottom: 4px;
}

.footer-sub {
  color: var(--text-muted);
  font-size: 13px;
}

/* ========== 动画 ========== */
.slide-down-enter-active,
.slide-down-leave-active {
  transition: all var(--transition-normal);
}

.slide-down-enter-from,
.slide-down-leave-to {
  opacity: 0;
  transform: translateY(-10px);
}

/* ========== 响应式布局 ========== */
@media (max-width: 768px) {
  .desktop-nav {
    display: none;
  }

  .mobile-menu-btn {
    display: flex;
  }

  .mobile-nav {
    display: flex;
  }

  .main-content {
    padding: 24px 0;
  }
}

@media (max-width: 480px) {
  .header-content {
    height: 56px;
  }

  .logo-text {
    font-size: 18px;
  }
}
</style>
