<template>
  <div class="p-6 h-full overflow-auto gradient-bg">
    <!-- 页面标题 -->
    <div class="flex items-center justify-between mb-6">
      <div>
        <h1 class="text-2xl font-bold text-gradient">课表查询</h1>
        <p class="text-sm text-gray-500 mt-1">本周课程安排一览</p>
      </div>
      <div class="flex gap-2">
        <button class="btn btn-sm btn-ghost text-gray-600 hover:bg-gray-100" @click="refreshData">
          <Icon name="loader" size="sm" />
        </button>
        <button class="btn btn-sm btn-primary" @click="exportSchedule">
          <Icon name="external-link" size="sm" class="mr-1" />
          导出
        </button>
      </div>
    </div>

    <!-- 周选择器 -->
    <div class="glass rounded-xl p-4 mb-6">
      <div class="flex items-center gap-4">
        <button class="btn btn-sm btn-ghost btn-circle text-gray-600 hover:bg-gray-100" @click="prevWeek">
          <Icon name="chevron-left" size="sm" />
        </button>
        <div class="flex-1 flex gap-2 overflow-x-auto pb-1">
          <button
            v-for="week in weeks"
            :key="week"
            class="btn btn-sm min-w-[60px]"
            :class="currentWeek === week ? 'btn-primary' : 'btn-ghost bg-gray-100 text-gray-700'"
            @click="selectWeek(week)"
          >
            第{{ week }}周
          </button>
        </div>
        <button class="btn btn-sm btn-ghost btn-circle text-gray-600 hover:bg-gray-100" @click="nextWeek">
          <Icon name="menu" size="sm" />
        </button>
      </div>
    </div>

    <!-- 课表网格 -->
    <div class="glass rounded-xl overflow-hidden">
      <div class="overflow-x-auto">
        <table class="table table-zebra w-full">
          <thead>
            <tr class="bg-orange-50">
              <th class="w-16 text-center text-gray-700">节次</th>
              <th v-for="(day, index) in weekDays" :key="index" class="text-center min-w-[120px] text-gray-700">
                <div class="flex flex-col items-center">
                  <span class="text-sm font-medium">{{ day.name }}</span>
                  <span class="text-xs text-gray-500">{{ day.date }}</span>
                </div>
              </th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="slot in timeSlots" :key="slot.number" class="hover:bg-orange-50/30">
              <td class="text-center py-4">
                <div class="flex flex-col items-center">
                  <span class="text-sm font-bold text-primary">{{ slot.number }}</span>
                  <span class="text-xs text-gray-400">{{ slot.time }}</span>
                </div>
              </td>
              <td
                v-for="dayIndex in 7"
                :key="dayIndex"
                class="p-1 min-h-[80px]"
              >
                <div
                  v-if="getCourse(dayIndex - 1, slot.number)"
                  class="course-card p-2 rounded-lg text-xs cursor-pointer card-hover shadow-sm"
                  :style="{ backgroundColor: getCourseColor(getCourse(dayIndex - 1, slot.number)) }"
                  @click="showCourseDetail(getCourse(dayIndex - 1, slot.number))"
                >
                  <p class="font-medium truncate">{{ getCourse(dayIndex - 1, slot.number)?.name }}</p>
                  <p class="text-white/80 truncate">{{ getCourse(dayIndex - 1, slot.number)?.location }}</p>
                  <p class="text-white/60 truncate">{{ getCourse(dayIndex - 1, slot.number)?.teacher }}</p>
                </div>
              </td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>

    <!-- 今日课程卡片 -->
    <div class="mt-6">
      <h2 class="text-lg font-semibold mb-4 text-gray-800">今日课程</h2>
      <div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4">
        <div
          v-for="course in todayCourses"
          :key="course.name + course.startSlot"
          class="glass rounded-xl p-4 card-hover"
          :style="{ borderLeft: `4px solid ${getCourseColor(course)}` }"
        >
          <div class="flex items-start justify-between">
            <div>
              <h3 class="font-semibold text-gray-800">{{ course.name }}</h3>
              <p class="text-sm text-gray-500 mt-1 flex items-center gap-1">
                <Icon name="map-pin" size="xs" />
                {{ course.location }}
              </p>
              <p class="text-sm text-gray-500 flex items-center gap-1">
                <Icon name="user" size="xs" />
                {{ course.teacher }}
              </p>
            </div>
            <div class="badge badge-primary">
              第{{ course.startSlot }}-{{ course.endSlot }}节
            </div>
          </div>
        </div>
        <div v-if="todayCourses.length === 0" class="glass rounded-xl p-8 text-center text-gray-400">
          <Icon name="star" size="xl" class="mb-2 block mx-auto" />
          <p>今天没有课，好好休息！</p>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, computed, onMounted } from 'vue';
import { useAppStore, type Course } from '../../stores/app';
import Icon from '../icons/Icon.vue';

const store = useAppStore();
const { requestData } = store;

const currentWeek = ref(1);
const weeks = Array.from({ length: 20 }, (_, i) => i + 1);

const weekDays = [
  { name: '周一', date: '12/16' },
  { name: '周二', date: '12/17' },
  { name: '周三', date: '12/18' },
  { name: '周四', date: '12/19' },
  { name: '周五', date: '12/20' },
  { name: '周六', date: '12/21' },
  { name: '周日', date: '12/22' },
];

const timeSlots = [
  { number: 1, time: '08:00' },
  { number: 2, time: '08:50' },
  { number: 3, time: '10:00' },
  { number: 4, time: '10:50' },
  { number: 5, time: '14:00' },
  { number: 6, time: '14:50' },
  { number: 7, time: '16:00' },
  { number: 8, time: '16:50' },
  { number: 9, time: '19:00' },
  { number: 10, time: '19:50' },
];

const courseColors = [
  '#FF6B35',
  '#FF8F5A',
  '#FFD23F',
  '#06D6A0',
  '#3ABFF8',
  '#F87272',
  '#A78BFA',
  '#F472B6',
];

const schedule = computed(() => store.state.schedule);

const todayCourses = computed(() => {
  const today = new Date().getDay() - 1;
  return schedule.value.filter(c => c.day === (today < 0 ? 6 : today));
});

const getCourse = (day: number, slot: number): Course | undefined => {
  return schedule.value.find(c =>
    c.day === day && slot >= c.startSlot && slot <= c.endSlot
  );
};

const getCourseColor = (course: Course | undefined): string => {
  if (!course) return courseColors[0];
  const index = course.name.charCodeAt(0) % courseColors.length;
  return courseColors[index];
};

const selectWeek = (week: number) => {
  currentWeek.value = week;
  requestData('schedule');
};

const prevWeek = () => {
  if (currentWeek.value > 1) {
    currentWeek.value--;
    requestData('schedule');
  }
};

const nextWeek = () => {
  if (currentWeek.value < 20) {
    currentWeek.value++;
    requestData('schedule');
  }
};

const refreshData = () => {
  requestData('schedule');
};

const exportSchedule = () => {
  console.log('导出课表');
};

const showCourseDetail = (course: Course | undefined) => {
  if (course) {
    console.log('课程详情:', course);
  }
};

onMounted(() => {
  requestData('schedule');
});
</script>

<style scoped>
.course-card {
  background: linear-gradient(135deg, var(--bg-color, #FF6B35) 0%, var(--bg-color-light, #FF8F5A) 100%);
  color: white;
}
</style>
