<template>
  <!-- 管理员登录页面 -->
  <div class="login-page">
    <div class="login-card card">
      <h2>管理后台登录</h2>
      <el-form :model="form" :rules="rules" ref="formRef">
        <el-form-item prop="username">
          <el-input v-model="form.username" placeholder="用户名" prefix-icon="User" />
        </el-form-item>
        <el-form-item prop="password">
          <el-input v-model="form.password" type="password" placeholder="密码" prefix-icon="Lock" show-password @keyup.enter="handleLogin" />
        </el-form-item>
        <el-form-item>
          <el-button type="primary" @click="handleLogin" :loading="loading" style="width: 100%">
            登录
          </el-button>
        </el-form-item>
      </el-form>
    </div>
  </div>
</template>

<script setup lang="ts">
/**
 * 管理员登录页面
 * 验证用户名密码后跳转到管理后台
 */
import { ref } from 'vue'
import { useRouter } from 'vue-router'
import { ElMessage } from 'element-plus'
import { useAuthStore } from '@/stores/auth'

const router = useRouter()
const authStore = useAuthStore()

// 表单数据
const form = ref({
  username: '',
  password: ''
})

// 验证规则
const rules = {
  username: [{ required: true, message: '请输入用户名', trigger: 'blur' }],
  password: [{ required: true, message: '请输入密码', trigger: 'blur' }]
}

const loading = ref(false)
const formRef = ref()

/**
 * 处理登录
 */
const handleLogin = async () => {
  try {
    await formRef.value?.validate()
    loading.value = true

    const success = await authStore.login(form.value.username, form.value.password)

    if (success) {
      ElMessage.success('登录成功')
      router.push('/admin')
    } else {
      ElMessage.error('用户名或密码错误')
    }
  } catch {
    ElMessage.error('登录失败')
  } finally {
    loading.value = false
  }
}
</script>

<style scoped>
.login-page {
  min-height: 100vh;
  display: flex;
  align-items: center;
  justify-content: center;
  background: var(--bg-primary);
}

.login-card {
  width: 400px;
  padding: 40px;
}

h2 {
  text-align: center;
  margin-bottom: 32px;
  color: var(--text-primary);
}
</style>
