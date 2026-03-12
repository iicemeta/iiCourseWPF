<template>
  <div class="space-y-6">
    <!-- 筛选器 -->
    <div class="card bg-base-100 shadow-lg">
      <div class="card-body">
        <div class="grid md:grid-cols-3 gap-4">
          <div class="form-control">
            <label class="label">
              <span class="label-text">教学楼</span>
            </label>
            <select v-model="selectedBuilding" class="select select-bordered">
              <option v-for="building in buildings" :key="building.id" :value="building.id">
                {{ building.name }}
              </option>
            </select>
          </div>
          <div class="form-control">
            <label class="label">
              <span class="label-text">日期</span>
            </label>
            <input type="date" v-model="selectedDate" class="input input-bordered" />
          </div>
          <div class="form-control">
            <label class="label">
              <span class="label-text">节次</span>
            </label>
            <select v-model="selectedPeriod" class="select select-bordered">
              <option value="all">全部节次</option>
              <option v-for="period in periods" :key="period" :value="period">
                第 {{ period }} 节
              </option>
            </select>
          </div>
        </div>
      </div>
    </div>
    
    <!-- 结果列表 -->
    <div class="card bg-base-100 shadow-lg overflow-hidden">
      <div class="card-body py-4 border-b border-base-300 flex items-center justify-between">
        <h3 class="card-title text-base">空教室列表</h3>
        <span class="badge badge-ghost">共 {{ filteredClassrooms.length }} 间</span>
      </div>
      
      <div class="divide-y divide-base-300">
        <div 
          v-for="room in filteredClassrooms" 
          :key="room.name"
          class="p-4 hover:bg-base-200 transition-colors flex items-center justify-between"
        >
          <div class="flex items-center gap-4">
            <div class="w-12 h-12 bg-green-100 rounded-xl flex items-center justify-center text-2xl">
              🏫
            </div>
            <div>
              <div class="font-semibold text-base-content">{{ room.name }}</div>
              <div class="text-sm text-base-content/50">{{ room.building }}</div>
            </div>
          </div>
          <div class="flex items-center gap-2">
            <span class="badge badge-success">空闲</span>
            <span class="text-sm text-base-content/50">{{ room.period }}节</span>
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
