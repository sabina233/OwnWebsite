<template>
  <!-- 照片管理页面 - 支持分类分区、批量上传、折叠分区 -->
  <div class="photo-upload">
    <div class="page-header">
      <h1 class="page-title">照片管理</h1>
      <div class="header-actions">
        <el-button type="primary" @click="showCategoryDialog = true">
          <el-icon><FolderAdd /></el-icon>
          新建分区
        </el-button>
      </div>
    </div>

    <!-- 分区列表（可折叠） -->
    <el-collapse v-model="activeCategories">
      <el-collapse-item
        v-for="category in categories"
        :key="category.id"
        :name="category.id"
      >
        <template #title>
          <div class="category-header">
            <span class="category-name">{{ category.name }}</span>
            <el-tag size="small">{{ getCategoryPhotos(category.name).length }} 张</el-tag>
          </div>
        </template>

        <!-- 该分区下的照片上传 -->
        <div class="category-upload">
          <el-upload
            action="/api/upload/photo"
            :headers="uploadHeaders"
            :data="{ category: category.name }"
            :on-success="handleUploadSuccess"
            :before-upload="beforeUpload"
            multiple
            :show-file-list="false"
          >
            <el-button size="small" type="primary">
              <el-icon><Upload /></el-icon>
              批量上传
            </el-button>
          </el-upload>
        </div>

        <!-- 该分区下的照片列表 -->
        <div class="photo-grid">
          <div
            v-for="photo in getCategoryPhotos(category.name)"
            :key="photo.id"
            class="photo-item"
          >
            <img :src="photo.filePath" :alt="photo.title" loading="lazy" />
            <div class="photo-overlay">
              <span class="photo-title">{{ photo.title || '未命名' }}</span>
              <el-button size="small" type="danger" @click="handleDelete(photo.id)">
                <el-icon><Delete /></el-icon>
              </el-button>
            </div>
          </div>
        </div>

        <div v-if="getCategoryPhotos(category.name).length === 0" class="empty-tip">
          暂无照片，请上传
        </div>
      </el-collapse-item>
    </el-collapse>

    <!-- 未分类照片 -->
    <div class="uncategorized-section" v-if="uncategorizedPhotos.length > 0">
      <h3>未分类照片</h3>
      <div class="photo-grid">
        <div v-for="photo in uncategorizedPhotos" :key="photo.id" class="photo-item">
          <img :src="photo.filePath" :alt="photo.title" loading="lazy" />
          <div class="photo-overlay">
            <span class="photo-title">{{ photo.title || '未命名' }}</span>
            <el-button size="small" type="danger" @click="handleDelete(photo.id)">
              <el-icon><Delete /></el-icon>
            </el-button>
          </div>
        </div>
      </div>
    </div>

    <!-- 新建分区对话框 -->
    <el-dialog v-model="showCategoryDialog" title="新建照片分区" width="400px">
      <el-form :model="newCategory" label-width="80px">
        <el-form-item label="分区名称">
          <el-input v-model="newCategory.name" placeholder="请输入分区名称" />
        </el-form-item>
        <el-form-item label="描述">
          <el-input v-model="newCategory.description" placeholder="分区描述（可选）" />
        </el-form-item>
      </el-form>
      <template #footer>
        <el-button @click="showCategoryDialog = false">取消</el-button>
        <el-button type="primary" @click="handleCreateCategory">创建</el-button>
      </template>
    </el-dialog>
  </div>
</template>

<script setup lang="ts">
/**
 * 照片管理页面
 * 支持分类分区、批量上传、折叠分区
 */
import { ref, onMounted, computed } from 'vue'
import { Upload, Delete, FolderAdd } from '@element-plus/icons-vue'
import { ElMessage, ElMessageBox } from 'element-plus'
import { getPhotos, deletePhoto, getCategories, createCategory } from '@/api'
import type { Photo, Category } from '@/types'

// 照片列表
const photos = ref<Photo[]>([])

// 分类列表
const categories = ref<Category[]>([])

// 当前展开的分区
const activeCategories = ref<number[]>([])

// 新建分区对话框
const showCategoryDialog = ref(false)
const newCategory = ref({ name: '', description: '' })

// 上传请求头
const uploadHeaders = computed(() => ({
  Authorization: `Bearer ${localStorage.getItem('admin_token')}`
}))

/**
 * 获取指定分类的照片
 */
const getCategoryPhotos = (categoryName: string) => {
  return photos.value.filter(p => p.category === categoryName)
}

/**
 * 获取未分类照片
 */
const uncategorizedPhotos = computed(() => {
  return photos.value.filter(p => !p.category)
})

/**
 * 上传前验证
 */
const beforeUpload = (file: File) => {
  const isImage = file.type.startsWith('image/')
  const isLt50M = file.size / 1024 / 1024 < 50

  if (!isImage) {
    ElMessage.error('只能上传图片文件')
    return false
  }
  if (!isLt50M) {
    ElMessage.error('图片大小不能超过50MB')
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
 * 创建新分区
 */
const handleCreateCategory = async () => {
  if (!newCategory.value.name) {
    ElMessage.warning('请输入分区名称')
    return
  }

  try {
    await createCategory({
      name: newCategory.value.name,
      slug: newCategory.value.name.toLowerCase().replace(/\s+/g, '-'),
      description: newCategory.value.description
    })
    ElMessage.success('创建成功')
    showCategoryDialog.value = false
    newCategory.value = { name: '', description: '' }
    fetchCategories()
  } catch {
    ElMessage.error('创建失败')
  }
}

/**
 * 获取照片列表
 */
const fetchPhotos = async () => {
  try {
    const { data } = await getPhotos({ pageSize: 200 })
    photos.value = data.items
  } catch {
    // 静默处理
  }
}

/**
 * 获取分类列表
 */
const fetchCategories = async () => {
  try {
    const { data } = await getCategories()
    categories.value = data
    // 默认展开第一个分区
    if (data.length > 0 && activeCategories.value.length === 0) {
      activeCategories.value = [data[0].id]
    }
  } catch {
    // 静默处理
  }
}

onMounted(() => {
  fetchPhotos()
  fetchCategories()
})
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

.header-actions {
  display: flex;
  gap: 12px;
}

.category-header {
  display: flex;
  align-items: center;
  gap: 12px;
}

.category-name {
  font-size: 16px;
  font-weight: 500;
}

.category-upload {
  margin-bottom: 16px;
}

.photo-grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(180px, 1fr));
  gap: 12px;
  margin-top: 12px;
}

.photo-item {
  position: relative;
  border-radius: var(--radius);
  overflow: hidden;
  aspect-ratio: 1;
  cursor: pointer;
}

.photo-item img {
  width: 100%;
  height: 100%;
  object-fit: cover;
  transition: transform 0.3s ease;
}

.photo-item:hover img {
  transform: scale(1.05);
}

.photo-overlay {
  position: absolute;
  bottom: 0;
  left: 0;
  right: 0;
  padding: 8px 12px;
  background: linear-gradient(transparent, rgba(0, 0, 0, 0.7));
  display: flex;
  justify-content: space-between;
  align-items: center;
  opacity: 0;
  transition: opacity 0.3s ease;
}

.photo-item:hover .photo-overlay {
  opacity: 1;
}

.photo-title {
  color: white;
  font-size: 12px;
  overflow: hidden;
  text-overflow: ellipsis;
  white-space: nowrap;
}

.empty-tip {
  text-align: center;
  padding: 24px;
  color: var(--text-secondary);
  font-size: 14px;
}

.uncategorized-section {
  margin-top: 24px;
  padding-top: 24px;
  border-top: 1px solid var(--border-color);
}

.uncategorized-section h3 {
  font-size: 18px;
  margin-bottom: 16px;
  color: var(--text-primary);
}
</style>
