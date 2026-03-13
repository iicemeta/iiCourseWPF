<template>
  <div class="p-6 h-full overflow-auto gradient-bg">
    <!-- 页面标题 -->
    <div class="mb-6">
      <button class="btn btn-ghost btn-sm mb-4 text-gray-600 hover:bg-gray-100" @click="goBack">
        <Icon name="arrow-left" size="sm" class="mr-1" />
        返回
      </button>
      <h1 class="text-2xl font-bold text-gradient">隐私政策</h1>
      <p class="text-sm text-gray-500 mt-1">最后更新：2024年12月</p>
    </div>

    <div class="max-w-3xl">
      <div class="glass rounded-xl p-8 space-y-8">
        <!-- 简介 -->
        <section>
          <h2 class="text-xl font-semibold mb-3 text-gray-800">简介</h2>
          <p class="text-gray-600 leading-relaxed">
            iiCourse（"我们"）非常重视用户的隐私保护。本隐私政策说明了我们如何收集、使用、存储和保护您的个人信息。使用我们的服务即表示您同意本隐私政策的条款。
          </p>
        </section>

        <!-- 信息收集 -->
        <section>
          <h2 class="text-xl font-semibold mb-3 text-gray-800">我们收集的信息</h2>
          <div class="space-y-3 text-gray-600">
            <p>为了提供教务服务，我们需要收集以下信息：</p>
            <ul class="list-disc list-inside space-y-2 ml-4">
              <li><strong class="text-gray-800">账号信息</strong>：学号、密码（加密存储）</li>
              <li><strong class="text-gray-800">学籍信息</strong>：姓名、学院、专业、班级</li>
              <li><strong class="text-gray-800">课程数据</strong>：课表、成绩、选课记录</li>
              <li><strong class="text-gray-800">设备信息</strong>：用于优化服务体验</li>
            </ul>
          </div>
        </section>

        <!-- 信息使用 -->
        <section>
          <h2 class="text-xl font-semibold mb-3 text-gray-800">信息使用</h2>
          <p class="text-gray-600 leading-relaxed mb-3">
            我们使用收集的信息用于：
          </p>
          <ul class="list-disc list-inside space-y-2 ml-4 text-gray-600">
            <li>提供教务数据查询服务</li>
            <li>同步您的课表和成绩信息</li>
            <li>发送课程提醒和通知</li>
            <li>改进我们的服务质量</li>
          </ul>
        </section>

        <!-- 数据安全 -->
        <section>
          <h2 class="text-xl font-semibold mb-3 text-gray-800">数据安全</h2>
          <p class="text-gray-600 leading-relaxed">
            我们采取多种安全措施保护您的个人信息：
          </p>
          <ul class="list-disc list-inside space-y-2 ml-4 text-gray-600 mt-3">
            <li>所有数据传输采用 HTTPS 加密</li>
            <li>密码使用行业标准的加密算法存储</li>
            <li>本地数据存储经过加密处理</li>
            <li>不会将您的数据出售给第三方</li>
          </ul>
        </section>

        <!-- 数据存储 -->
        <section>
          <h2 class="text-xl font-semibold mb-3 text-gray-800">数据存储</h2>
          <p class="text-gray-600 leading-relaxed">
            您的数据主要存储在本地设备上，用于提供离线访问功能。登录凭证和临时令牌会安全地存储在系统密钥库中。您可以随时在设置中清除所有本地数据。
          </p>
        </section>

        <!-- 用户权利 -->
        <section>
          <h2 class="text-xl font-semibold mb-3 text-gray-800">您的权利</h2>
          <p class="text-gray-600 leading-relaxed mb-3">
            您拥有以下权利：
          </p>
          <ul class="list-disc list-inside space-y-2 ml-4 text-gray-600">
            <li>访问您的个人数据</li>
            <li>更正不准确的信息</li>
            <li>删除您的账户和数据</li>
            <li>导出您的数据副本</li>
            <li>撤回数据使用同意</li>
          </ul>
        </section>

        <!-- 联系我们 -->
        <section>
          <h2 class="text-xl font-semibold mb-3 text-gray-800">联系我们</h2>
          <p class="text-gray-600 leading-relaxed">
            如果您对本隐私政策有任何疑问，或希望行使您的权利，请通过以下方式联系我们：
          </p>
          <div class="mt-4 p-4 rounded-lg bg-orange-50">
            <p class="text-sm text-gray-600">邮箱：privacy@iicourse.example.com</p>
          </div>
        </section>

        <!-- 协议确认 -->
        <div class="pt-6 border-t border-gray-200">
          <label class="label cursor-pointer justify-start gap-3">
            <input type="checkbox" class="checkbox checkbox-primary" v-model="agreed" />
            <span class="label-text text-gray-700">我已阅读并同意隐私政策</span>
          </label>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref } from 'vue';
import { useAppStore } from '../../stores/app';
import Icon from '../icons/Icon.vue';

const store = useAppStore();
const agreed = ref(false);

const goBack = () => {
  store.navigate('login');
};
</script>
