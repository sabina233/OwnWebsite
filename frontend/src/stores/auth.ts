/**
 * 认证状态管理 - 管理管理员登录和私密区域访问
 */
import { defineStore } from 'pinia'
import { ref, computed } from 'vue'
import { login as loginApi, verifyPassword as verifyPasswordApi } from '@/api'

export const useAuthStore = defineStore('auth', () => {
  // 管理员Token
  const adminToken = ref<string | null>(localStorage.getItem('admin_token'))

  // 私密区域访问Token
  const privateToken = ref<string | null>(localStorage.getItem('private_token'))

  // 是否为管理员
  const isAdmin = computed(() => !!adminToken.value)

  // 是否有私密区域访问权限
  const hasPrivateAccess = computed(() => !!privateToken.value)

  /**
   * 管理员登录
   * @param username 用户名
   * @param password 密码
   * @returns 是否登录成功
   */
  const login = async (username: string, password: string): Promise<boolean> => {
    try {
      const { data } = await loginApi({ username, password })
      adminToken.value = data.token
      localStorage.setItem('admin_token', data.token)
      return true
    } catch {
      return false
    }
  }

  /**
   * 管理员登出
   */
  const logout = () => {
    adminToken.value = null
    localStorage.removeItem('admin_token')
  }

  /**
   * 验证私密区域密码
   * @param password 访问密码
   * @returns 是否验证成功
   */
  const verifyPrivatePassword = async (password: string): Promise<boolean> => {
    try {
      const { data } = await verifyPasswordApi(password)
      if (data.success && data.token) {
        privateToken.value = data.token
        localStorage.setItem('private_token', data.token)
        return true
      }
      return false
    } catch {
      return false
    }
  }

  /**
   * 清除私密区域访问权限
   */
  const clearPrivateAccess = () => {
    privateToken.value = null
    localStorage.removeItem('private_token')
  }

  return {
    adminToken,
    privateToken,
    isAdmin,
    hasPrivateAccess,
    login,
    logout,
    verifyPrivatePassword,
    clearPrivateAccess
  }
})
