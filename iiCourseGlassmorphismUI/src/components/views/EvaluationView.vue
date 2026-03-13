<template>
  <div class="p-6 h-full overflow-auto gradient-bg">
    <!-- 页面标题 -->
    <div class="flex items-center justify-between mb-6">
      <div>
        <h1 class="text-2xl font-bold text-gradient">教学评价</h1>
        <p class="text-sm text-gray-500 mt-1">评价课程，帮助改进教学质量</p>
      </div>
      <div class="flex gap-2">
        <button class="btn btn-sm btn-ghost text-gray-600 hover:bg-gray-100" @click="refreshData">
          <Icon name="loader" size="sm" />
        </button>
      </div>
    </div>

    <!-- 评价统计 -->
    <div class="grid grid-cols-1 md:grid-cols-3 gap-4 mb-6">
      <div class="glass rounded-xl p-4 card-hover">
        <div class="flex items-center gap-3">
          <div class="w-12 h-12 rounded-xl bg-orange-100 flex items-center justify-center">
            <Icon name="calendar" size="lg" class="text-primary" />
          </div>
          <div>
            <p class="text-sm text-gray-500">待评价课程</p>
            <p class="text-2xl font-bold text-gradient">{{ pendingCount }}</p>
          </div>
        </div>
      </div>
      <div class="glass rounded-xl p-4 card-hover">
        <div class="flex items-center gap-3">
          <div class="w-12 h-12 rounded-xl bg-green-100 flex items-center justify-center">
            <Icon name="check" size="lg" class="text-success" />
          </div>
          <div>
            <p class="text-sm text-gray-500">已完成评价</p>
            <p class="text-2xl font-bold text-success">{{ completedCount }}</p>
          </div>
        </div>
      </div>
      <div class="glass rounded-xl p-4 card-hover">
        <div class="flex items-center gap-3">
          <div class="w-12 h-12 rounded-xl bg-amber-100 flex items-center justify-center">
            <Icon name="clock" size="lg" class="text-warning" />
          </div>
          <div>
            <p class="text-sm text-gray-500">评价截止时间</p>
            <p class="text-2xl font-bold text-warning">{{ deadline }}</p>
          </div>
        </div>
      </div>
    </div>

    <!-- 课程评价列表 -->
    <div class="glass rounded-xl overflow-hidden">
      <div class="p-4 border-b border-gray-200/50">
        <h2 class="text-lg font-semibold text-gray-800">课程列表</h2>
      </div>
      <div class="divide-y divide-gray-100">
        <div
          v-for="course in courses"
          :key="course.id"
          class="p-4 hover:bg-orange-50/50 transition-colors"
        >
          <div class="flex items-start justify-between gap-4">
            <div class="flex-1">
              <div class="flex items-center gap-2 mb-2">
                <h3 class="font-semibold text-gray-800">{{ course.name }}</h3>
                <span
                  class="badge badge-sm"
                  :class="course.evaluated ? 'badge-success' : 'badge-warning'"
                >
                  {{ course.evaluated ? '已评价' : '待评价' }}
                </span>
              </div>
              <div class="flex flex-wrap gap-4 text-sm text-gray-500">
                <span class="flex items-center gap-1">
                  <Icon name="user" size="xs" />
                  {{ course.teacher }}
                </span>
                <span class="flex items-center gap-1">
                  <Icon name="award" size="xs" />
                  {{ course.credit }}学分
                </span>
                <span class="flex items-center gap-1">
                  <Icon name="calendar" size="xs" />
                  {{ course.semester }}
                </span>
              </div>
            </div>
            <button
              class="btn btn-sm"
              :class="course.evaluated ? 'btn-ghost text-gray-600' : 'btn-primary'"
              @click="openEvaluation(course)"
            >
              {{ course.evaluated ? '查看' : '评价' }}
            </button>
          </div>

          <!-- 已评价显示评分 -->
          <div v-if="course.evaluated && course.rating" class="mt-3 flex items-center gap-2">
            <span class="text-sm text-gray-500">您的评分：</span>
            <div class="flex gap-1">
              <Icon
                v-for="i in 5"
                :key="i"
                name="star"
                size="sm"
                :class="i <= course.rating ? 'text-warning' : 'text-gray-300'"
              />
            </div>
            <span class="text-sm font-medium text-warning ml-2">{{ course.rating }}分</span>
          </div>
        </div>
      </div>
      <div v-if="courses.length === 0" class="p-8 text-center text-gray-400">
        <Icon name="info" size="xl" class="mb-2 block mx-auto" />
        <p>暂无课程数据</p>
      </div>
    </div>

    <!-- 评价弹窗 -->
    <dialog id="evaluation-modal" class="modal" :class="{ 'modal-open': showModal }">
      <div class="modal-box glass-strong max-w-2xl border border-white/80">
        <h3 class="font-bold text-lg mb-4 text-gray-800">课程评价</h3>
        <div v-if="selectedCourse" class="space-y-6">
          <div class="p-4 rounded-lg bg-orange-50/50 border border-orange-100">
            <p class="font-medium text-gray-800">{{ selectedCourse.name }}</p>
            <p class="text-sm text-gray-500">{{ selectedCourse.teacher }}</p>
          </div>

          <!-- 评分项 -->
          <div v-for="item in evaluationItems" :key="item.id" class="space-y-2">
            <label class="text-sm font-medium text-gray-700">{{ item.name }}</label>
            <div class="flex gap-2">
              <button
                v-for="score in [1, 2, 3, 4, 5]"
                :key="score"
                class="btn btn-sm btn-circle"
                :class="item.score === score ? 'btn-primary' : 'btn-ghost bg-gray-100 text-gray-600'"
                @click="item.score = score"
              >
                {{ score }}
              </button>
            </div>
          </div>

          <!-- 评论 -->
          <div class="form-control">
            <label class="label">
              <span class="label-text text-gray-700">其他建议（可选）</span>
            </label>
            <textarea
              v-model="comment"
              class="textarea textarea-bordered bg-white/80 border-gray-200 text-gray-800 placeholder-gray-400"
              rows="3"
              placeholder="请输入您的建议..."
            ></textarea>
          </div>
        </div>

        <div class="modal-action">
          <button class="btn btn-ghost text-gray-600" @click="closeModal">取消</button>
          <button class="btn btn-primary" @click="submitEvaluation" :disabled="!canSubmit">
            提交评价
          </button>
        </div>
      </div>
      <form method="dialog" class="modal-backdrop bg-black/30" @click="closeModal">
        <button>关闭</button>
      </form>
    </dialog>
  </div>
</template>

<script setup lang="ts">
import { ref, computed, onMounted } from 'vue';
import { useAppStore } from '../../stores/app';
import Icon from '../icons/Icon.vue';

const store = useAppStore();
const { requestData } = store;

const showModal = ref(false);
const selectedCourse = ref<any>(null);
const comment = ref('');

const courses = computed(() => store.state.evaluations);

const pendingCount = computed(() => courses.value.filter(c => !c.evaluated).length);
const completedCount = computed(() => courses.value.filter(c => c.evaluated).length);
const deadline = computed(() => '2024-12-31');

const evaluationItems = ref([
  { id: 'teaching', name: '教学内容', score: 0 },
  { id: 'method', name: '教学方法', score: 0 },
  { id: 'attitude', name: '教学态度', score: 0 },
  { id: 'effect', name: '教学效果', score: 0 },
]);

const canSubmit = computed(() => {
  return evaluationItems.value.every(item => item.score > 0);
});

const openEvaluation = (course: any) => {
  selectedCourse.value = course;
  if (!course.evaluated) {
    showModal.value = true;
    // 重置评分
    evaluationItems.value.forEach(item => item.score = 0);
    comment.value = '';
  }
};

const closeModal = () => {
  showModal.value = false;
  selectedCourse.value = null;
};

const submitEvaluation = () => {
  if (selectedCourse.value) {
    const avgScore = evaluationItems.value.reduce((sum, item) => sum + item.score, 0) / evaluationItems.value.length;
    console.log('提交评价:', {
      courseId: selectedCourse.value.id,
      scores: evaluationItems.value,
      rating: Math.round(avgScore),
      comment: comment.value,
    });
    // 标记为已评价
    selectedCourse.value.evaluated = true;
    selectedCourse.value.rating = Math.round(avgScore);
    closeModal();
  }
};

const refreshData = () => {
  requestData('evaluations');
};

onMounted(() => {
  requestData('evaluations');
});
</script>
