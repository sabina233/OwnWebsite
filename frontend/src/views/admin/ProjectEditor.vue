<template>
  <!-- 项目管理页面 -->
  <div class="project-editor">
    <div class="page-header">
      <h1 class="page-title">项目管理</h1>
      <el-button type="primary" @click="showEditor = true">
        <el-icon><Plus /></el-icon>
        新建项目
      </el-button>
    </div>

    <!-- 项目列表 -->
    <el-table :data="projects" style="width: 100%">
      <el-table-column prop="title" label="项目名称" />
      <el-table-column prop="techStack" label="技术栈" />
      <el-table-column label="置顶" width="80">
        <template #default="{ row }">
          <el-tag v-if="row.isFeatured" type="success">是</el-tag>
        </template>
      </el-table-column>
      <el-table-column label="操作" width="200">
        <template #default="{ row }">
          <el-button size="small" @click="editProject(row)">编辑</el-button>
          <el-button size="small" type="danger" @click="handleDelete(row.id)">删除</el-button>
        </template>
      </el-table-column>
    </el-table>

    <!-- 编辑对话框 -->
    <el-dialog v-model="showEditor" :title="editingId ? '编辑项目' : '新建项目'" width="60%">
      <el-form :model="form" label-position="top">
        <el-form-item label="项目名称">
          <el-input v-model="form.title" placeholder="项目名称" />
        </el-form-item>
        <el-form-item label="项目描述">
          <el-input v-model="form.description" type="textarea" :rows="4" placeholder="项目描述" />
        </el-form-item>
        <el-form-item label="技术栈">
          <el-input v-model="form.techStack" placeholder="技术栈（用逗号分隔）" />
        </el-form-item>
        <el-form-item label="GitHub链接">
          <el-input v-model="form.gitHubUrl" placeholder="GitHub仓库地址" />
        </el-form-item>
        <el-form-item label="演示链接">
          <el-input v-model="form.demoUrl" placeholder="在线演示地址" />
        </el-form-item>
        <el-form-item label="封面图片">
          <el-input v-model="form.coverImage" placeholder="封面图片URL" />
        </el-form-item>
        <el-form-item>
          <el-checkbox v-model="form.isFeatured">置顶展示</el-checkbox>
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
 * 项目管理页面
 * 支持创建、编辑、删除项目
 */
import { ref, onMounted } from 'vue'
import { Plus } from '@element-plus/icons-vue'
import { ElMessageBox, ElMessage } from 'element-plus'
import { getProjects, createProject, updateProject, deleteProject } from '@/api'
import type { Project } from '@/types'

// 项目列表
const projects = ref<Project[]>([])

// 编辑器状态
const showEditor = ref(false)
const editingId = ref<number | null>(null)
const saving = ref(false)

// 表单数据
const form = ref({
  title: '',
  description: '',
  techStack: '',
  gitHubUrl: '',
  demoUrl: '',
  coverImage: '',
  isFeatured: false
})

/**
 * 编辑项目
 */
const editProject = (project: Project) => {
  editingId.value = project.id
  form.value = {
    title: project.title,
    description: project.description,
    techStack: project.techStack || '',
    gitHubUrl: project.gitHubUrl || '',
    demoUrl: project.demoUrl || '',
    coverImage: project.coverImage || '',
    isFeatured: project.isFeatured
  }
  showEditor.value = true
}

/**
 * 保存项目
 */
const handleSave = async () => {
  if (!form.value.title || !form.value.description) {
    ElMessage.warning('请填写项目名称和描述')
    return
  }

  saving.value = true

  try {
    if (editingId.value) {
      await updateProject(editingId.value, form.value)
      ElMessage.success('更新成功')
    } else {
      await createProject(form.value)
      ElMessage.success('创建成功')
    }
    showEditor.value = false
    editingId.value = null
    form.value = { title: '', description: '', techStack: '', gitHubUrl: '', demoUrl: '', coverImage: '', isFeatured: false }
    fetchProjects()
  } catch {
    ElMessage.error('保存失败')
  } finally {
    saving.value = false
  }
}

/**
 * 删除项目
 */
const handleDelete = async (id: number) => {
  try {
    await ElMessageBox.confirm('确定要删除这个项目吗？', '确认删除')
    await deleteProject(id)
    ElMessage.success('删除成功')
    fetchProjects()
  } catch {
    // 取消操作
  }
}

/**
 * 获取项目列表
 */
const fetchProjects = async () => {
  try {
    const { data } = await getProjects()
    projects.value = data
  } catch {
    // 静默处理
  }
}

onMounted(fetchProjects)
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
