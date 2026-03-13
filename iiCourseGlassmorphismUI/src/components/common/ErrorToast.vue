<template>
  <div
    v-if="visible"
    class="fixed bottom-4 right-4 z-50 animate-slide-up"
  >
    <div class="alert alert-error glass-strong shadow-lg border border-red-200">
      <Icon name="alert-circle" size="md" />
      <span class="text-gray-800">{{ message }}</span>
      <button class="btn btn-sm btn-ghost btn-circle text-gray-600 hover:text-gray-800" @click="close">
        <Icon name="x" size="sm" />
      </button>
    </div>
  </div>
</template>

<script setup lang="ts">
import Icon from '../icons/Icon.vue';

defineProps<{
  visible: boolean;
  message: string;
}>();

const emit = defineEmits<{
  close: [];
}>();

const close = () => {
  emit('close');
};
</script>
