/**
 * 主题状态管理 - 控制深浅主题切换
 */
import { defineStore } from 'pinia'
import { ref, watch } from 'vue'

export const useThemeStore = defineStore('theme', () => {
  // 当前主题模式：light 或 dark
  const isDark = ref(false)

  /**
   * 初始化主题
   * 从localStorage读取用户偏好，或跟随系统主题
   */
  const initTheme = () => {
    const savedTheme = localStorage.getItem('theme')
    if (savedTheme) {
      isDark.value = savedTheme === 'dark'
    } else {
      // 跟随系统主题
      isDark.value = window.matchMedia('(prefers-color-scheme: dark)').matches
    }
    applyTheme()
  }

  /**
   * 切换主题
   */
  const toggleTheme = () => {
    isDark.value = !isDark.value
  }

  /**
   * 应用主题到DOM
   */
  const applyTheme = () => {
    document.documentElement.setAttribute('data-theme', isDark.value ? 'dark' : 'light')
    localStorage.setItem('theme', isDark.value ? 'dark' : 'light')
  }

  // 监听主题变化，自动应用
  watch(isDark, applyTheme)

  return {
    isDark,
    initTheme,
    toggleTheme
  }
})
