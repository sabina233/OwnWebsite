<template>
  <!-- 应用根组件 - 根据路由渲染不同页面 -->
  <router-view v-slot="{ Component }">
    <Transition name="fade" mode="out-in">
      <component :is="Component" :key="$route.fullPath" />
    </Transition>
  </router-view>
</template>

<script setup lang="ts">
/**
 * 应用根组件
 * 使用 key 强制路由切换时重新渲染组件
 */
</script>

<style>
/* 全局样式已在 main.css 中定义 */
</style>
