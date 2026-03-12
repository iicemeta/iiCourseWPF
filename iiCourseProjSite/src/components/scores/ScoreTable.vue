<template>
  <div class="card bg-base-100 shadow-xl overflow-hidden">
    <!-- 统计卡片 -->
    <div class="card-body border-b border-base-300">
      <div class="grid grid-cols-2 md:grid-cols-4 gap-4">
        <div class="card bg-gradient-to-br from-primary to-secondary text-white">
          <div class="card-body p-4">
            <div class="text-sm opacity-80">本学期绩点</div>
            <div class="text-3xl font-bold">3.85</div>
          </div>
        </div>
        <div class="card bg-blue-50 border border-blue-200">
          <div class="card-body p-4">
            <div class="text-sm text-blue-600">平均分</div>
            <div class="text-3xl font-bold text-blue-700">87.5</div>
          </div>
        </div>
        <div class="card bg-green-50 border border-green-200">
          <div class="card-body p-4">
            <div class="text-sm text-green-600">已修学分</div>
            <div class="text-3xl font-bold text-green-700">86</div>
          </div>
        </div>
        <div class="card bg-purple-50 border border-purple-200">
          <div class="card-body p-4">
            <div class="text-sm text-purple-600">课程数</div>
            <div class="text-3xl font-bold text-purple-700">24</div>
          </div>
        </div>
      </div>
    </div>
    
    <!-- 筛选器 -->
    <div class="card-body py-4 border-b border-base-300 flex flex-wrap items-center gap-4">
      <select v-model="selectedYear" class="select select-bordered select-sm">
        <option value="all">全部学年</option>
        <option value="2024-2025">2024-2025</option>
        <option value="2023-2024">2023-2024</option>
      </select>
      <select v-model="selectedSemester" class="select select-bordered select-sm">
        <option value="all">全部学期</option>
        <option value="1">第一学期</option>
        <option value="2">第二学期</option>
      </select>
    </div>
    
    <!-- 成绩表格 -->
    <div class="overflow-x-auto">
      <table class="table table-zebra w-full">
        <thead>
          <tr class="bg-base-200">
            <th class="py-3 px-4 text-left text-sm font-semibold text-base-content">课程名称</th>
            <th class="py-3 px-4 text-center text-sm font-semibold text-base-content">学分</th>
            <th class="py-3 px-4 text-center text-sm font-semibold text-base-content">成绩</th>
            <th class="py-3 px-4 text-center text-sm font-semibold text-base-content">绩点</th>
            <th class="py-3 px-4 text-center text-sm font-semibold text-base-content">性质</th>
          </tr>
        </thead>
        <tbody class="divide-y divide-base-300">
          <tr v-for="score in filteredScores" :key="score.name" class="hover:bg-base-200">
            <td class="py-4 px-4">
              <div class="font-medium text-base-content">{{ score.name }}</div>
              <div class="text-sm text-base-content/50">{{ score.semester }}</div>
            </td>
            <td class="py-4 px-4 text-center text-base-content">{{ score.credit }}</td>
            <td class="py-4 px-4 text-center">
              <span :class="['badge', getScoreClass(score.score)]">
                {{ score.score }}
              </span>
            </td>
            <td class="py-4 px-4 text-center text-base-content">{{ score.gpa }}</td>
            <td class="py-4 px-4 text-center">
              <span class="badge badge-ghost badge-sm">
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
  if (score >= 90) return 'badge-success';
  if (score >= 80) return 'badge-info';
  if (score >= 70) return 'badge-warning';
  return 'badge-error';
}
</script>
