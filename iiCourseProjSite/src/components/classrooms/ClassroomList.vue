<template>
  <div class="space-y-6">
    <!-- 筛选器 -->
    <div class="bg-white rounded-2xl shadow-lg p-6">
      <div class="grid md:grid-cols-3 gap-4">
        <div>
          <label class="block text-sm font-medium text-gray-700 mb-2">教学楼</label>
          <select v-model="selectedBuilding" class="w-full px-4 py-2 border border-gray-200 rounded-lg">
            <option v-for="building in buildings" :key="building.id" :value="building.id">
              {{ building.name }}
            </option>
          </select>
        </div>
        <div>
          <label class="block text-sm font-medium text-gray-700 mb-2">日期</label>
          <input type="date" v-model="selectedDate" class="w-full px-4 py-2 border border-gray-200 rounded-lg" />
        </div>
        <div>
          <label class="block text-sm font-medium text-gray-700 mb-2">节次</label>
          <select v-model="selectedPeriod" class="w-full px-4 py-2 border border-gray-200 rounded-lg">
            <option value="all">全部节次</option>
            <option v-for="period in periods" :key="period" :value="period">
              第 {{ period }} 节
            </option>
          </select>
        </div>
      </div>
    </div>
    
    <!-- 结果列表 -->
    <div class="bg-white rounded-2xl shadow-lg overflow-hidden">
      <div class="p-4 border-b border-gray-100 flex items-center justify-between">
        <h3 class="font-semibold text-gray-900">空教室列表</h3>
        <span class="text-sm text-gray-500">共 {{ filteredClassrooms.length }} 间</span>
      </div>
      
      <div class="divide-y divide-gray-100">
        <div 
          v-for="room in filteredClassrooms" 
          :key="room.name"
          class="p-4 hover:bg-gray-50 transition-colors flex items-center justify-between"
        >
          <div class="flex items-center space-x-4">
            <div class="w-12 h-12 bg-green-100 rounded-xl flex items-center justify-center text-2xl">
              🏫
            </div>
            <div>
              <div class="font-semibold text-gray-900">{{ room.name }}</div>
              <div class="text-sm text-gray-500">{{ room.building }}</div>
            </div>
          </div>
          <div class="flex items-center space-x-2">
            <span class="px-3 py-1 bg-green-100 text-green-700 rounded-full text-sm">
              空闲
            </span>
            <span class="text-sm text-gray-500">{{ room.period }}节</span>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, computed } from 'vue';

const selectedBuilding = ref('all');
const selectedDate = ref('2025-03-12');
const selectedPeriod = ref('all');

const buildings = [
  { id: 'all', name: '全部教学楼' },
  { id: 'A', name: 'A座教学楼' },
  { id: 'B', name: 'B座教学楼' },
  { id: 'C', name: 'C座教学楼' },
  { id: 'D', name: '实验楼' },
];

const periods = [1, 2, 3, 4, 5, 6, 7, 8];

const classrooms = [
  { name: 'A101', building: 'A座教学楼', period: '1-2' },
  { name: 'A102', building: 'A座教学楼', period: '3-4' },
  { name: 'A201', building: 'A座教学楼', period: '5-6' },
  { name: 'B101', building: 'B座教学楼', period: '1-2' },
  { name: 'B102', building: 'B座教学楼', period: '3-4' },
  { name: 'B201', building: 'B座教学楼', period: '7-8' },
  { name: 'C101', building: 'C座教学楼', period: '1-2' },
  { name: 'C102', building: 'C座教学楼', period: '5-6' },
  { name: 'D301', building: '实验楼', period: '3-4' },
  { name: 'D302', building: '实验楼', period: '7-8' },
];

const filteredClassrooms = computed(() => {
  return classrooms;
});
</script>
