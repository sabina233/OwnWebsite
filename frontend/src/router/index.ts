/**
 * 路由配置 - 定义所有页面路由
 * 包含公开页面、私密页面和管理后台
 */
import { createRouter, createWebHistory } from 'vue-router'

const router = createRouter({
  history: createWebHistory(),
  routes: [
    // ========== 公开页面（带导航栏布局）==========
    {
      path: '/',
      component: () => import('@/layouts/MainLayout.vue'),
      children: [
        {
          path: '',
          name: 'Home',
          component: () => import('@/views/public/Home.vue'),
          meta: { title: '首页' }
        },
        {
          path: 'about',
          name: 'About',
          component: () => import('@/views/public/About.vue'),
          meta: { title: '关于我' }
        },
        {
          path: 'blog',
          name: 'Blog',
          component: () => import('@/views/public/Blog.vue'),
          meta: { title: '博客' }
        },
        {
          path: 'article/:id',
          name: 'ArticleDetail',
          component: () => import('@/views/public/ArticleDetail.vue'),
          meta: { title: '文章详情' }
        },
        {
          path: 'projects',
          name: 'Projects',
          component: () => import('@/views/public/Projects.vue'),
          meta: { title: '项目展示' }
        },
        {
          path: 'gallery',
          name: 'Gallery',
          component: () => import('@/views/public/Gallery.vue'),
          meta: { title: '摄影作品' }
        },
        {
          path: 'contact',
          name: 'Contact',
          component: () => import('@/views/public/Contact.vue'),
          meta: { title: '联系方式' }
        }
      ]
    },

    // ========== 私密页面（需要密码验证）==========
    {
      path: '/private',
      component: () => import('@/layouts/MainLayout.vue'),
      children: [
        {
          path: '',
          name: 'PrivateGate',
          component: () => import('@/views/private/PrivateGate.vue'),
          meta: { title: '秘密空间' }
        },
        {
          path: 'articles',
          name: 'PrivateArticles',
          component: () => import('@/views/private/PrivateArticles.vue'),
          meta: { title: '私密文章', requiresAuth: true }
        },
        {
          path: 'articles/:id',
          name: 'PrivateArticleDetail',
          component: () => import('@/views/private/PrivateArticleDetail.vue'),
          meta: { title: '文章详情', requiresAuth: true }
        },
        {
          path: 'diary',
          name: 'Diary',
          component: () => import('@/views/private/Diary.vue'),
          meta: { title: '我的日记', requiresAuth: true }
        },
        {
          path: 'diary/:id',
          name: 'DiaryDetail',
          component: () => import('@/views/private/DiaryDetail.vue'),
          meta: { title: '日记详情', requiresAuth: true }
        }
      ]
    },

    // ========== 管理后台（独立布局）==========
    {
      path: '/admin',
      component: () => import('@/layouts/AdminLayout.vue'),
      meta: { requiresAdmin: true },
      children: [
        {
          path: '',
          name: 'AdminDashboard',
          component: () => import('@/views/admin/Dashboard.vue'),
          meta: { title: '管理后台' }
        },
        {
          path: 'articles',
          name: 'AdminArticles',
          component: () => import('@/views/admin/ArticleList.vue'),
          meta: { title: '文章管理' }
        },
        {
          path: 'articles/edit/:id?',
          name: 'AdminArticleEdit',
          component: () => import('@/views/admin/ArticleEditor.vue'),
          meta: { title: '编辑文章' }
        },
        {
          path: 'diary',
          name: 'AdminDiary',
          component: () => import('@/views/admin/DiaryEditor.vue'),
          meta: { title: '日记管理' }
        },
        {
          path: 'projects',
          name: 'AdminProjects',
          component: () => import('@/views/admin/ProjectEditor.vue'),
          meta: { title: '项目管理' }
        },
        {
          path: 'photos',
          name: 'AdminPhotos',
          component: () => import('@/views/admin/PhotoUpload.vue'),
          meta: { title: '照片管理' }
        },
        {
          path: 'settings',
          name: 'AdminSettings',
          component: () => import('@/views/admin/Settings.vue'),
          meta: { title: '网站设置' }
        }
      ]
    },

    // 管理员登录页
    {
      path: '/admin/login',
      name: 'AdminLogin',
      component: () => import('@/views/admin/Login.vue'),
      meta: { title: '管理员登录' }
    }
  ]
})

// 路由守卫：处理页面标题和权限验证
router.beforeEach((to, _from, next) => {
  // 设置页面标题
  const title = to.meta.title as string
  document.title = title ? `${title} - 我的个人网站` : '我的个人网站'

  // 验证私密页面权限
  if (to.meta.requiresAuth) {
    const privateToken = localStorage.getItem('private_token')
    if (!privateToken) {
      next({ name: 'PrivateGate' })
      return
    }
  }

  // 验证管理员权限
  if (to.meta.requiresAdmin || to.matched.some(record => record.meta.requiresAdmin)) {
    const adminToken = localStorage.getItem('admin_token')
    if (!adminToken) {
      next({ name: 'AdminLogin' })
      return
    }
  }

  next()
})

export default router
