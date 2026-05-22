<template>
  <!-- 联系方式页面 -->
  <div class="contact">
    <div class="container">
      <h1 class="page-title">联系方式</h1>

      <div class="contact-grid">
        <!-- 联系信息 -->
        <div class="contact-info card">
          <h3>联系我</h3>
          <div class="info-list">
            <div class="info-item" v-if="settings.email">
              <el-icon><Message /></el-icon>
              <span>{{ settings.email }}</span>
            </div>
            <div class="info-item" v-if="settings.gitHub">
              <el-icon><Link /></el-icon>
              <a :href="settings.gitHub" target="_blank">GitHub</a>
            </div>
            <div class="info-item" v-if="settings.twitter">
              <el-icon><Link /></el-icon>
              <a :href="settings.twitter" target="_blank">Twitter</a>
            </div>
            <div class="info-item" v-if="settings.weChat">
              <el-icon><ChatDotRound /></el-icon>
              <span>微信: {{ settings.weChat }}</span>
            </div>
            <div class="info-item" v-if="settings.qq">
              <el-icon><ChatDotRound /></el-icon>
              <span>QQ: {{ settings.qq }}</span>
            </div>
          </div>
        </div>

        <!-- 联系表单 -->
        <div class="contact-form card">
          <h3>发送消息</h3>
          <el-form :model="form" :rules="rules" ref="formRef" label-position="top">
            <el-form-item label="姓名" prop="name">
              <el-input v-model="form.name" placeholder="请输入您的姓名" />
            </el-form-item>
            <el-form-item label="邮箱" prop="email">
              <el-input v-model="form.email" placeholder="请输入您的邮箱" />
            </el-form-item>
            <el-form-item label="主题">
              <el-input v-model="form.subject" placeholder="消息主题（可选）" />
            </el-form-item>
            <el-form-item label="消息" prop="message">
              <el-input v-model="form.message" type="textarea" :rows="4" placeholder="请输入您想说的话" />
            </el-form-item>
            <el-form-item>
              <el-button type="primary" @click="handleSubmit" :loading="submitting">
                发送消息
              </el-button>
            </el-form-item>
          </el-form>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
/**
 * 联系方式页面
 * 展示联系信息和联系表单
 */
import { ref, onMounted } from 'vue'
import { Message, Link, ChatDotRound } from '@element-plus/icons-vue'
import { ElMessage } from 'element-plus'
import { getSettings, submitContact } from '@/api'
import type { SiteSettings } from '@/types'

// 网站设置
const settings = ref<SiteSettings>({})

// 表单数据
const form = ref({
  name: '',
  email: '',
  subject: '',
  message: ''
})

// 表单验证规则
const rules = {
  name: [{ required: true, message: '请输入姓名', trigger: 'blur' }],
  email: [
    { required: true, message: '请输入邮箱', trigger: 'blur' },
    { type: 'email', message: '请输入正确的邮箱格式', trigger: 'blur' }
  ],
  message: [{ required: true, message: '请输入消息内容', trigger: 'blur' }]
}

// 提交状态
const submitting = ref(false)
const formRef = ref()

/**
 * 提交联系表单
 */
const handleSubmit = async () => {
  try {
    await formRef.value?.validate()
    submitting.value = true
    await submitContact(form.value)
    ElMessage.success('消息已发送，感谢您的联系！')
    form.value = { name: '', email: '', subject: '', message: '' }
  } catch {
    ElMessage.error('发送失败，请稍后重试')
  } finally {
    submitting.value = false
  }
}

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

.contact-grid {
  display: grid;
  grid-template-columns: 1fr 2fr;
  gap: 24px;
}

.contact-info h3,
.contact-form h3 {
  font-size: 20px;
  margin-bottom: 24px;
  color: var(--text-primary);
}

.info-list {
  display: flex;
  flex-direction: column;
  gap: 16px;
}

.info-item {
  display: flex;
  align-items: center;
  gap: 12px;
  font-size: 16px;
  color: var(--text-secondary);
}

.info-item a {
  color: var(--accent-primary);
}
</style>
