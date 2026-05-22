<template>
  <!-- 私密空间入口 - 密码验证页面 -->
  <div class="private-gate">
    <div class="container">
      <div class="gate-card card">
        <el-icon class="lock-icon"><Lock /></el-icon>
        <h2>私密空间</h2>
        <p>请输入密码进入我的私密日记</p>

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
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
/**
 * 私密空间入口组件
 * 验证密码后跳转到日记页面
 */
import { ref } from 'vue'
import { useRouter } from 'vue-router'
import { Lock } from '@element-plus/icons-vue'
import { useAuthStore } from '@/stores/auth'

const router = useRouter()
const authStore = useAuthStore()

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
    router.push('/private/diary')
  } else {
    errorMsg.value = '密码错误，请重试'
  }

  verifying.value = false
}
</script>

<style scoped>
.private-gate {
  min-height: 60vh;
  display: flex;
  align-items: center;
}

.gate-card {
  max-width: 400px;
  margin: 0 auto;
  text-align: center;
  padding: 48px;
}

.lock-icon {
  font-size: 48px;
  color: var(--accent-primary);
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
</style>
