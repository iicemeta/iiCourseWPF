<template>
  <div class="flex h-screen overflow-hidden">
    <!-- 侧边栏 -->
    <Sidebar v-if="isLoggedIn" />
    
    <!-- 主内容区 -->
    <main class="flex-1 overflow-auto">
      <Transition name="page" mode="out-in">
        <LoginView v-if="currentView === 'login'" key="login" />
        <ScheduleView v-else-if="currentView === 'schedule'" key="schedule" />
        <ScoreView v-else-if="currentView === 'score'" key="score" />
        <UserInfoView v-else-if="currentView === 'userinfo'" key="userinfo" />
        <EvaluationView v-else-if="currentView === 'evaluation'" key="evaluation" />
        <ClassroomView v-else-if="currentView === 'classroom'" key="classroom" />
        <SettingsView v-else-if="currentView === 'settings'" key="settings" />
        <PrivacyView v-else-if="currentView === 'privacy'" key="privacy" />
      </Transition>
    </main>
    
    <!-- 加载遮罩 -->
    <LoadingOverlay v-if="isLoading" />
    
    <!-- 错误提示 -->
    <ErrorToast v-if="errorMessage" :message="errorMessage" @close="setError(null)" />
  </div>
</template>

<script setup lang="ts">
import { computed } from 'vue';
import { useAppStore } from '../stores/app';

// 组件导入
import Sidebar from './Sidebar.vue';
import LoginView from './views/LoginView.vue';
import ScheduleView from './views/ScheduleView.vue';
import ScoreView from './views/ScoreView.vue';
import UserInfoView from './views/UserInfoView.vue';
import EvaluationView from './views/EvaluationView.vue';
import ClassroomView from './views/ClassroomView.vue';
import SettingsView from './views/SettingsView.vue';
import PrivacyView from './views/PrivacyView.vue';
import LoadingOverlay from './common/LoadingOverlay.vue';
import ErrorToast from './common/ErrorToast.vue';

const store = useAppStore();

const currentView = computed(() => store.currentView.value);
const isLoggedIn = computed(() => store.isLoggedIn.value);
const isLoading = computed(() => store.state.isLoading);
const errorMessage = computed(() => store.state.errorMessage);
const { setError } = store;
</script>

<style scoped>
.page-enter-active,
.page-leave-active {
  transition: all 0.3s cubic-bezier(0.16, 1, 0.3, 1);
}

.page-enter-from {
  opacity: 0;
  transform: translateY(20px);
}

.page-leave-to {
  opacity: 0;
  transform: translateY(-20px);
}
</style>
