<template>
  <!-- 摄影作品页面 - 壁纸网站风格 -->
  <div class="gallery">
    <div class="container">
      <h1 class="page-title">摄影作品</h1>

      <!-- 分类筛选 -->
      <div class="category-filter">
        <button
          class="tag"
          :class="{ active: !selectedCategory }"
          @click="selectedCategory = ''"
        >
          全部
        </button>
        <button
          v-for="cat in photoCategories"
          :key="cat"
          class="tag"
          :class="{ active: selectedCategory === cat }"
          @click="selectedCategory = cat"
        >
          {{ cat }}
        </button>
      </div>

      <!-- 瀑布流照片墙 -->
      <div class="masonry-grid">
        <div
          v-for="photo in filteredPhotos"
          :key="photo.id"
          class="masonry-item"
          @click="openLightbox(photo)"
        >
          <img
            :src="photo.thumbnailPath || photo.filePath"
            :alt="photo.title || '摄影作品'"
            loading="lazy"
            class="photo-img"
          />
          <div class="photo-info" v-if="photo.title">
            <span>{{ photo.title }}</span>
          </div>
        </div>
      </div>

      <!-- 加载状态 -->
      <div class="load-more" v-if="loading">
        <el-icon class="is-loading"><Loading /></el-icon>
        加载中...
      </div>
      <div class="no-more" v-else-if="filteredPhotos.length === 0">
        暂无照片
      </div>
    </div>

    <!-- 灯箱预览（带评论） -->
    <Teleport to="body">
      <div v-if="lightboxVisible" class="lightbox" @click="closeLightbox">
        <div class="lightbox-container" @click.stop>
          <!-- 左侧图片 -->
          <div class="lightbox-left">
            <button class="lightbox-close" @click="closeLightbox">&times;</button>
            <button
              v-if="currentIndex > 0"
              class="lightbox-prev"
              @click="prevPhoto"
            >&lsaquo;</button>
            <img
              :src="currentPhoto?.filePath"
              class="lightbox-img"
              :alt="currentPhoto?.title"
            />
            <button
              v-if="currentIndex < filteredPhotos.length - 1"
              class="lightbox-next"
              @click="nextPhoto"
            >&rsaquo;</button>
          </div>

          <!-- 右侧信息和评论 -->
          <div class="lightbox-right">
            <div class="photo-detail">
              <h3 v-if="currentPhoto?.title" class="photo-title">{{ currentPhoto.title }}</h3>
              <p v-if="currentPhoto?.description" class="photo-desc">{{ currentPhoto.description }}</p>
              <p class="photo-date">{{ formatDate(currentPhoto?.createdAt) }}</p>
            </div>

            <!-- 评论区 -->
            <div class="comments-section">
              <h4 class="comments-title">评论</h4>
              <div class="comments-list" ref="commentsListRef">
                <div v-for="comment in comments" :key="comment.id" class="comment-item">
                  <div class="comment-header">
                    <span class="comment-author">{{ comment.author }}</span>
                    <span class="comment-time">{{ formatTime(comment.createdAt) }}</span>
                  </div>
                  <p class="comment-content">{{ comment.content }}</p>
                </div>
                <div v-if="comments.length === 0" class="no-comments">
                  暂无评论，来说点什么吧~
                </div>
              </div>

              <!-- 评论输入 -->
              <div class="comment-input">
                <input
                  v-model="newComment"
                  placeholder="写下你的评论..."
                  @keyup.enter="submitComment"
                  class="comment-field"
                />
                <button class="btn-submit" @click="submitComment" :disabled="!newComment.trim()">
                  发送
                </button>
              </div>
            </div>
          </div>
        </div>
      </div>
    </Teleport>
  </div>
</template>

<script setup lang="ts">
/**
 * 摄影作品页面
 * 壁纸网站风格：瀑布流布局、懒加载、灯箱预览、评论功能
 */
import { ref, computed, onMounted, onUnmounted, watch } from 'vue'
import { Loading } from '@element-plus/icons-vue'
import { getPhotos } from '@/api'
import type { Photo } from '@/types'

// 评论类型
interface Comment {
  id: number
  photoId: number
  author: string
  content: string
  createdAt: string
}

// 照片列表
const photos = ref<Photo[]>([])

// 分类筛选
const selectedCategory = ref('')
const photoCategories = ref<string[]>([])

// 加载状态
const loading = ref(false)

// 灯箱状态
const lightboxVisible = ref(false)
const currentPhoto = ref<Photo | null>(null)
const currentIndex = ref(0)

// 评论相关
const comments = ref<Comment[]>([])
const newComment = ref('')
const commentsListRef = ref<HTMLElement | null>(null)

/**
 * 根据分类过滤照片
 */
const filteredPhotos = computed(() => {
  if (!selectedCategory.value) return photos.value
  return photos.value.filter(p => p.category === selectedCategory.value)
})

/**
 * 格式化日期
 */
const formatDate = (dateStr?: string) => {
  if (!dateStr) return ''
  return new Date(dateStr).toLocaleDateString('zh-CN', {
    year: 'numeric',
    month: 'long',
    day: 'numeric'
  })
}

/**
 * 格式化时间（用于评论）
 */
const formatTime = (dateStr: string) => {
  const date = new Date(dateStr)
  const now = new Date()
  const diff = now.getTime() - date.getTime()
  const minutes = Math.floor(diff / 60000)
  const hours = Math.floor(diff / 3600000)
  const days = Math.floor(diff / 86400000)

  if (minutes < 1) return '刚刚'
  if (minutes < 60) return `${minutes}分钟前`
  if (hours < 24) return `${hours}小时前`
  if (days < 7) return `${days}天前`
  return formatDate(dateStr)
}

/**
 * 打开灯箱
 */
const openLightbox = (photo: Photo) => {
  currentPhoto.value = photo
  currentIndex.value = filteredPhotos.value.findIndex(p => p.id === photo.id)
  lightboxVisible.value = true
  document.body.style.overflow = 'hidden'
  loadComments(photo.id)
}

/**
 * 关闭灯箱
 */
const closeLightbox = () => {
  lightboxVisible.value = false
  document.body.style.overflow = ''
}

/**
 * 上一张
 */
const prevPhoto = () => {
  if (currentIndex.value > 0) {
    currentIndex.value--
    currentPhoto.value = filteredPhotos.value[currentIndex.value]
    loadComments(currentPhoto.value.id)
  }
}

/**
 * 下一张
 */
const nextPhoto = () => {
  if (currentIndex.value < filteredPhotos.value.length - 1) {
    currentIndex.value++
    currentPhoto.value = filteredPhotos.value[currentIndex.value]
    loadComments(currentPhoto.value.id)
  }
}

/**
 * 加载评论
 */
const loadComments = (photoId: number) => {
  // 从本地存储加载评论
  const stored = localStorage.getItem(`photo_comments_${photoId}`)
  if (stored) {
    comments.value = JSON.parse(stored)
  } else {
    comments.value = []
  }
}

/**
 * 提交评论
 */
const submitComment = () => {
  if (!newComment.value.trim() || !currentPhoto.value) return

  const comment: Comment = {
    id: Date.now(),
    photoId: currentPhoto.value.id,
    author: '访客',
    content: newComment.value.trim(),
    createdAt: new Date().toISOString()
  }

  comments.value.push(comment)

  // 保存到本地存储
  localStorage.setItem(
    `photo_comments_${currentPhoto.value.id}`,
    JSON.stringify(comments.value)
  )

  newComment.value = ''

  // 滚动到底部
  setTimeout(() => {
    if (commentsListRef.value) {
      commentsListRef.value.scrollTop = commentsListRef.value.scrollHeight
    }
  }, 100)
}

/**
 * 获取照片列表
 */
const fetchPhotos = async () => {
  loading.value = true
  try {
    const { data } = await getPhotos({ pageSize: 100 })
    photos.value = data.items

    // 提取分类
    const cats = new Set(data.items.map(p => p.category).filter(Boolean))
    photoCategories.value = Array.from(cats) as string[]
  } catch {
    // 静默处理
  } finally {
    loading.value = false
  }
}

// 键盘事件
const handleKeydown = (e: KeyboardEvent) => {
  if (!lightboxVisible.value) return
  switch (e.key) {
    case 'Escape':
      closeLightbox()
      break
    case 'ArrowLeft':
      prevPhoto()
      break
    case 'ArrowRight':
      nextPhoto()
      break
  }
}

onMounted(() => {
  fetchPhotos()
  window.addEventListener('keydown', handleKeydown)
})

onUnmounted(() => {
  window.removeEventListener('keydown', handleKeydown)
})
</script>

<style scoped>
.page-title {
  font-size: 28px;
  margin-bottom: 32px;
  color: var(--text-primary);
}

/* 分类筛选 */
.category-filter {
  display: flex;
  flex-wrap: wrap;
  gap: 8px;
  margin-bottom: 32px;
}

.tag {
  padding: 8px 20px;
  background: var(--bg-card);
  border: 1px solid var(--border-color);
  border-radius: 24px;
  font-size: 14px;
  cursor: pointer;
  transition: all 0.2s ease;
  color: var(--text-secondary);
}

.tag:hover,
.tag.active {
  background: var(--accent-primary);
  color: white;
  border-color: var(--accent-primary);
}

/* 瀑布流布局 */
.masonry-grid {
  columns: 4;
  column-gap: 16px;
}

@media (max-width: 1200px) {
  .masonry-grid { columns: 3; }
}

@media (max-width: 768px) {
  .masonry-grid { columns: 2; }
}

@media (max-width: 480px) {
  .masonry-grid { columns: 1; }
}

.masonry-item {
  break-inside: avoid;
  margin-bottom: 16px;
  border-radius: 12px;
  overflow: hidden;
  cursor: pointer;
  position: relative;
  background: var(--bg-secondary);
}

.photo-img {
  width: 100%;
  display: block;
  transition: transform 0.3s ease;
}

.masonry-item:hover .photo-img {
  transform: scale(1.05);
}

.photo-info {
  position: absolute;
  bottom: 0;
  left: 0;
  right: 0;
  padding: 12px;
  background: linear-gradient(transparent, rgba(0, 0, 0, 0.7));
  color: white;
  font-size: 14px;
  opacity: 0;
  transition: opacity 0.3s ease;
}

.masonry-item:hover .photo-info {
  opacity: 1;
}

/* 加载状态 */
.load-more {
  padding: 40px 0;
  text-align: center;
  color: var(--text-secondary);
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 8px;
}

.no-more {
  padding: 40px 0;
  text-align: center;
  color: var(--text-secondary);
}

/* ========== 灯箱样式 ========== */
.lightbox {
  position: fixed;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background: rgba(0, 0, 0, 0.9);
  z-index: 9999;
  display: flex;
  align-items: center;
  justify-content: center;
}

.lightbox-container {
  display: flex;
  width: 90vw;
  height: 85vh;
  max-width: 1400px;
  background: var(--bg-card);
  border-radius: 16px;
  overflow: hidden;
}

.lightbox-left {
  flex: 1;
  position: relative;
  display: flex;
  align-items: center;
  justify-content: center;
  background: #000;
}

.lightbox-img {
  max-width: 100%;
  max-height: 100%;
  object-fit: contain;
}

.lightbox-close {
  position: absolute;
  top: 16px;
  left: 16px;
  background: rgba(255, 255, 255, 0.2);
  border: none;
  color: white;
  font-size: 24px;
  cursor: pointer;
  width: 40px;
  height: 40px;
  border-radius: 50%;
  display: flex;
  align-items: center;
  justify-content: center;
  z-index: 10;
}

.lightbox-prev,
.lightbox-next {
  position: absolute;
  top: 50%;
  transform: translateY(-50%);
  background: rgba(255, 255, 255, 0.2);
  border: none;
  color: white;
  font-size: 36px;
  cursor: pointer;
  width: 48px;
  height: 80px;
  display: flex;
  align-items: center;
  justify-content: center;
  z-index: 10;
}

.lightbox-prev { left: 8px; }
.lightbox-next { right: 8px; }

.lightbox-prev:hover,
.lightbox-next:hover {
  background: rgba(255, 255, 255, 0.3);
}

/* 右侧信息和评论 */
.lightbox-right {
  width: 360px;
  display: flex;
  flex-direction: column;
  border-left: 1px solid var(--border-color);
}

.photo-detail {
  padding: 24px;
  border-bottom: 1px solid var(--border-color);
}

.photo-title {
  font-size: 18px;
  font-weight: 600;
  margin-bottom: 8px;
  color: var(--text-primary);
}

.photo-desc {
  font-size: 14px;
  color: var(--text-secondary);
  line-height: 1.6;
  margin-bottom: 8px;
}

.photo-date {
  font-size: 12px;
  color: var(--text-muted);
}

/* 评论区 */
.comments-section {
  flex: 1;
  display: flex;
  flex-direction: column;
  overflow: hidden;
}

.comments-title {
  padding: 16px 24px;
  font-size: 16px;
  font-weight: 600;
  border-bottom: 1px solid var(--border-color);
}

.comments-list {
  flex: 1;
  overflow-y: auto;
  padding: 16px 24px;
}

.comment-item {
  margin-bottom: 16px;
  padding-bottom: 16px;
  border-bottom: 1px solid var(--border-light);
}

.comment-item:last-child {
  border-bottom: none;
}

.comment-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 8px;
}

.comment-author {
  font-size: 14px;
  font-weight: 500;
  color: var(--text-primary);
}

.comment-time {
  font-size: 12px;
  color: var(--text-muted);
}

.comment-content {
  font-size: 14px;
  color: var(--text-secondary);
  line-height: 1.6;
}

.no-comments {
  text-align: center;
  color: var(--text-muted);
  padding: 40px 0;
  font-size: 14px;
}

/* 评论输入 */
.comment-input {
  padding: 16px 24px;
  display: flex;
  gap: 12px;
  border-top: 1px solid var(--border-color);
  background: var(--bg-secondary);
}

.comment-field {
  flex: 1;
  padding: 10px 16px;
  border: 1px solid var(--border-color);
  border-radius: 8px;
  font-size: 14px;
  background: var(--bg-card);
  color: var(--text-primary);
  outline: none;
}

.comment-field:focus {
  border-color: var(--accent-primary);
}

.btn-submit {
  padding: 10px 20px;
  background: var(--accent-primary);
  color: white;
  border: none;
  border-radius: 8px;
  font-size: 14px;
  cursor: pointer;
  transition: background 0.2s ease;
}

.btn-submit:hover:not(:disabled) {
  background: var(--accent-secondary);
}

.btn-submit:disabled {
  opacity: 0.5;
  cursor: not-allowed;
}

/* 响应式 */
@media (max-width: 768px) {
  .lightbox-container {
    flex-direction: column;
    width: 95vw;
    height: 90vh;
  }

  .lightbox-left {
    flex: 1;
  }

  .lightbox-right {
    width: 100%;
    height: 40vh;
  }
}
</style>
