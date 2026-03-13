<template>
  <div class="p-6 h-full overflow-auto gradient-bg">
    <!-- 页面标题 -->
    <div class="mb-6">
      <h1 class="text-2xl font-bold text-gradient">设置</h1>
      <p class="text-sm text-gray-500 mt-1">自定义您的应用体验</p>
    </div>

    <div class="grid grid-cols-1 lg:grid-cols-2 gap-6">
      <!-- 外观设置 -->
      <div class="glass rounded-xl p-6">
        <h3 class="text-lg font-semibold mb-4 flex items-center gap-2 text-gray-800">
          <Icon name="palette" size="md" class="text-primary" />
          外观
        </h3>
        <div class="space-y-4">
          <div class="flex items-center justify-between">
            <div>
              <p class="font-medium text-gray-800">深色模式</p>
              <p class="text-sm text-gray-500">使用深色主题</p>
            </div>
            <input type="checkbox" class="toggle toggle-primary" v-model="settings.darkMode" />
          </div>
          <div class="flex items-center justify-between">
            <div>
              <p class="font-medium text-gray-800">玻璃态效果</p>
              <p class="text-sm text-gray-500">启用毛玻璃视觉效果</p>
            </div>
            <input type="checkbox" class="toggle toggle-primary" v-model="settings.glassEffect" />
          </div>
          <div class="flex items-center justify-between">
            <div>
              <p class="font-medium text-gray-800">动画效果</p>
              <p class="text-sm text-gray-500">启用界面动画</p>
            </div>
            <input type="checkbox" class="toggle toggle-primary" v-model="settings.animations" />
          </div>
        </div>
      </div>

      <!-- 通知设置 -->
      <div class="glass rounded-xl p-6">
        <h3 class="text-lg font-semibold mb-4 flex items-center gap-2 text-gray-800">
          <Icon name="bell" size="md" class="text-secondary" />
          通知
        </h3>
        <div class="space-y-4">
          <div class="flex items-center justify-between">
            <div>
              <p class="font-medium text-gray-800">课程提醒</p>
              <p class="text-sm text-gray-500">上课前15分钟提醒</p>
            </div>
            <input type="checkbox" class="toggle toggle-primary" v-model="settings.courseReminder" />
          </div>
          <div class="flex items-center justify-between">
            <div>
              <p class="font-medium text-gray-800">成绩通知</p>
              <p class="text-sm text-gray-500">新成绩发布时通知</p>
            </div>
            <input type="checkbox" class="toggle toggle-primary" v-model="settings.scoreNotification" />
          </div>
          <div class="flex items-center justify-between">
            <div>
              <p class="font-medium text-gray-800">系统通知</p>
              <p class="text-sm text-gray-500">接收系统更新和公告</p>
            </div>
            <input type="checkbox" class="toggle toggle-primary" v-model="settings.systemNotification" />
          </div>
        </div>
      </div>

      <!-- 数据设置 -->
      <div class="glass rounded-xl p-6">
        <h3 class="text-lg font-semibold mb-4 flex items-center gap-2 text-gray-800">
          <Icon name="database" size="md" class="text-accent" />
          数据
        </h3>
        <div class="space-y-4">
          <div class="flex items-center justify-between">
            <div>
              <p class="font-medium text-gray-800">自动同步</p>
              <p class="text-sm text-gray-500">启动时自动同步数据</p>
            </div>
            <input type="checkbox" class="toggle toggle-primary" v-model="settings.autoSync" />
          </div>
          <div class="flex items-center justify-between">
            <div>
              <p class="font-medium text-gray-800">离线模式</p>
              <p class="text-sm text-gray-500">优先使用本地缓存</p>
            </div>
            <input type="checkbox" class="toggle toggle-primary" v-model="settings.offlineMode" />
          </div>
          <div class="pt-2">
            <button class="btn btn-sm btn-error btn-outline w-full" @click="clearCache">
              清除缓存数据
            </button>
          </div>
        </div>
      </div>

      <!-- 关于 -->
      <div class="glass rounded-xl p-6">
        <h3 class="text-lg font-semibold mb-4 flex items-center gap-2 text-gray-800">
          <Icon name="info" size="md" class="text-success" />
          关于
        </h3>
        <div class="space-y-4">
          <div class="flex items-center justify-between">
            <span class="text-gray-500">版本</span>
            <span class="font-medium text-gray-800">v1.7.200</span>
          </div>
          <div class="flex items-center justify-between">
            <span class="text-gray-500">构建日期</span>
            <span class="font-medium text-gray-800">2024-12-13</span>
          </div>
          <div class="flex items-center justify-between">
            <span class="text-gray-500">开发者</span>
            <span class="font-medium text-gray-800">iiCourse Team</span>
          </div>
          <div class="pt-2 space-y-2">
            <button class="btn btn-sm btn-ghost w-full text-gray-600 hover:bg-gray-100" @click="checkUpdate">
              检查更新
            </button>
            <button class="btn btn-sm btn-ghost w-full text-gray-600 hover:bg-gray-100" @click="viewPrivacy">
              隐私政策
            </button>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { reactive } from 'vue';
import { useAppStore } from '../../stores/app';
import Icon from '../icons/Icon.vue';

const store = useAppStore();

const settings = reactive({
  darkMode: false,
  glassEffect: true,
  animations: true,
  courseReminder: true,
  scoreNotification: true,
  systemNotification: true,
  autoSync: true,
  offlineMode: false,
});

const clearCache = () => {
  console.log('清除缓存');
};

const checkUpdate = () => {
  console.log('检查更新');
};

const viewPrivacy = () => {
  store.navigate('privacy');
};
</script>
