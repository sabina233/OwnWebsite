<template>
  <!-- 文章编辑页面 - 使用Markdown编辑器 -->
  <div class="article-editor">
    <h1 class="page-title">{{ isEdit ? '编辑文章' : '新建文章' }}</h1>

    <el-form :model="form" label-position="top">
      <el-form-item label="文章标题">
        <el-input v-model="form.title" placeholder="请输入文章标题" />
      </el-form-item>

      <el-form-item label="分类">
        <el-select v-model="form.category" placeholder="选择分类" allow-create filterable>
          <el-option v-for="cat in categories" :key="cat.id" :label="cat.name" :value="cat.name" />
        </el-select>
      </el-form-item>

      <el-form-item label="摘要">
        <el-input v-model="form.summary" type="textarea" :rows="2" placeholder="文章摘要（可选）" />
      </el-form-item>

      <el-form-item label="文章内容">
        <MdEditor v-model="form.content" style="height: 500px" />
      </el-form-item>

      <el-form-item label="封面图片">
        <el-input v-model="form.coverImage" placeholder="封面图片URL" />
      </el-form-item>

      <el-form-item label="标签">
        <el-input v-model="form.tags" placeholder="标签（用逗号分隔）" />
      </el-form-item>

      <el-form-item>
        <el-checkbox v-model="form.isPublished">发布文章</el-checkbox>
        <el-checkbox v-model="form.isPrivate">私密文章</el-checkbox>
      </el-form-item>

      <el-form-item>
        <el-button type="primary" @click="handleSave" :loading="saving">保存</el-button>
        <el-button @click="goBack">取消</el-button>
      </el-form-item>
    </el-form>
  </div>
</template>

<script setup lang="ts">
/**
 * 文章编辑页面
 * 使用Markdown编辑器编写文章
 */
import { ref, onMounted, computed } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { ElMessage } from 'element-plus'
import { MdEditor } from 'md-editor-v3'
import 'md-editor-v3/lib/style.css'
import { getArticle, createArticle, updateArticle, getCategories } from '@/api'
import type { Category } from '@/types'

const route = useRoute()
const router = useRouter()

// 是否为编辑模式
const isEdit = computed(() => !!route.params.id)

// 分类列表
const categories = ref<Category[]>([])

// 表单数据
const form = ref({
  title: '',
  content: '',
  summary: '',
  category: '',
  coverImage: '',
  tags: '',
  isPublished: true,
  isPrivate: false
})

const saving = ref(false)

/**
 * 保存文章
 */
const handleSave = async () => {
  if (!form.value.title || !form.value.content) {
    ElMessage.warning('请填写标题和内容')
    return
  }

  saving.value = true

  try {
    if (isEdit.value) {
      await updateArticle(Number(route.params.id), form.value)
      ElMessage.success('更新成功')
    } else {
      await createArticle(form.value)
      ElMessage.success('创建成功')
    }
    router.push('/admin/articles')
  } catch {
    ElMessage.error('保存失败')
  } finally {
    saving.value = false
  }
}

/**
 * 返回列表
 */
const goBack = () => {
  router.push('/admin/articles')
}

onMounted(async () => {
  // 加载分类
  try {
    const { data } = await getCategories()
    categories.value = data
  } catch {
    // 静默处理
  }

  // 编辑模式：加载文章数据
  if (isEdit.value) {
    try {
      const { data } = await getArticle(Number(route.params.id))
      form.value = {
        title: data.title,
        content: data.content,
        summary: data.summary || '',
        category: data.category || '',
        coverImage: data.coverImage || '',
        tags: data.tags || '',
        isPublished: data.isPublished,
        isPrivate: data.isPrivate
      }
    } catch {
      ElMessage.error('加载文章失败')
    }
  }
})
</script>

<style scoped>
.page-title {
  font-size: 24px;
  margin-bottom: 24px;
  color: var(--text-primary);
}
</style>
