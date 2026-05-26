import { defineConfig } from 'vite'
import vue from '@vitejs/plugin-vue'
import { fileURLToPath, URL } from 'node:url'

// Vite配置文件
export default defineConfig({
  plugins: [vue()],
  resolve: {
    alias: {
      '@': fileURLToPath(new URL('./src', import.meta.url))
    }
  },
  server: {
    port: 5173,  // 开发服务器端口
    proxy: {
      // 代理后端API请求
      '/api': {
        target: 'http://localhost:5116',
        changeOrigin: true
      },
      // 代理静态文件请求（图片等）
      '/uploads': {
        target: 'http://localhost:5116',
        changeOrigin: true
      }
    }
  }
})
