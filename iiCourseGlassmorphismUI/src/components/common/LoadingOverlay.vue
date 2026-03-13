<template>
  <div v-if="visible" class="fixed inset-0 z-50 flex items-center justify-center bg-white/60 backdrop-blur-sm">
    <div class="glass-strong rounded-2xl p-8 flex flex-col items-center gap-4 animate-pulse border border-white/80">
      <span class="loading loading-spinner loading-lg text-primary"></span>
      <p class="text-lg font-medium text-gray-700">{{ message }}</p>
    </div>
  </div>
</template>

<script setup lang="ts">
defineProps<{
  visible: boolean;
  message?: string;
}>();
</script>
