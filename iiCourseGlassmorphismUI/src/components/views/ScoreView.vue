<template>
  <div class="p-6 h-full overflow-auto gradient-bg">
    <!-- 页面标题 -->
    <div class="flex items-center justify-between mb-6">
      <div>
        <h1 class="text-2xl font-bold text-gradient">成绩查询</h1>
        <p class="text-sm text-gray-500 mt-1">查看所有学期成绩</p>
      </div>
      <div class="flex gap-2">
        <button class="btn btn-sm btn-ghost text-gray-600 hover:bg-gray-100" @click="refreshData">
          <Icon name="loader" size="sm" />
        </button>
        <button class="btn btn-sm btn-primary" @click="exportScores">
          <Icon name="external-link" size="sm" class="mr-1" />
          导出
        </button>
      </div>
    </div>

    <!-- 成绩统计卡片 -->
    <div class="grid grid-cols-1 md:grid-cols-4 gap-4 mb-6">
      <div class="glass rounded-xl p-4 card-hover">
        <div class="flex items-center gap-3">
          <div class="w-12 h-12 rounded-xl bg-orange-100 flex items-center justify-center">
            <Icon name="award" size="lg" class="text-primary" />
          </div>
          <div>
            <p class="text-sm text-gray-500">平均绩点</p>
            <p class="text-2xl font-bold text-gradient">{{ gpa.toFixed(2) }}</p>
          </div>
        </div>
      </div>
      <div class="glass rounded-xl p-4 card-hover">
        <div class="flex items-center gap-3">
          <div class="w-12 h-12 rounded-xl bg-green-100 flex items-center justify-center">
            <Icon name="check" size="lg" class="text-success" />
          </div>
          <div>
            <p class="text-sm text-gray-500">已修学分</p>
            <p class="text-2xl font-bold text-success">{{ totalCredits }}</p>
          </div>
        </div>
      </div>
      <div class="glass rounded-xl p-4 card-hover">
        <div class="flex items-center gap-3">
          <div class="w-12 h-12 rounded-xl bg-amber-100 flex items-center justify-center">
            <Icon name="star" size="lg" class="text-warning" />
          </div>
          <div>
            <p class="text-sm text-gray-500">优秀课程</p>
            <p class="text-2xl font-bold text-warning">{{ excellentCount }}</p>
          </div>
        </div>
      </div>
      <div class="glass rounded-xl p-4 card-hover">
        <div class="flex items-center gap-3">
          <div class="w-12 h-12 rounded-xl bg-blue-100 flex items-center justify-center">
            <Icon name="calendar" size="lg" class="text-info" />
          </div>
          <div>
            <p class="text-sm text-gray-500">学期数</p>
            <p class="text-2xl font-bold text-info">{{ semesters.length }}</p>
          </div>
        </div>
      </div>
    </div>

    <!-- 学期选择器 -->
    <div class="glass rounded-xl p-4 mb-6">
      <div class="flex items-center gap-4">
        <span class="text-sm font-medium text-gray-700">选择学期：</span>
        <div class="flex-1 flex gap-2 overflow-x-auto">
          <button
            class="btn btn-sm"
            :class="selectedSemester === 'all' ? 'btn-primary' : 'btn-ghost bg-gray-100 text-gray-700'"
            @click="selectedSemester = 'all'"
          >
            全部
          </button>
          <button
            v-for="semester in semesters"
            :key="semester"
            class="btn btn-sm"
            :class="selectedSemester === semester ? 'btn-primary' : 'btn-ghost bg-gray-100 text-gray-700'"
            @click="selectedSemester = semester"
          >
            {{ semester }}
          </button>
        </div>
      </div>
    </div>

    <!-- 成绩列表 -->
    <div class="glass rounded-xl overflow-hidden">
      <div class="overflow-x-auto">
        <table class="table table-zebra w-full">
          <thead>
            <tr class="bg-orange-50">
              <th class="text-left text-gray-700">课程名称</th>
              <th class="text-center text-gray-700">学分</th>
              <th class="text-center text-gray-700">类型</th>
              <th class="text-center text-gray-700">成绩</th>
              <th class="text-center text-gray-700">绩点</th>
              <th class="text-center text-gray-700">学期</th>
            </tr>
          </thead>
          <tbody>
            <tr
              v-for="score in filteredScores"
              :key="score.id"
              class="hover:bg-orange-50/30"
            >
              <td>
                <div class="flex items-center gap-2">
                  <div
                    class="w-2 h-2 rounded-full"
                    :style="{ backgroundColor: getScoreColor(score.score) }"
                  ></div>
                  <span class="font-medium text-gray-800">{{ score.name }}</span>
                </div>
              </td>
              <td class="text-center text-gray-600">{{ score.credit }}</td>
              <td class="text-center">
                <span class="badge badge-sm" :class="getTypeClass(score.type)">
                  {{ score.type }}
                </span>
              </td>
              <td class="text-center">
                <span
                  class="font-bold"
                  :class="getScoreClass(score.score)"
                >
                  {{ score.score }}
                </span>
              </td>
              <td class="text-center text-gray-600">{{ score.gpa.toFixed(1) }}</td>
              <td class="text-center text-sm text-gray-500">{{ score.semester }}</td>
            </tr>
          </tbody>
        </table>
      </div>
      <div v-if="filteredScores.length === 0" class="p-8 text-center text-gray-400">
        <Icon name="info" size="xl" class="mb-2 block mx-auto" />
        <p>暂无成绩数据</p>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, computed, onMounted } from 'vue';
import { useAppStore } from '../../stores/app';
import Icon from '../icons/Icon.vue';

const store = useAppStore();
const { requestData } = store;

const selectedSemester = ref('all');

const scores = computed(() => store.state.scores);

const semesters = computed(() => {
  const set = new Set(scores.value.map(s => s.semester));
  return Array.from(set).sort();
});

const filteredScores = computed(() => {
  if (selectedSemester.value === 'all') {
    return scores.value;
  }
  return scores.value.filter(s => s.semester === selectedSemester.value);
});

const gpa = computed(() => {
  if (scores.value.length === 0) return 0;
  const totalGpa = scores.value.reduce((sum, s) => sum + s.gpa * s.credit, 0);
  const totalCredits = scores.value.reduce((sum, s) => sum + s.credit, 0);
  return totalCredits > 0 ? totalGpa / totalCredits : 0;
});

const totalCredits = computed(() => {
  return scores.value.reduce((sum, s) => sum + s.credit, 0);
});

const excellentCount = computed(() => {
  return scores.value.filter(s => s.score >= 90).length;
});

const getScoreColor = (score: number): string => {
  if (score >= 90) return '#10B981';
  if (score >= 80) return '#3ABFF8';
  if (score >= 70) return '#F59E0B';
  if (score >= 60) return '#F97316';
  return '#EF4444';
};

const getScoreClass = (score: number): string => {
  if (score >= 90) return 'text-success';
  if (score >= 80) return 'text-info';
  if (score >= 70) return 'text-warning';
  if (score >= 60) return 'text-orange-500';
  return 'text-error';
};

const getTypeClass = (type: string): string => {
  const map: Record<string, string> = {
    '必修': 'badge-primary',
    '选修': 'badge-secondary',
    '通识': 'badge-accent',
  };
  return map[type] || 'badge-ghost';
};

const refreshData = () => {
  requestData('scores');
};

const exportScores = () => {
  console.log('导出成绩');
};

onMounted(() => {
  requestData('scores');
});
</script>
