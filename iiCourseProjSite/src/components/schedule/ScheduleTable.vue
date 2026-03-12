<template>
  <div class="card bg-base-100 shadow-xl overflow-hidden">
    <!-- 工具栏 -->
    <div class="card-body py-4 border-b border-base-300 flex flex-wrap items-center justify-between gap-4">
      <div class="flex items-center gap-4">
        <select v-model="selectedWeek" class="select select-bordered select-sm">
          <option v-for="week in 20" :key="week" :value="week">第 {{ week }} 周</option>
        </select>
        <span class="text-base-content/50 text-sm">2024-2025学年 第2学期</span>
      </div>
      <div class="text-sm text-base-content/50">
        {{ dateRange }}
      </div>
    </div>
    
    <!-- 课程表 -->
    <div class="overflow-x-auto">
      <table class="table table-zebra w-full min-w-[800px]">
        <thead>
          <tr class="bg-gradient-to-r from-primary to-secondary text-white">
            <th class="py-4 px-2 w-20">节次</th>
            <th v-for="day in weekdays" :key="day.key" class="py-4 px-2">
              <div>{{ day.name }}</div>
              <div class="text-xs opacity-80">{{ day.date }}</div>
            </th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="period in periods" :key="period.num" class="border-b border-base-300">
            <td class="py-4 px-2 text-center bg-base-200">
              <div class="font-semibold text-base-content">{{ period.num }}</div>
              <div class="text-xs text-base-content/50">{{ period.time }}</div>
            </td>
            <td 
              v-for="day in weekdays" 
              :key="day.key"
              class="py-2 px-1 min-h-[80px]"
            >
              <div 
                v-if="getClass(day.key, period.num)"
                :class="['rounded-lg p-3 text-sm h-full', getClass(day.key, period.num)?.color]"
              >
                <div class="font-semibold text-base-content">{{ getClass(day.key, period.num)?.name }}</div>
                <div class="text-base-content/70 text-xs mt-1">{{ getClass(day.key, period.num)?.room }}</div>
                <div class="text-base-content/50 text-xs">{{ getClass(day.key, period.num)?.teacher }}</div>
              </div>
            </td>
          </tr>
        </tbody>
      </table>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, computed } from 'vue';

const selectedWeek = ref(3);

const weekdays = [
  { key: 1, name: '周一', date: '3/10' },
  { key: 2, name: '周二', date: '3/11' },
  { key: 3, name: '周三', date: '3/12' },
  { key: 4, name: '周四', date: '3/13' },
  { key: 5, name: '周五', date: '3/14' },
];

const periods = [
  { num: 1, time: '08:00' },
  { num: 2, time: '09:00' },
  { num: 3, time: '10:00' },
  { num: 4, time: '11:00' },
  { num: 5, time: '14:00' },
  { num: 6, time: '15:00' },
  { num: 7, time: '16:00' },
  { num: 8, time: '17:00' },
];

// 示例课程数据
const classes = [
  { weekday: 1, period: 1, name: '高等数学', room: 'A101', teacher: '张教授', color: 'bg-orange-100 border-l-4 border-orange-400' },
  { weekday: 1, period: 3, name: '大学英语', room: 'B205', teacher: '李老师', color: 'bg-blue-100 border-l-4 border-blue-400' },
  { weekday: 2, period: 2, name: '程序设计', room: 'C301', teacher: '王教授', color: 'bg-green-100 border-l-4 border-green-400' },
  { weekday: 2, period: 5, name: '数据结构', room: 'C302', teacher: '赵老师', color: 'bg-purple-100 border-l-4 border-purple-400' },
  { weekday: 3, period: 1, name: '线性代数', room: 'A102', teacher: '刘教授', color: 'bg-yellow-100 border-l-4 border-yellow-400' },
  { weekday: 3, period: 4, name: '物理实验', room: 'D401', teacher: '陈老师', color: 'bg-pink-100 border-l-4 border-pink-400' },
  { weekday: 4, period: 2, name: '操作系统', room: 'C303', teacher: '周教授', color: 'bg-indigo-100 border-l-4 border-indigo-400' },
  { weekday: 4, period: 6, name: '计算机网络', room: 'C304', teacher: '吴老师', color: 'bg-teal-100 border-l-4 border-teal-400' },
  { weekday: 5, period: 1, name: '数据库原理', room: 'C305', teacher: '郑教授', color: 'bg-red-100 border-l-4 border-red-400' },
  { weekday: 5, period: 3, name: '软件工程', room: 'C306', teacher: '孙老师', color: 'bg-cyan-100 border-l-4 border-cyan-400' },
];

const dateRange = computed(() => {
  return '2025年3月10日 - 3月16日';
});

function getClass(weekday: number, period: number) {
  return classes.find(c => c.weekday === weekday && c.period === period);
}
</script>
