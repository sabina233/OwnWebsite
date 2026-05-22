<template>
  <!-- 摄影作品页面 - 瀑布流展示 -->
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

      <!-- 照片网格 -->
      <div class="photo-grid">
        <div
          v-for="photo in photos"
          :key="photo.id"
          class="photo-item"
          @click="openLightbox(photo)"
        >
          <img :src="photo.filePath" :alt="photo.title || '摄影作品'" loading="lazy" />
          <div class="photo-overlay" v-if="photo.title">
            <span>{{ photo.title }}</span>
          </div>
        </div>
      </div>

      <!-- 灯箱预览 -->
      <el-dialog v-model="lightboxVisible" :title="currentPhoto?.title" width="80%">
        <img :src="currentPhoto?.filePath" class="lightbox-img" />
        <p v-if="currentPhoto?.description" class="lightbox-desc">
          {{ currentPhoto.description }}
        </p>
      </el-dialog>
    </div>
  </div>
</template>

<script setup lang="ts">
/**
 * 摄影作品页面
 * 瀑布流展示照片，支持分类筛选和灯箱预览
 */
import { ref, onMounted, watch } from 'vue'
import { getPhotos } from '@/api'
import type { Photo } from '@/types'

// 照片列表
const photos = ref<Photo[]>([])

// 分类筛选
const selectedCategory = ref('')
const photoCategories = ref<string[]>([])

// 灯箱状态
const lightboxVisible = ref(false)
const currentPhoto = ref<Photo | null>(null)

/**
 * 打开灯箱预览
 */
const openLightbox = (photo: Photo) => {
  currentPhoto.value = photo
  lightboxVisible.value = true
}

/**
 * 获取照片列表
 */
const fetchPhotos = async () => {
  try {
    const { data } = await getPhotos({
      pageSize: 50,
      category: selectedCategory.value || undefined
    })
    photos.value = data.items
    // 提取所有分类
    const cats = new Set(data.items.map(p => p.category).filter(Boolean))
    photoCategories.value = Array.from(cats) as string[]
  } catch {
    // 静默处理
  }
}

watch(selectedCategory, fetchPhotos)

onMounted(fetchPhotos)
</script>

<style scoped>
.page-title {
  font-size: 28px;
  margin-bottom: 32px;
  color: var(--text-primary);
}

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

.photo-grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(250px, 1fr));
  gap: 16px;
}

.photo-item {
  position: relative;
  border-radius: var(--radius);
  overflow: hidden;
  cursor: pointer;
  aspect-ratio: 4/3;
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
  padding: 12px;
  background: linear-gradient(transparent, rgba(0, 0, 0, 0.7));
  color: white;
  font-size: 14px;
  opacity: 0;
  transition: opacity 0.3s ease;
}

.photo-item:hover .photo-overlay {
  opacity: 1;
}

.lightbox-img {
  width: 100%;
  max-height: 70vh;
  object-fit: contain;
}

.lightbox-desc {
  margin-top: 16px;
  color: var(--text-secondary);
}
</style>
