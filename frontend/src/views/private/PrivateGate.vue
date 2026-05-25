<template>
  <!-- 秘密空间入口 - 密码验证页面 -->
  <div class="private-gate">
    <div class="container">
      <div class="gate-card card">
        <!-- 未验证状态 -->
        <template v-if="!isVerified">
          <el-icon class="lock-icon"><Lock /></el-icon>
          <h2>秘密空间</h2>
          <p>请输入密码进入我的私密世界</p>

          <div class="password-form">
            <el-input
              v-model="password"
              type="password"
              placeholder="请输入访问密码"
              show-password
              @keyup.enter="handleVerify"
            >
              <template #prefix>
                <el-icon><Lock /></el-icon>
              </template>
            </el-input>
            <el-button type="primary" @click="handleVerify" :loading="verifying">
              进入
            </el-button>
          </div>

          <p class="error-msg" v-if="errorMsg">{{ errorMsg }}</p>
        </template>

        <!-- 已验证状态 - 显示入口 -->
        <template v-else>
          <el-icon class="unlock-icon"><Unlock /></el-icon>
          <h2>欢迎来到秘密空间</h2>
          <p>选择要查看的内容</p>

          <div class="entry-grid">
            <router-link to="/private/articles" class="entry-item">
              <el-icon class="entry-icon"><Document /></el-icon>
              <span class="entry-title">私密文章</span>
              <span class="entry-desc">只属于我的文字</span>
            </router-link>

            <router-link to="/private/diary" class="entry-item">
              <el-icon class="entry-icon"><Notebook /></el-icon>
              <span class="entry-title">我的日记</span>
              <span class="entry-desc">记录每日心情</span>
            </router-link>
          </div>
        </template>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
/**
 * 秘密空间入口组件
 * 验证密码后显示文章和日记入口
 */
import { ref, onMounted } from 'vue'
import { Lock, Unlock, Document, Notebook } from '@element-plus/icons-vue'
import { useAuthStore } from '@/stores/auth'

const authStore = useAuthStore()

// 验证状态
const isVerified = ref(false)

// 密码输入
const password = ref('')
const verifying = ref(false)
const errorMsg = ref('')

/**
 * 验证密码
 */
const handleVerify = async () => {
  if (!password.value) {
    errorMsg.value = '请输入密码'
    return
  }

  verifying.value = true
  errorMsg.value = ''

  const success = await authStore.verifyPrivatePassword(password.value)

  if (success) {
    isVerified.value = true
  } else {
    errorMsg.value = '密码错误，请重试'
  }

  verifying.value = false
}

// 检查是否已验证
onMounted(() => {
  if (authStore.hasPrivateAccess) {
    isVerified.value = true
  }
})
</script>

<style scoped>
.private-gate {
  min-height: 60vh;
  display: flex;
  align-items: center;
}

.gate-card {
  max-width: 500px;
  margin: 0 auto;
  text-align: center;
  padding: 48px;
}

.lock-icon {
  font-size: 48px;
  color: var(--accent-primary);
  margin-bottom: 16px;
}

.unlock-icon {
  font-size: 48px;
  color: #52c41a;
  margin-bottom: 16px;
}

h2 {
  font-size: 24px;
  margin-bottom: 8px;
  color: var(--text-primary);
}

p {
  color: var(--text-secondary);
  margin-bottom: 24px;
}

.password-form {
  display: flex;
  flex-direction: column;
  gap: 16px;
}

.error-msg {
  color: #ff4d4f;
  font-size: 14px;
  margin-top: 8px;
}

/* 入口网格 */
.entry-grid {
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 16px;
  margin-top: 8px;
}

.entry-item {
  display: flex;
  flex-direction: column;
  align-items: center;
  gap: 12px;
  padding: 24px;
  background: var(--bg-secondary);
  border-radius: var(--radius);
  transition: all 0.2s ease;
  cursor: pointer;
  text-decoration: none;
}

.entry-item:hover {
  transform: translateY(-2px);
  box-shadow: var(--shadow);
}

.entry-icon {
  font-size: 36px;
  color: var(--accent-primary);
}

.entry-title {
  font-size: 18px;
  font-weight: 500;
  color: var(--text-primary);
}

.entry-desc {
  font-size: 14px;
  color: var(--text-secondary);
}
</style>
