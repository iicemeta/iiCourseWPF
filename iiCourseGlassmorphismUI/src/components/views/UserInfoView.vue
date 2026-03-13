<template>
  <div class="p-6 h-full overflow-auto gradient-bg">
    <!-- 页面标题 -->
    <div class="mb-6">
      <h1 class="text-2xl font-bold text-gradient">个人信息</h1>
      <p class="text-sm text-gray-500 mt-1">查看和管理您的学籍信息</p>
    </div>

    <!-- 用户信息卡片 -->
    <div class="glass rounded-xl p-8 mb-6">
      <div class="flex flex-col md:flex-row items-center gap-6">
        <!-- 头像 -->
        <div class="relative">
          <div class="w-24 h-24 rounded-full bg-gradient-to-br from-primary to-secondary flex items-center justify-center text-3xl font-bold text-white shadow-lg">
            {{ userInitial }}
          </div>
          <button class="absolute -bottom-1 -right-1 btn btn-circle btn-sm btn-primary shadow-md">
            <Icon name="settings" size="xs" />
          </button>
        </div>

        <!-- 基本信息 -->
        <div class="flex-1 text-center md:text-left">
          <h2 class="text-2xl font-bold text-gray-800">{{ userInfo?.name }}</h2>
          <p class="text-gray-500">{{ userInfo?.studentId }}</p>
          <div class="flex flex-wrap justify-center md:justify-start gap-2 mt-3">
            <span class="badge badge-primary">{{ userInfo?.department }}</span>
            <span class="badge badge-secondary">{{ userInfo?.major }}</span>
            <span class="badge badge-accent">{{ userInfo?.className }}</span>
          </div>
        </div>

        <!-- 操作按钮 -->
        <div class="flex gap-2">
          <button class="btn btn-ghost btn-sm text-gray-600 hover:bg-gray-100" @click="refreshData">
            <Icon name="loader" size="sm" />
            刷新
          </button>
          <button class="btn btn-primary btn-sm" @click="editInfo">
            <Icon name="settings" size="sm" class="mr-1" />
            编辑
          </button>
        </div>
      </div>
    </div>

    <!-- 详细信息网格 -->
    <div class="grid grid-cols-1 md:grid-cols-2 gap-6">
      <!-- 学籍信息 -->
      <div class="glass rounded-xl p-6">
        <h3 class="text-lg font-semibold mb-4 flex items-center gap-2 text-gray-800">
          <Icon name="user-circle" size="md" class="text-primary" />
          学籍信息
        </h3>
        <div class="space-y-4">
          <div class="flex justify-between items-center py-2 border-b border-gray-100">
            <span class="text-gray-500">学号</span>
            <span class="font-medium text-gray-800">{{ userInfo?.studentId }}</span>
          </div>
          <div class="flex justify-between items-center py-2 border-b border-gray-100">
            <span class="text-gray-500">姓名</span>
            <span class="font-medium text-gray-800">{{ userInfo?.name }}</span>
          </div>
          <div class="flex justify-between items-center py-2 border-b border-gray-100">
            <span class="text-gray-500">性别</span>
            <span class="font-medium text-gray-800">{{ userInfo?.gender || '未设置' }}</span>
          </div>
          <div class="flex justify-between items-center py-2 border-b border-gray-100">
            <span class="text-gray-500">出生日期</span>
            <span class="font-medium text-gray-800">{{ userInfo?.birthDate || '未设置' }}</span>
          </div>
          <div class="flex justify-between items-center py-2">
            <span class="text-gray-500">身份证号</span>
            <span class="font-medium text-gray-800">{{ maskIdCard(userInfo?.idCard) }}</span>
          </div>
        </div>
      </div>

      <!-- 学院信息 -->
      <div class="glass rounded-xl p-6">
        <h3 class="text-lg font-semibold mb-4 flex items-center gap-2 text-gray-800">
          <Icon name="graduation-cap" size="md" class="text-secondary" />
          学院信息
        </h3>
        <div class="space-y-4">
          <div class="flex justify-between items-center py-2 border-b border-gray-100">
            <span class="text-gray-500">学院</span>
            <span class="font-medium text-gray-800">{{ userInfo?.department }}</span>
          </div>
          <div class="flex justify-between items-center py-2 border-b border-gray-100">
            <span class="text-gray-500">专业</span>
            <span class="font-medium text-gray-800">{{ userInfo?.major }}</span>
          </div>
          <div class="flex justify-between items-center py-2 border-b border-gray-100">
            <span class="text-gray-500">班级</span>
            <span class="font-medium text-gray-800">{{ userInfo?.className }}</span>
          </div>
          <div class="flex justify-between items-center py-2 border-b border-gray-100">
            <span class="text-gray-500">年级</span>
            <span class="font-medium text-gray-800">{{ userInfo?.grade }}</span>
          </div>
          <div class="flex justify-between items-center py-2">
            <span class="text-gray-500">学制</span>
            <span class="font-medium text-gray-800">{{ userInfo?.duration || '4年' }}</span>
          </div>
        </div>
      </div>

      <!-- 联系信息 -->
      <div class="glass rounded-xl p-6">
        <h3 class="text-lg font-semibold mb-4 flex items-center gap-2 text-gray-800">
          <Icon name="bell" size="md" class="text-accent" />
          联系信息
        </h3>
        <div class="space-y-4">
          <div class="flex justify-between items-center py-2 border-b border-gray-100">
            <span class="text-gray-500">手机号码</span>
            <span class="font-medium text-gray-800">{{ userInfo?.phone || '未绑定' }}</span>
          </div>
          <div class="flex justify-between items-center py-2 border-b border-gray-100">
            <span class="text-gray-500">邮箱</span>
            <span class="font-medium text-gray-800">{{ userInfo?.email || '未绑定' }}</span>
          </div>
          <div class="flex justify-between items-center py-2">
            <span class="text-gray-500">宿舍地址</span>
            <span class="font-medium text-gray-800">{{ userInfo?.dormitory || '未设置' }}</span>
          </div>
        </div>
      </div>

      <!-- 学业统计 -->
      <div class="glass rounded-xl p-6">
        <h3 class="text-lg font-semibold mb-4 flex items-center gap-2 text-gray-800">
          <Icon name="award" size="md" class="text-success" />
          学业统计
        </h3>
        <div class="grid grid-cols-2 gap-4">
          <div class="text-center p-4 rounded-lg bg-orange-50">
            <p class="text-2xl font-bold text-primary">{{ userInfo?.gpa || '3.5' }}</p>
            <p class="text-sm text-gray-500">平均绩点</p>
          </div>
          <div class="text-center p-4 rounded-lg bg-orange-50">
            <p class="text-2xl font-bold text-secondary">{{ userInfo?.credits || '120' }}</p>
            <p class="text-sm text-gray-500">已修学分</p>
          </div>
          <div class="text-center p-4 rounded-lg bg-orange-50">
            <p class="text-2xl font-bold text-accent">{{ userInfo?.rank || '15%' }}</p>
            <p class="text-sm text-gray-500">专业排名</p>
          </div>
          <div class="text-center p-4 rounded-lg bg-orange-50">
            <p class="text-2xl font-bold text-success">{{ userInfo?.status || '在读' }}</p>
            <p class="text-sm text-gray-500">学籍状态</p>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { computed, onMounted } from 'vue';
import { useAppStore } from '../../stores/app';
import Icon from '../icons/Icon.vue';

const store = useAppStore();
const { requestData } = store;

const userInfo = computed(() => store.userInfo.value);

const userInitial = computed(() => {
  return userInfo.value?.name?.charAt(0)?.toUpperCase() || '?';
});

const maskIdCard = (idCard: string | undefined): string => {
  if (!idCard) return '未设置';
  if (idCard.length !== 18) return idCard;
  return idCard.slice(0, 6) + '********' + idCard.slice(14);
};

const refreshData = () => {
  requestData('userInfo');
};

const editInfo = () => {
  console.log('编辑信息');
};

onMounted(() => {
  requestData('userInfo');
});
</script>
