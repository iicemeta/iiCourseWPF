// 全局状态管理
import { reactive, computed } from 'vue';
import { bridge } from '../utils/bridge';

// 用户信息接口
export interface UserInfo {
  name: string;
  studentId: string;
  college?: string;
  major?: string;
  grade?: string;
  className?: string;
}

// 课程信息接口
export interface Course {
  name: string;
  teacher: string;
  location: string;
  day: number; // 0-6 表示周一到周日
  startSlot: number; // 开始节次
  endSlot: number; // 结束节次
  color?: string;
}

// 成绩信息接口
export interface Score {
  courseName: string;
  credit: number;
  score: number;
  gpa: number;
  type: string; // 必修/选修
  semester: string;
}

// 应用状态
interface AppState {
  // 导航状态
  currentView: string;
  sidebarCollapsed: boolean;
  
  // 用户状态
  isLoggedIn: boolean;
  userInfo: UserInfo | null;
  
  // 数据缓存
  schedule: Course[];
  scores: Score[];
  
  // UI 状态
  isLoading: boolean;
  errorMessage: string | null;
}

const state = reactive<AppState>({
  currentView: 'login',
  sidebarCollapsed: false,
  isLoggedIn: false,
  userInfo: null,
  schedule: [],
  scores: [],
  isLoading: false,
  errorMessage: null
});

// 计算属性
const isLoggedIn = computed(() => state.isLoggedIn);
const currentView = computed(() => state.currentView);
const userInfo = computed(() => state.userInfo);

// 方法
const actions = {
  // 导航
  navigate(view: string) {
    state.currentView = view;
    bridge.navigate(view);
  },
  
  // 登录
  login(username: string, password: string) {
    state.isLoading = true;
    bridge.login(username, password);
  },
  
  // 处理登录结果
  handleLoginResult(success: boolean, userData?: UserInfo) {
    state.isLoading = false;
    if (success && userData) {
      state.isLoggedIn = true;
      state.userInfo = userData;
      // 注意：导航由后端通过 navigate 消息控制，这里不设置 currentView
    } else {
      state.errorMessage = '登录失败，请检查账号密码';
    }
  },
  
  // 登出
  logout() {
    state.isLoggedIn = false;
    state.userInfo = null;
    state.currentView = 'login';
    state.schedule = [];
    state.scores = [];
  },
  
  // 切换侧边栏
  toggleSidebar() {
    state.sidebarCollapsed = !state.sidebarCollapsed;
  },
  
  // 设置加载状态
  setLoading(loading: boolean) {
    state.isLoading = loading;
  },
  
  // 设置错误信息
  setError(message: string | null) {
    state.errorMessage = message;
  },
  
  // 更新课表数据
  setSchedule(courses: Course[]) {
    state.schedule = courses;
  },
  
  // 更新成绩数据
  setScores(scores: Score[]) {
    state.scores = scores;
  },
  
  // 更新用户信息
  setUserInfo(info: UserInfo) {
    state.userInfo = info;
  }
};

// 注册桥接消息监听
bridge.on('navigate', (view: string) => {
  // 统一转换为小写以匹配前端视图名称
  state.currentView = view.toLowerCase();
});

bridge.on('loginStatus', (data: { isLoggedIn: boolean }) => {
  state.isLoggedIn = data.isLoggedIn;
});

bridge.on('userInfo', (data: UserInfo) => {
  state.userInfo = data;
});

bridge.on('loginCompleted', (data: { success: boolean; username?: string }) => {
  actions.handleLoginResult(data.success, data.username ? { 
    name: data.username, 
    studentId: '' 
  } : undefined);
});

bridge.on('scheduleData', (data: Course[]) => {
  state.schedule = data;
});

bridge.on('scoresData', (data: Score[]) => {
  state.scores = data;
});

export function useAppStore() {
  return {
    state,
    isLoggedIn,
    currentView,
    userInfo,
    ...actions
  };
}
