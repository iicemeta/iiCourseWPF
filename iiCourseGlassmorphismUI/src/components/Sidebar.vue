<template>
  <aside
    class="glass-strong h-full flex flex-col transition-all duration-300 border-r border-white/50"
    :class="{ 'w-64': !collapsed, 'w-16': collapsed }"
  >
    <!-- Logo 区域 -->
    <div class="p-4 flex items-center gap-3 border-b border-gray-200/50">
      <div class="w-10 h-10 rounded-xl gradient-orange flex items-center justify-center flex-shrink-0 shadow-md">
        <Icon name="graduation-cap" size="lg" class="text-white" />
      </div>
      <span v-if="!collapsed" class="font-bold text-lg text-gradient">iiCourse</span>
      <button
        v-if="!collapsed"
        class="ml-auto btn btn-ghost btn-sm btn-circle text-gray-500 hover:text-gray-700"
        @click="toggleSidebar"
      >
        <Icon name="chevron-left" size="sm" />
      </button>
      <button
        v-else
        class="btn btn-ghost btn-sm btn-circle text-gray-500 hover:text-gray-700"
        @click="toggleSidebar"
      >
        <Icon name="menu" size="sm" />
      </button>
    </div>

    <!-- 用户信息 -->
    <div v-if="isLoggedIn && !collapsed" class="p-4 border-b border-gray-200/50">
      <div class="flex items-center gap-3">
        <div class="w-12 h-12 rounded-full bg-gradient-to-br from-primary to-secondary flex items-center justify-center shadow-md">
          <span class="text-lg font-bold text-white">{{ userInitial }}</span>
        </div>
        <div class="flex-1 min-w-0">
          <p class="font-medium truncate text-gray-800">{{ userInfo?.name || '未登录' }}</p>
          <p class="text-xs text-gray-500 truncate">{{ userInfo?.studentId || '' }}</p>
        </div>
      </div>
    </div>

    <!-- 导航菜单 -->
    <nav class="flex-1 overflow-y-auto py-4">
      <ul class="menu menu-sm gap-1 px-2">
        <li v-for="item in menuItems" :key="item.id">
          <a
            class="flex items-center gap-3 rounded-lg transition-all duration-200"
            :class="{
              'bg-primary/10 text-primary': currentView === item.id,
              'hover:bg-gray-100 text-gray-700': currentView !== item.id
            }"
            @click="navigate(item.id)"
          >
            <Icon :name="item.icon" size="md" />
            <span v-if="!collapsed">{{ item.label }}</span>
          </a>
        </li>
      </ul>
    </nav>

    <!-- 底部操作 -->
    <div class="p-4 border-t border-gray-200/50">
      <ul class="menu menu-sm gap-1">
        <li>
          <a
            class="flex items-center gap-3 rounded-lg hover:bg-gray-100 transition-all text-gray-700"
            :class="{ 'bg-primary/10 text-primary': currentView === 'settings' }"
            @click="navigate('settings')"
          >
            <Icon name="settings" size="md" />
            <span v-if="!collapsed">设置</span>
          </a>
        </li>
        <li>
          <a
            class="flex items-center gap-3 rounded-lg hover:bg-red-50 hover:text-red-600 transition-all text-gray-700"
            @click="logout"
          >
            <Icon name="door-open" size="md" />
            <span v-if="!collapsed">退出</span>
          </a>
        </li>
      </ul>
    </div>
  </aside>
</template>

<script setup lang="ts">
import { computed } from 'vue';
import { useAppStore } from '../stores/app';
import Icon from './icons/Icon.vue';

const store = useAppStore();

const currentView = computed(() => store.currentView.value);
const isLoggedIn = computed(() => store.isLoggedIn.value);
const userInfo = computed(() => store.userInfo.value);
const collapsed = computed(() => store.state.sidebarCollapsed);

const userInitial = computed(() => {
  return userInfo.value?.name?.charAt(0)?.toUpperCase() || '?';
});

const menuItems = [
  { id: 'schedule', label: '课表查询', icon: 'calendar' },
  { id: 'score', label: '成绩查询', icon: 'award' },
  { id: 'userinfo', label: '个人信息', icon: 'user-circle' },
  { id: 'evaluation', label: '教学评价', icon: 'star' },
  { id: 'classroom', label: '空教室', icon: 'door-open' },
];

const { navigate, logout, toggleSidebar } = store;
</script>
