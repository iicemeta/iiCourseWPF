// Navigation 配置文件 - 类似 Docusaurus 的配置方式
// 修改此文件即可更新导航栏内容，无需修改组件源码

export interface NavItem {
  label: string;
  href: string;
  // 是否在新窗口打开
  external?: boolean;
  // 子菜单项（如果有则为下拉菜单）
  children?: Omit<NavItem, 'children'>[];
}

export interface NavAction {
  label: string;
  href: string;
  // 按钮样式: primary | secondary | ghost
  variant: 'primary' | 'secondary' | 'ghost';
  // 是否显示图标
  showIcon?: boolean;
  // 是否只在桌面端显示
  desktopOnly?: boolean;
}

export interface NavigationConfig {
  // 品牌配置
  brand: {
    name: string;
    logo: string;
    tagline?: string;
  };
  // 导航项列表
  items: NavItem[];
  // 右侧操作按钮
  actions: NavAction[];
}

export const navigationConfig: NavigationConfig = {
  brand: {
    name: "iiCourse",
    logo: "/iiCourse.png",
    tagline: "智慧校园，一手掌握"
  },
  items: [
    { label: "首页", href: "/" },
    { label: "功能", href: "/features" },
    { label: "更新日志", href: "/updates" },
    { label: "新闻", href: "/news" },
    {
      label: "关于",
      href: "#",
      children: [
        { label: "联络", href: "/contact" },
        { label: "隐私政策", href: "/privacy" },
        { label: "使用条款", href: "/terms" }
      ]
    }
  ],
  actions: [
    {
      label: "立即下载",
      href: "/download",
      variant: "primary",
      showIcon: true,
      desktopOnly: true
    }
  ]
};
