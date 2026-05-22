<template>
  <!-- 项目展示页面 -->
  <div class="projects">
    <div class="container">
      <h1 class="page-title">项目展示</h1>

      <div class="project-grid">
        <div v-for="project in projects" :key="project.id" class="project-card card">
          <div class="card-cover" v-if="project.coverImage">
            <img :src="project.coverImage" :alt="project.title" />
          </div>
          <div class="card-body">
            <h3 class="project-title">{{ project.title }}</h3>
            <p class="project-desc">{{ project.description }}</p>
            <div class="tech-stack" v-if="project.techStack">
              <span v-for="tech in parseTechStack(project.techStack)" :key="tech" class="tech-tag">
                {{ tech }}
              </span>
            </div>
            <div class="project-links">
              <a v-if="project.gitHubUrl" :href="project.gitHubUrl" target="_blank" class="link-btn">
                GitHub
              </a>
              <a v-if="project.demoUrl" :href="project.demoUrl" target="_blank" class="link-btn">
                在线演示
              </a>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
/**
 * 项目展示页面
 * 展示所有项目，支持GitHub链接和在线演示
 */
import { ref, onMounted } from 'vue'
import { getProjects } from '@/api'
import type { Project } from '@/types'

// 项目列表
const projects = ref<Project[]>([])

/**
 * 解析技术栈标签
 */
const parseTechStack = (techStack: string): string[] => {
  try {
    return JSON.parse(techStack)
  } catch {
    return techStack.split(',').map(t => t.trim())
  }
}

onMounted(async () => {
  try {
    const { data } = await getProjects()
    projects.value = data
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

.project-grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(350px, 1fr));
  gap: 24px;
}

.project-card {
  overflow: hidden;
}

.card-cover {
  height: 200px;
  overflow: hidden;
}

.card-cover img {
  width: 100%;
  height: 100%;
  object-fit: cover;
  transition: transform 0.3s ease;
}

.card-cover:hover img {
  transform: scale(1.05);
}

.card-body {
  padding: 20px;
}

.project-title {
  font-size: 20px;
  margin-bottom: 8px;
  color: var(--text-primary);
}

.project-desc {
  color: var(--text-secondary);
  font-size: 14px;
  line-height: 1.6;
  margin-bottom: 16px;
}

.tech-stack {
  display: flex;
  flex-wrap: wrap;
  gap: 8px;
  margin-bottom: 16px;
}

.tech-tag {
  padding: 4px 10px;
  background: var(--bg-secondary);
  color: var(--accent-primary);
  border-radius: 12px;
  font-size: 12px;
}

.project-links {
  display: flex;
  gap: 12px;
}

.link-btn {
  padding: 8px 16px;
  background: var(--accent-primary);
  color: white;
  border-radius: var(--radius);
  font-size: 14px;
  transition: background 0.2s ease;
}

.link-btn:hover {
  background: var(--accent-secondary);
}
</style>
