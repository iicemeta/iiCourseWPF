<template>
  <div class="min-h-screen flex items-center justify-center p-4 gradient-bg">
    <!-- 装饰背景 -->
    <div class="fixed inset-0 overflow-hidden pointer-events-none">
      <div class="absolute -top-40 -left-40 w-96 h-96 bg-orange-300/30 rounded-full blur-3xl animate-float"></div>
      <div class="absolute -bottom-40 -right-40 w-80 h-80 bg-amber-300/30 rounded-full blur-3xl animate-float" style="animation-delay: 1s;"></div>
      <div class="absolute top-1/2 left-1/2 -translate-x-1/2 -translate-y-1/2 w-[600px] h-[600px] bg-orange-100/50 rounded-full blur-3xl"></div>
    </div>

    <!-- 登录卡片 -->
    <div class="glass-strong rounded-2xl p-8 w-full max-w-md relative z-10 animate-slide-up">
      <!-- Logo -->
      <div class="text-center mb-8">
        <div class="w-20 h-20 mx-auto rounded-2xl gradient-orange flex items-center justify-center mb-4 shadow-lg animate-pulse-glow">
          <Icon name="graduation-cap" size="xl" class="text-white" />
        </div>
        <h1 class="text-2xl font-bold text-gradient mb-2">iiCourse</h1>
        <p class="text-gray-500 text-sm">教务助手，让学习更简单</p>
      </div>

      <!-- 登录表单 -->
      <form @submit.prevent="handleLogin" class="space-y-5">
        <!-- 学号输入 -->
        <div class="form-control">
          <label class="label">
            <span class="label-text text-sm font-medium text-gray-700">学号</span>
          </label>
          <div class="relative">
            <span class="absolute left-4 top-1/2 -translate-y-1/2 text-gray-400">
              <Icon name="user" size="sm" />
            </span>
            <input
              v-model="username"
              type="text"
              placeholder="请输入学号"
              class="input input-bordered w-full pl-11 bg-white/80 border-gray-200 focus:border-primary text-gray-800 placeholder-gray-400"
              required
            />
          </div>
        </div>

        <!-- 密码输入 -->
        <div class="form-control">
          <label class="label">
            <span class="label-text text-sm font-medium text-gray-700">密码</span>
          </label>
          <div class="relative">
            <span class="absolute left-4 top-1/2 -translate-y-1/2 text-gray-400">
              <Icon name="lock" size="sm" />
            </span>
            <input
              v-model="password"
              :type="showPassword ? 'text' : 'password'"
              placeholder="请输入密码"
              class="input input-bordered w-full pl-11 pr-11 bg-white/80 border-gray-200 focus:border-primary text-gray-800 placeholder-gray-400"
              required
            />
            <button
              type="button"
              class="absolute right-4 top-1/2 -translate-y-1/2 btn btn-ghost btn-xs btn-circle text-gray-400 hover:text-gray-600"
              @click="showPassword = !showPassword"
            >
              <Icon :name="showPassword ? 'eye-off' : 'eye'" size="sm" />
            </button>
          </div>
        </div>

        <!-- 记住我 -->
        <div class="flex items-center justify-between">
          <label class="label cursor-pointer gap-2">
            <input type="checkbox" class="checkbox checkbox-sm checkbox-primary" v-model="rememberMe" />
            <span class="label-text text-sm text-gray-600">记住我</span>
          </label>
          <a class="text-sm text-primary hover:underline cursor-pointer">忘记密码？</a>
        </div>

        <!-- 登录按钮 -->
        <button
          type="submit"
          class="btn btn-primary w-full btn-hover-lift"
          :disabled="isLoading"
        >
          <span v-if="isLoading" class="loading loading-spinner loading-sm"></span>
          <span v-else>登录</span>
        </button>
      </form>

      <!-- 底部链接 -->
      <div class="mt-6 text-center space-y-2">
        <p class="text-xs text-gray-400">
          登录即表示同意
          <a class="text-primary hover:underline cursor-pointer" @click="navigate('privacy')">隐私政策</a>
        </p>
        <p class="text-xs text-gray-300">v1.7.200</p>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref } from 'vue';
import { useAppStore } from '../../stores/app';
import Icon from '../icons/Icon.vue';

const store = useAppStore();
const username = ref('');
const password = ref('');
const rememberMe = ref(false);
const showPassword = ref(false);
const isLoading = ref(false);

const handleLogin = async () => {
  isLoading.value = true;
  store.login(username.value, password.value);
  // 模拟登录延迟
  setTimeout(() => {
    isLoading.value = false;
  }, 1000);
};

const navigate = (view: string) => {
  store.navigate(view);
};
</script>
