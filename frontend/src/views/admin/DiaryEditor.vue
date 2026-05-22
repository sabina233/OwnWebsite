<template>
  <!-- 日记管理页面 -->
  <div class="diary-editor">
    <div class="page-header">
      <h1 class="page-title">日记管理</h1>
      <el-button type="primary" @click="showEditor = true">
        <el-icon><Plus /></el-icon>
        写日记
      </el-button>
    </div>

    <!-- 日记列表 -->
    <el-table :data="diaries" style="width: 100%">
      <el-table-column prop="title" label="标题" />
      <el-table-column prop="mood" label="心情" width="100" />
      <el-table-column prop="weather" label="天气" width="100" />
      <el-table-column label="日期" width="180">
        <template #default="{ row }">
          {{ formatDate(row.createdAt) }}
        </template>
      </el-table-column>
      <el-table-column label="操作" width="200">
        <template #default="{ row }">
          <el-button size="small" @click="editDiary(row)">编辑</el-button>
          <el-button size="small" type="danger" @click="handleDelete(row.id)">删除</el-button>
        </template>
      </el-table-column>
    </el-table>

    <!-- 编辑对话框 -->
    <el-dialog v-model="showEditor" :title="editingId ? '编辑日记' : '写日记'" width="80%">
      <el-form :model="form" label-position="top">
        <el-form-item label="标题">
          <el-input v-model="form.title" placeholder="日记标题" />
        </el-form-item>
        <el-form-item label="心情">
          <el-input v-model="form.mood" placeholder="今天的心情" />
        </el-form-item>
        <el-form-item label="天气">
          <el-input v-model="form.weather" placeholder="今天的天气" />
        </el-form-item>
        <el-form-item label="内容">
          <MdEditor v-model="form.content" style="height: 400px" />
        </el-form-item>
      </el-form>
      <template #footer>
        <el-button @click="showEditor = false">取消</el-button>
        <el-button type="primary" @click="handleSave" :loading="saving">保存</el-button>
      </template>
    </el-dialog>
  </div>
</template>

<script setup lang="ts">
/**
 * 日记管理页面
 * 支持创建、编辑、删除日记
 */
import { ref, onMounted } from 'vue'
import { Plus } from '@element-plus/icons-vue'
import { ElMessageBox, ElMessage } from 'element-plus'
import { MdEditor } from 'md-editor-v3'
import 'md-editor-v3/lib/style.css'
import { getDiaries, createDiary, updateDiary, deleteDiary } from '@/api'
import type { DiaryListItem } from '@/types'

// 日记列表
const diaries = ref<DiaryListItem[]>([])

// 编辑器状态
const showEditor = ref(false)
const editingId = ref<number | null>(null)
const saving = ref(false)

// 表单数据
const form = ref({
  title: '',
  content: '',
  mood: '',
  weather: ''
})

/**
 * 格式化日期
 */
const formatDate = (dateStr: string) => {
  return new Date(dateStr).toLocaleDateString('zh-CN')
}

/**
 * 编辑日记
 */
const editDiary = (diary: DiaryListItem) => {
  editingId.value = diary.id
  form.value = {
    title: diary.title,
    content: '', // 需要重新获取完整内容
    mood: diary.mood || '',
    weather: diary.weather || ''
  }
  showEditor.value = true
}

/**
 * 保存日记
 */
const handleSave = async () => {
  if (!form.value.title || !form.value.content) {
    ElMessage.warning('请填写标题和内容')
    return
  }

  saving.value = true

  try {
    if (editingId.value) {
      await updateDiary(editingId.value, form.value)
      ElMessage.success('更新成功')
    } else {
      await createDiary(form.value)
      ElMessage.success('保存成功')
    }
    showEditor.value = false
    editingId.value = null
    form.value = { title: '', content: '', mood: '', weather: '' }
    fetchDiaries()
  } catch {
    ElMessage.error('保存失败')
  } finally {
    saving.value = false
  }
}

/**
 * 删除日记
 */
const handleDelete = async (id: number) => {
  try {
    await ElMessageBox.confirm('确定要删除这篇日记吗？', '确认删除')
    await deleteDiary(id)
    ElMessage.success('删除成功')
    fetchDiaries()
  } catch {
    // 取消操作
  }
}

/**
 * 获取日记列表
 */
const fetchDiaries = async () => {
  try {
    const { data } = await getDiaries({ pageSize: 100 })
    diaries.value = data.items
  } catch {
    // 静默处理
  }
}

onMounted(fetchDiaries)
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
