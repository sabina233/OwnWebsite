import { defineConfig } from 'vite'
import vue from '@vitejs/plugin-vue'

// Vite配置文件
export default defineConfig({
  plugins: [vue()],
  server: {
    port: 5173,  // 开发服务器端口
    proxy: {
      // 代理后端API请求
      '/api': {
        target: 'http://localhost:5000',
        changeOrigin: true
      }
    }
  }
})
