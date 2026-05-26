<template>
  <!-- 摄影作品页面 - 壁纸网站风格（瀑布流、懒加载、无限滚动） -->
  <div class="gallery">
    <div class="container">
      <h1 class="page-title">摄影作品</h1>

      <!-- 分类筛选 -->
      <div class="category-filter">
        <span
          class="tag"
          :class="{ active: !selectedCategory }"
          @click="selectedCategory = ''"
        >
          全部
        </span>
        <span
          v-for="cat in photoCategories"
          :key="cat"
          class="tag"
          :class="{ active: selectedCategory === cat }"
          @click="selectedCategory = cat"
        >
          {{ cat }}
        </span>
      </div>

      <!-- 瀑布流照片墙 -->
      <div class="masonry-grid" ref="masonryRef">
        <div
          v-for="photo in photos"
          :key="photo.id"
          class="masonry-item"
          @click="openLightbox(photo)"
        >
          <div class="photo-wrapper" :style="{ paddingBottom: getAspectRatio(photo) }">
            <img
              :src="photo.thumbnailPath || photo.filePath"
              :alt="photo.title || '摄影作品'"
              loading="lazy"
              @load="onImageLoad"
            />
          </div>
          <div class="photo-info" v-if="photo.title">
            <span>{{ photo.title }}</span>
          </div>
        </div>
      </div>

      <!-- 加载更多触发器 -->
      <div ref="loadMoreRef" class="load-more">
        <div v-if="loading" class="loading-spinner">
          <el-icon class="is-loading"><Loading /></el-icon>
          加载中...
        </div>
        <div v-else-if="!hasMore && photos.length > 0" class="no-more">
          已加载全部照片
        </div>
      </div>

      <!-- 灯箱预览 -->
      <Teleport to="body">
        <div v-if="lightboxVisible" class="lightbox" @click="closeLightbox">
          <div class="lightbox-content" @click.stop>
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
              v-if="currentIndex < photos.length - 1"
              class="lightbox-next"
              @click="nextPhoto"
            >&rsaquo;</button>
            <div class="lightbox-info" v-if="currentPhoto?.title || currentPhoto?.description">
              <h3 v-if="currentPhoto?.title">{{ currentPhoto.title }}</h3>
              <p v-if="currentPhoto?.description">{{ currentPhoto.description }}</p>
            </div>
          </div>
        </div>
      </Teleport>
    </div>
  </div>
</template>

<script setup lang="ts">
/**
 * 摄影作品页面
 * 壁纸网站风格：瀑布流布局、懒加载、无限滚动、灯箱预览
 */
import { ref, onMounted, onUnmounted, watch, nextTick } from 'vue'
import { Loading } from '@element-plus/icons-vue'
import { getPhotos } from '@/api'
import type { Photo } from '@/types'

// 照片列表
const photos = ref<Photo[]>([])

// 分类筛选
const selectedCategory = ref('')
const photoCategories = ref<string[]>([])

// 分页和加载状态
const currentPage = ref(1)
const pageSize = 20
const hasMore = ref(true)
const loading = ref(false)

// 灯箱状态
const lightboxVisible = ref(false)
const currentPhoto = ref<Photo | null>(null)
const currentIndex = ref(0)

// DOM引用
const masonryRef = ref<HTMLElement | null>(null)
const loadMoreRef = ref<HTMLElement | null>(null)

// Intersection Observer
let observer: IntersectionObserver | null = null

/**
 * 获取照片宽高比（用于瀑布流）
 * 默认 4:3 比例
 */
const getAspectRatio = (_photo: Photo) => {
  return '75%'
}

/**
 * 图片加载完成回调
 */
const onImageLoad = (e: Event) => {
  const img = e.target as HTMLImageElement
  img.style.opacity = '1'
}

/**
 * 打开灯箱预览
 */
const openLightbox = (photo: Photo) => {
  currentPhoto.value = photo
  currentIndex.value = photos.value.findIndex(p => p.id === photo.id)
  lightboxVisible.value = true
  document.body.style.overflow = 'hidden'
}

/**
 * 关闭灯箱
 */
const closeLightbox = () => {
  lightboxVisible.value = false
  document.body.style.overflow = ''
}

/**
 * 上一张照片
 */
const prevPhoto = () => {
  if (currentIndex.value > 0) {
    currentIndex.value--
    currentPhoto.value = photos.value[currentIndex.value]
  }
}

/**
 * 下一张照片
 */
const nextPhoto = () => {
  if (currentIndex.value < photos.value.length - 1) {
    currentIndex.value++
    currentPhoto.value = photos.value[currentIndex.value]
  }
}

/**
 * 获取照片列表
 */
const fetchPhotos = async (reset = false) => {
  if (loading.value) return

  loading.value = true

  try {
    const { data } = await getPhotos({
      page: reset ? 1 : currentPage.value,
      pageSize,
      category: selectedCategory.value || undefined
    })

    if (reset) {
      photos.value = data.items
      currentPage.value = 2
    } else {
      photos.value = [...photos.value, ...data.items]
      currentPage.value++
    }

    hasMore.value = data.hasNextPage

    // 提取分类
    const cats = new Set(photos.value.map(p => p.category).filter(Boolean))
    photoCategories.value = Array.from(cats) as string[]
  } catch {
    // 静默处理
  } finally {
    loading.value = false
  }
}

/**
 * 设置 Intersection Observer 实现无限滚动
 */
const setupObserver = () => {
  if (!loadMoreRef.value) return

  observer = new IntersectionObserver(
    (entries) => {
      if (entries[0].isIntersecting && hasMore.value && !loading.value) {
        fetchPhotos()
      }
    },
    { threshold: 0.1 }
  )

  observer.observe(loadMoreRef.value)
}

// 监听分类变化
watch(selectedCategory, () => {
  fetchPhotos(true)
})

// 键盘事件（灯箱）
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
  fetchPhotos(true)
  nextTick(setupObserver)
  window.addEventListener('keydown', handleKeydown)
})

onUnmounted(() => {
  observer?.disconnect()
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
  padding: 6px 16px;
  background: var(--bg-card);
  border: 1px solid var(--border-color);
  border-radius: 20px;
  font-size: 14px;
  cursor: pointer;
  transition: all 0.2s ease;
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
  border-radius: var(--radius);
  overflow: hidden;
  cursor: pointer;
  position: relative;
  background: var(--bg-secondary);
}

.photo-wrapper {
  position: relative;
  overflow: hidden;
}

.photo-wrapper img {
  width: 100%;
  height: 100%;
  object-fit: cover;
  position: absolute;
  top: 0;
  left: 0;
  opacity: 0;
  transition: opacity 0.3s ease, transform 0.3s ease;
}

.masonry-item:hover .photo-wrapper img {
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

/* 加载更多 */
.load-more {
  padding: 40px 0;
  text-align: center;
}

.loading-spinner {
  color: var(--text-secondary);
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 8px;
}

.no-more {
  color: var(--text-secondary);
  font-size: 14px;
}

/* 灯箱样式 */
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

.lightbox-content {
  position: relative;
  max-width: 90vw;
  max-height: 90vh;
}

.lightbox-img {
  max-width: 90vw;
  max-height: 85vh;
  object-fit: contain;
}

.lightbox-close {
  position: absolute;
  top: -40px;
  right: 0;
  background: none;
  border: none;
  color: white;
  font-size: 32px;
  cursor: pointer;
  width: 40px;
  height: 40px;
  display: flex;
  align-items: center;
  justify-content: center;
}

.lightbox-prev,
.lightbox-next {
  position: absolute;
  top: 50%;
  transform: translateY(-50%);
  background: rgba(255, 255, 255, 0.2);
  border: none;
  color: white;
  font-size: 48px;
  cursor: pointer;
  width: 60px;
  height: 80px;
  display: flex;
  align-items: center;
  justify-content: center;
  transition: background 0.2s ease;
}

.lightbox-prev:hover,
.lightbox-next:hover {
  background: rgba(255, 255, 255, 0.3);
}

.lightbox-prev {
  left: -80px;
}

.lightbox-next {
  right: -80px;
}

.lightbox-info {
  position: absolute;
  bottom: -60px;
  left: 0;
  right: 0;
  text-align: center;
  color: white;
}

.lightbox-info h3 {
  margin: 0 0 4px;
  font-size: 16px;
}

.lightbox-info p {
  margin: 0;
  font-size: 14px;
  opacity: 0.8;
}
</style>
