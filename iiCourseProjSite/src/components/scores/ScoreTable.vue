<template>
  <div class="bg-white rounded-2xl shadow-xl overflow-hidden">
    <!-- 统计卡片 -->
    <div class="p-6 border-b border-gray-100">
      <div class="grid grid-cols-2 md:grid-cols-4 gap-4">
        <div class="bg-gradient-to-br from-primary to-secondary rounded-xl p-4 text-white">
          <div class="text-sm opacity-80">本学期绩点</div>
          <div class="text-3xl font-bold">3.85</div>
        </div>
        <div class="bg-blue-50 rounded-xl p-4">
          <div class="text-sm text-blue-600">平均分</div>
          <div class="text-3xl font-bold text-blue-700">87.5</div>
        </div>
        <div class="bg-green-50 rounded-xl p-4">
          <div class="text-sm text-green-600">已修学分</div>
          <div class="text-3xl font-bold text-green-700">86</div>
        </div>
        <div class="bg-purple-50 rounded-xl p-4">
          <div class="text-sm text-purple-600">课程数</div>
          <div class="text-3xl font-bold text-purple-700">24</div>
        </div>
      </div>
    </div>
    
    <!-- 筛选器 -->
    <div class="p-4 border-b border-gray-100 flex flex-wrap items-center gap-4">
      <select v-model="selectedYear" class="px-4 py-2 border border-gray-200 rounded-lg">
        <option value="all">全部学年</option>
        <option value="2024-2025">2024-2025</option>
        <option value="2023-2024">2023-2024</option>
      </select>
      <select v-model="selectedSemester" class="px-4 py-2 border border-gray-200 rounded-lg">
        <option value="all">全部学期</option>
        <option value="1">第一学期</option>
        <option value="2">第二学期</option>
      </select>
    </div>
    
    <!-- 成绩表格 -->
    <div class="overflow-x-auto">
      <table class="w-full">
        <thead class="bg-gray-50">
          <tr>
            <th class="py-3 px-4 text-left text-sm font-semibold text-gray-700">课程名称</th>
            <th class="py-3 px-4 text-center text-sm font-semibold text-gray-700">学分</th>
            <th class="py-3 px-4 text-center text-sm font-semibold text-gray-700">成绩</th>
            <th class="py-3 px-4 text-center text-sm font-semibold text-gray-700">绩点</th>
            <th class="py-3 px-4 text-center text-sm font-semibold text-gray-700">性质</th>
          </tr>
        </thead>
        <tbody class="divide-y divide-gray-100">
          <tr v-for="score in filteredScores" :key="score.name" class="hover:bg-gray-50">
            <td class="py-4 px-4">
              <div class="font-medium text-gray-900">{{ score.name }}</div>
              <div class="text-sm text-gray-500">{{ score.semester }}</div>
            </td>
            <td class="py-4 px-4 text-center text-gray-700">{{ score.credit }}</td>
            <td class="py-4 px-4 text-center">
              <span :class="['px-3 py-1 rounded-full text-sm font-semibold', getScoreClass(score.score)]">
                {{ score.score }}
              </span>
            </td>
            <td class="py-4 px-4 text-center text-gray-700">{{ score.gpa }}</td>
            <td class="py-4 px-4 text-center">
              <span class="px-2 py-1 bg-gray-100 rounded text-xs text-gray-600">
                {{ score.nature }}
              </span>
            </td>
          </tr>
        </tbody>
      </table>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, computed } from 'vue';

const selectedYear = ref('all');
const selectedSemester = ref('all');

const scores = [
  { name: '设计素描', credit: '3.0', score: 92, gpa: '4.0', nature: '必修', semester: '2024-2025-1' },
  { name: '色彩构成', credit: '3.0', score: 88, gpa: '3.7', nature: '必修', semester: '2024-2025-1' },
  { name: '平面构成', credit: '3.5', score: 95, gpa: '4.0', nature: '必修', semester: '2024-2025-1' },
  { name: '艺术概论', credit: '2.0', score: 85, gpa: '3.3', nature: '必修', semester: '2024-2025-1' },
  { name: '数字图像处理', credit: '3.5', score: 90, gpa: '4.0', nature: '必修', semester: '2024-2025-2' },
  { name: '版式设计', credit: '3.0', score: 87, gpa: '3.7', nature: '必修', semester: '2024-2025-2' },
  { name: '插画设计', credit: '3.0', score: 94, gpa: '4.0', nature: '必修', semester: '2024-2025-2' },
  { name: '品牌设计基础', credit: '3.5', score: 91, gpa: '4.0', nature: '必修', semester: '2024-2025-2' },
  { name: '摄影基础', credit: '2.0', score: 88, gpa: '3.7', nature: '必修', semester: '2024-2025-2' },
  { name: 'UI设计入门', credit: '3.0', score: 93, gpa: '4.0', nature: '选修', semester: '2024-2025-2' },
];

const filteredScores = computed(() => {
  return scores;
});

function getScoreClass(score: number) {
  if (score >= 90) return 'bg-green-100 text-green-700';
  if (score >= 80) return 'bg-blue-100 text-blue-700';
  if (score >= 70) return 'bg-yellow-100 text-yellow-700';
  return 'bg-red-100 text-red-700';
}
</script>
