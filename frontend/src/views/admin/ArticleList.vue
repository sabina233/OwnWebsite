<template>
  <!-- 文章管理页面 -->
  <div class="article-list">
    <div class="page-header">
      <h1 class="page-title">文章管理</h1>
      <el-button type="primary" @click="createNew">
        <el-icon><Plus /></el-icon>
        新建文章
      </el-button>
    </div>

    <el-table :data="articles" style="width: 100%">
      <el-table-column prop="title" label="标题" />
      <el-table-column prop="category" label="分类" width="120" />
      <el-table-column prop="viewCount" label="阅读量" width="100" />
      <el-table-column label="状态" width="100">
        <template #default="{ row }">
          <el-tag :type="row.isPublished ? 'success' : 'info'">
            {{ row.isPublished ? '已发布' : '草稿' }}
          </el-tag>
        </template>
      </el-table-column>
      <el-table-column label="操作" width="200">
        <template #default="{ row }">
          <el-button size="small" @click="editArticle(row.id)">编辑</el-button>
          <el-button size="small" type="danger" @click="handleDelete(row.id)">删除</el-button>
        </template>
      </el-table-column>
    </el-table>
  </div>
</template>

<script setup lang="ts">
/**
 * 文章管理页面
 * 展示文章列表，支持编辑和删除操作
 */
import { ref, onMounted } from 'vue'
import { useRouter } from 'vue-router'
import { Plus } from '@element-plus/icons-vue'
import { ElMessageBox, ElMessage } from 'element-plus'
import { getArticles, deleteArticle } from '@/api'
import type { ArticleListItem } from '@/types'

const router = useRouter()

// 文章列表
const articles = ref<ArticleListItem[]>([])

/**
 * 创建新文章
 */
const createNew = () => {
  router.push('/admin/articles/edit')
}

/**
 * 编辑文章
 */
const editArticle = (id: number) => {
  router.push(`/admin/articles/edit/${id}`)
}

/**
 * 删除文章
 */
const handleDelete = async (id: number) => {
  try {
    await ElMessageBox.confirm('确定要删除这篇文章吗？', '确认删除')
    await deleteArticle(id)
    ElMessage.success('删除成功')
    fetchArticles()
  } catch {
    // 取消操作
  }
}

/**
 * 获取文章列表
 */
const fetchArticles = async () => {
  try {
    const { data } = await getArticles({ pageSize: 100 })
    articles.value = data.items
  } catch {
    // 静默处理
  }
}

onMounted(fetchArticles)
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
</style>
