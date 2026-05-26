<template>
  <!-- 网站设置页面 -->
  <div class="settings">
    <h1 class="page-title">网站设置</h1>

    <el-form :model="form" label-position="top" class="settings-form">
      <!-- 基本信息 -->
      <div class="form-section card">
        <h3>基本信息</h3>

        <!-- 头像上传 -->
        <el-form-item label="头像">
          <div class="avatar-upload">
            <div class="avatar-preview">
              <img :src="form.avatar || '/default-avatar.png'" alt="头像预览" />
            </div>
            <div class="avatar-actions">
              <el-upload
                action="/api/upload/avatar"
                :headers="uploadHeaders"
                :show-file-list="false"
                :on-success="handleAvatarSuccess"
                :before-upload="beforeAvatarUpload"
              >
                <el-button type="primary" size="small">上传头像</el-button>
              </el-upload>
              <el-input v-model="form.avatar" placeholder="或输入头像URL" size="small" />
            </div>
          </div>
        </el-form-item>

        <el-form-item label="网站名称">
          <el-input v-model="form.siteName" placeholder="我的个人网站" />
        </el-form-item>
        <el-form-item label="网站描述">
          <el-input v-model="form.siteDescription" type="textarea" :rows="2" placeholder="网站简介" />
        </el-form-item>
        <el-form-item label="关于我（Markdown格式）">
          <MdEditor v-model="form.aboutContent" style="height: 300px" />
        </el-form-item>
      </div>

      <!-- 联系方式 -->
      <div class="form-section card">
        <h3>联系方式</h3>
        <el-form-item label="邮箱">
          <el-input v-model="form.email" placeholder="your@email.com" />
        </el-form-item>
        <el-form-item label="GitHub">
          <el-input v-model="form.gitHub" placeholder="GitHub主页地址" />
        </el-form-item>
        <el-form-item label="Twitter">
          <el-input v-model="form.twitter" placeholder="Twitter主页地址" />
        </el-form-item>
        <el-form-item label="微信号">
          <el-input v-model="form.weChat" placeholder="微信号" />
        </el-form-item>
        <el-form-item label="QQ号">
          <el-input v-model="form.qq" placeholder="QQ号" />
        </el-form-item>
      </div>

      <!-- 私密区域设置 -->
      <div class="form-section card">
        <h3>私密区域设置</h3>
        <el-form-item label="私密区域密码">
          <el-input v-model="form.privatePassword" type="password" placeholder="留空则不修改" show-password />
        </el-form-item>
      </div>

      <!-- Giscus评论设置 -->
      <div class="form-section card">
        <h3>Giscus评论设置</h3>
        <el-form-item label="仓库名称">
          <el-input v-model="form.giscusRepo" placeholder="username/repo" />
        </el-form-item>
        <el-form-item label="仓库ID">
          <el-input v-model="form.giscusRepoId" placeholder="从Giscus获取" />
        </el-form-item>
        <el-form-item label="分类名称">
          <el-input v-model="form.giscusCategory" placeholder="Announcements" />
        </el-form-item>
        <el-form-item label="分类ID">
          <el-input v-model="form.giscusCategoryId" placeholder="从Giscus获取" />
        </el-form-item>
      </div>

      <el-form-item>
        <el-button type="primary" @click="handleSave" :loading="saving">保存设置</el-button>
      </el-form-item>
    </el-form>
  </div>
</template>

<script setup lang="ts">
/**
 * 网站设置页面
 * 管理网站全局配置，支持头像上传
 */
import { ref, onMounted, computed } from 'vue'
import { ElMessage } from 'element-plus'
import { MdEditor } from 'md-editor-v3'
import 'md-editor-v3/lib/style.css'
import { getSettings, updateSettings } from '@/api'

// 表单数据
const form = ref({
  siteName: '',
  siteDescription: '',
  avatar: '',
  aboutContent: '',
  email: '',
  gitHub: '',
  twitter: '',
  weChat: '',
  qq: '',
  privatePassword: '',
  giscusRepo: '',
  giscusRepoId: '',
  giscusCategory: '',
  giscusCategoryId: ''
})

const saving = ref(false)

// 上传请求头
const uploadHeaders = computed(() => ({
  Authorization: `Bearer ${localStorage.getItem('admin_token')}`
}))

/**
 * 头像上传前验证
 */
const beforeAvatarUpload = (file: File) => {
  const isImage = file.type.startsWith('image/')
  const isLt5M = file.size / 1024 / 1024 < 5

  if (!isImage) {
    ElMessage.error('只能上传图片文件')
    return false
  }
  if (!isLt5M) {
    ElMessage.error('头像大小不能超过5MB')
    return false
  }
  return true
}

/**
 * 头像上传成功回调
 */
const handleAvatarSuccess = (response: { url: string }) => {
  form.value.avatar = response.url
  ElMessage.success('头像上传成功')
}

/**
 * 保存设置
 */
const handleSave = async () => {
  saving.value = true
  try {
    await updateSettings(form.value)
    ElMessage.success('保存成功')
  } catch {
    ElMessage.error('保存失败')
  } finally {
    saving.value = false
  }
}

onMounted(async () => {
  try {
    const { data } = await getSettings()
    form.value = {
      ...form.value,
      siteName: data.siteName || '',
      siteDescription: data.siteDescription || '',
      avatar: data.avatar || '',
      aboutContent: data.aboutContent || '',
      email: data.email || '',
      gitHub: data.gitHub || '',
      twitter: data.twitter || '',
      weChat: data.weChat || '',
      qq: data.qq || '',
      giscusRepo: data.giscusRepo || '',
      giscusRepoId: data.giscusRepoId || '',
      giscusCategory: data.giscusCategory || '',
      giscusCategoryId: data.giscusCategoryId || ''
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

.form-section {
  margin-bottom: 24px;
  padding: 24px;
}

.form-section h3 {
  font-size: 18px;
  margin-bottom: 20px;
  color: var(--text-primary);
  padding-bottom: 12px;
  border-bottom: 1px solid var(--border-color);
}

/* 头像上传 */
.avatar-upload {
  display: flex;
  align-items: flex-start;
  gap: 24px;
}

.avatar-preview {
  width: 100px;
  height: 100px;
  border-radius: 50%;
  overflow: hidden;
  border: 3px solid var(--border-color);
  flex-shrink: 0;
}

.avatar-preview img {
  width: 100%;
  height: 100%;
  object-fit: cover;
}

.avatar-actions {
  display: flex;
  flex-direction: column;
  gap: 12px;
}
</style>
