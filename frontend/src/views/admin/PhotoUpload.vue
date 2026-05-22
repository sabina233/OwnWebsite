<template>
  <!-- 照片管理页面 -->
  <div class="photo-upload">
    <div class="page-header">
      <h1 class="page-title">照片管理</h1>
      <el-upload
        action="/api/upload/photo"
        :headers="uploadHeaders"
        :data="uploadData"
        :on-success="handleUploadSuccess"
        :before-upload="beforeUpload"
        :show-file-list="false"
      >
        <el-button type="primary">
          <el-icon><Upload /></el-icon>
          上传照片
        </el-button>
      </el-upload>
    </div>

    <!-- 上传信息 -->
    <div class="upload-form card">
      <el-form :inline="true">
        <el-form-item label="照片标题">
          <el-input v-model="uploadData.title" placeholder="标题（可选）" />
        </el-form-item>
        <el-form-item label="照片描述">
          <el-input v-model="uploadData.description" placeholder="描述（可选）" />
        </el-form-item>
        <el-form-item label="分类">
          <el-input v-model="uploadData.category" placeholder="分类（可选）" />
        </el-form-item>
      </el-form>
    </div>

    <!-- 照片列表 -->
    <div class="photo-grid">
      <div v-for="photo in photos" :key="photo.id" class="photo-item">
        <img :src="photo.filePath" :alt="photo.title" />
        <div class="photo-actions">
          <span class="photo-title">{{ photo.title || '未命名' }}</span>
          <el-button size="small" type="danger" @click="handleDelete(photo.id)">删除</el-button>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
/**
 * 照片管理页面
 * 支持上传和删除照片
 */
import { ref, onMounted, computed } from 'vue'
import { Upload } from '@element-plus/icons-vue'
import { ElMessage, ElMessageBox } from 'element-plus'
import { getPhotos, deletePhoto } from '@/api'
import type { Photo } from '@/types'

// 照片列表
const photos = ref<Photo[]>([])

// 上传数据
const uploadData = ref({
  title: '',
  description: '',
  category: ''
})

// 上传请求头
const uploadHeaders = computed(() => ({
  Authorization: `Bearer ${localStorage.getItem('admin_token')}`
}))

/**
 * 上传前验证
 */
const beforeUpload = (file: File) => {
  const isImage = file.type.startsWith('image/')
  const isLt10M = file.size / 1024 / 1024 < 10

  if (!isImage) {
    ElMessage.error('只能上传图片文件')
    return false
  }
  if (!isLt10M) {
    ElMessage.error('图片大小不能超过10MB')
    return false
  }
  return true
}

/**
 * 上传成功回调
 */
const handleUploadSuccess = () => {
  ElMessage.success('上传成功')
  fetchPhotos()
}

/**
 * 删除照片
 */
const handleDelete = async (id: number) => {
  try {
    await ElMessageBox.confirm('确定要删除这张照片吗？', '确认删除')
    await deletePhoto(id)
    ElMessage.success('删除成功')
    fetchPhotos()
  } catch {
    // 取消操作
  }
}

/**
 * 获取照片列表
 */
const fetchPhotos = async () => {
  try {
    const { data } = await getPhotos({ pageSize: 100 })
    photos.value = data.items
  } catch {
    // 静默处理
  }
}

onMounted(fetchPhotos)
</script>

<style scoped>
.page-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 24px;
}

.page-title {
  font-size: 24px;
  color: var(--text-primary);
}

.upload-form {
  margin-bottom: 24px;
  padding: 16px;
}

.photo-grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(200px, 1fr));
  gap: 16px;
}

.photo-item {
  border-radius: var(--radius);
  overflow: hidden;
  background: var(--bg-card);
  box-shadow: var(--shadow);
}

.photo-item img {
  width: 100%;
  height: 150px;
  object-fit: cover;
}

.photo-actions {
  padding: 12px;
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.photo-title {
  font-size: 14px;
  color: var(--text-primary);
  overflow: hidden;
  text-overflow: ellipsis;
  white-space: nowrap;
}
</style>
