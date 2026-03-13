<template>
  <div class="p-6 h-full overflow-auto gradient-bg">
    <!-- 页面标题 -->
    <div class="flex items-center justify-between mb-6">
      <div>
        <h1 class="text-2xl font-bold text-gradient">空教室查询</h1>
        <p class="text-sm text-gray-500 mt-1">查找可用的自习教室</p>
      </div>
      <div class="flex gap-2">
        <button class="btn btn-sm btn-ghost text-gray-600 hover:bg-gray-100" @click="refreshData">
          <Icon name="loader" size="sm" />
        </button>
      </div>
    </div>

    <!-- 筛选条件 -->
    <div class="glass rounded-xl p-4 mb-6">
      <div class="grid grid-cols-1 md:grid-cols-4 gap-4">
        <!-- 日期选择 -->
        <div class="form-control">
          <label class="label">
            <span class="label-text text-gray-700">日期</span>
          </label>
          <input
            v-model="filters.date"
            type="date"
            class="input input-bordered input-sm bg-white/80 border-gray-200 text-gray-800"
          />
        </div>

        <!-- 时间段 -->
        <div class="form-control">
          <label class="label">
            <span class="label-text text-gray-700">时间段</span>
          </label>
          <select v-model="filters.timeSlot" class="select select-bordered select-sm bg-white/80 border-gray-200 text-gray-800">
            <option value="">全部</option>
            <option value="1-2">第1-2节 (08:00-09:40)</option>
            <option value="3-4">第3-4节 (10:00-11:40)</option>
            <option value="5-6">第5-6节 (14:00-15:40)</option>
            <option value="7-8">第7-8节 (16:00-17:40)</option>
            <option value="9-10">第9-10节 (19:00-20:40)</option>
          </select>
        </div>

        <!-- 教学楼 -->
        <div class="form-control">
          <label class="label">
            <span class="label-text text-gray-700">教学楼</span>
          </label>
          <select v-model="filters.building" class="select select-bordered select-sm bg-white/80 border-gray-200 text-gray-800">
            <option value="">全部</option>
            <option v-for="building in buildings" :key="building" :value="building">
              {{ building }}
            </option>
          </select>
        </div>

        <!-- 容量 -->
        <div class="form-control">
          <label class="label">
            <span class="label-text text-gray-700">最小容量</span>
          </label>
          <select v-model="filters.minCapacity" class="select select-bordered select-sm bg-white/80 border-gray-200 text-gray-800">
            <option value="0">不限</option>
            <option value="30">30人以上</option>
            <option value="50">50人以上</option>
            <option value="100">100人以上</option>
            <option value="200">200人以上</option>
          </select>
        </div>
      </div>
    </div>

    <!-- 教学楼快捷选择 -->
    <div class="flex flex-wrap gap-2 mb-6">
      <button
        class="btn btn-sm"
        :class="filters.building === '' ? 'btn-primary' : 'btn-ghost bg-gray-100 text-gray-700'"
        @click="filters.building = ''"
      >
        全部
      </button>
      <button
        v-for="building in buildings"
        :key="building"
        class="btn btn-sm"
        :class="filters.building === building ? 'btn-primary' : 'btn-ghost bg-gray-100 text-gray-700'"
        @click="filters.building = building"
      >
        {{ building }}
      </button>
    </div>

    <!-- 空教室列表 -->
    <div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4">
      <div
        v-for="classroom in filteredClassrooms"
        :key="classroom.id"
        class="glass rounded-xl p-4 card-hover"
      >
        <div class="flex items-start justify-between">
          <div>
            <h3 class="font-semibold text-lg text-gray-800">{{ classroom.name }}</h3>
            <p class="text-sm text-gray-500">{{ classroom.building }}</p>
          </div>
          <div class="badge badge-success">空闲</div>
        </div>

        <div class="mt-4 space-y-2">
          <div class="flex items-center gap-2 text-sm text-gray-500">
            <Icon name="users" size="xs" />
            <span>容量：{{ classroom.capacity }}人</span>
          </div>
          <div class="flex items-center gap-2 text-sm text-gray-500">
            <Icon name="map-pin" size="xs" />
            <span>位置：{{ classroom.location }}</span>
          </div>
          <div class="flex items-center gap-2 text-sm text-gray-500">
            <Icon name="clock" size="xs" />
            <span>空闲时段：{{ classroom.freeTime }}</span>
          </div>
        </div>

        <div class="mt-4 flex gap-2">
          <button class="btn btn-sm btn-primary flex-1" @click="bookClassroom(classroom)">
            预约
          </button>
          <button class="btn btn-sm btn-ghost text-gray-600 hover:bg-gray-100" @click="viewDetails(classroom)">
            详情
          </button>
        </div>
      </div>
    </div>

    <!-- 无结果提示 -->
    <div v-if="filteredClassrooms.length === 0" class="glass rounded-xl p-12 text-center text-gray-400">
      <Icon name="search" size="xl" class="mb-2 block mx-auto" />
      <p class="text-lg mb-2">未找到符合条件的教室</p>
      <p class="text-sm">请尝试调整筛选条件</p>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, computed, onMounted } from 'vue';
import { useAppStore } from '../../stores/app';
import Icon from '../icons/Icon.vue';

const store = useAppStore();

const filters = ref({
  date: new Date().toISOString().split('T')[0],
  timeSlot: '',
  building: '',
  minCapacity: '0',
});

const buildings = ['教学楼A', '教学楼B', '教学楼C', '图书馆', '实验楼'];

const classrooms = ref([
  { id: 1, name: 'A101', building: '教学楼A', capacity: 60, location: '1楼东侧', freeTime: '全天' },
  { id: 2, name: 'A205', building: '教学楼A', capacity: 80, location: '2楼西侧', freeTime: '14:00-17:00' },
  { id: 3, name: 'B301', building: '教学楼B', capacity: 120, location: '3楼东侧', freeTime: '全天' },
  { id: 4, name: 'C102', building: '教学楼C', capacity: 50, location: '1楼西侧', freeTime: '08:00-12:00' },
  { id: 5, name: 'L201', building: '图书馆', capacity: 200, location: '2楼自习区', freeTime: '全天' },
  { id: 6, name: 'E305', building: '实验楼', capacity: 40, location: '3楼东侧', freeTime: '19:00-22:00' },
]);

const filteredClassrooms = computed(() => {
  return classrooms.value.filter(c => {
    if (filters.value.building && c.building !== filters.value.building) return false;
    if (parseInt(filters.value.minCapacity) > c.capacity) return false;
    return true;
  });
});

const bookClassroom = (classroom: any) => {
  console.log('预约教室:', classroom);
};

const viewDetails = (classroom: any) => {
  console.log('查看详情:', classroom);
};

const refreshData = () => {
  console.log('刷新数据');
};
</script>
