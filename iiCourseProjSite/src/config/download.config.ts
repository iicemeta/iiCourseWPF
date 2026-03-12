// 下载页面配置文件
// 修改此文件即可更新下载页面内容，无需修改页面源码

export interface SystemRequirement {
  icon: string;
  title: string;
  desc: string;
}

export interface InstallStep {
  step: number;
  title: string;
  desc: string;
}

export interface FAQItem {
  question: string;
  answer: string;
}

export interface DownloadConfig {
  // 页面标题
  pageTitle: string;
  pageSubtitle: string;

  // 版本信息
  currentVersion: string;
  releaseDate: string;

  // 下载按钮
  downloadButton: {
    text: string;
    icon: string;
    href: string;
  };
  changelogButton: {
    text: string;
    href: string;
  };

  // 下载说明
  downloadNote: string;

  // 系统要求
  systemRequirementsTitle: string;
  systemRequirements: SystemRequirement[];

  // 安装步骤
  installStepsTitle: string;
  installSteps: InstallStep[];

  // 常见问题
  faqTitle: string;
  faqItems: FAQItem[];
}

export const downloadConfig: DownloadConfig = {
  pageTitle: "下载应用",
  pageSubtitle: "免费下载 iiCourse，让教务管理更简单",

  currentVersion: "1.7.200",
  releaseDate: "2026-03-11",

  downloadButton: {
    text: "下载 Windows 版",
    icon: "🪟",
    href: "#"
  },
  changelogButton: {
    text: "查看更新日志",
    href: "/updates"
  },

  downloadNote: "完全免费 · 开源透明 · 本地运行",

  systemRequirementsTitle: "系统要求",
  systemRequirements: [
    { icon: "🪟", title: "操作系统", desc: "Windows 10/11 (64位)" },
    { icon: "💻", title: "运行环境", desc: ".NET 9 Runtime" },
    { icon: "📦", title: "安装包大小", desc: "约 26 MB" },
    { icon: "🔌", title: "网络", desc: "需要校园网或 VPN" }
  ],

  installStepsTitle: "安装步骤",
  installSteps: [
    { step: 1, title: "下载安装包", desc: "点击上方按钮下载最新版本" },
    { step: 2, title: "运行安装程序", desc: "双击下载的 .exe 文件" },
    { step: 3, title: "完成安装", desc: "按照向导完成安装" },
    { step: 4, title: "开始使用", desc: "打开应用，使用学号登录" }
  ],

  faqTitle: "常见问题",
  faqItems: [
    {
      question: "应用安全吗？",
      answer: "完全安全。应用完全本地运行，不会上传任何数据到第三方服务器。源代码开源，可审计。"
    },
    {
      question: "需要联网吗？",
      answer: "需要连接校园网或使用 VPN 访问教务系统。查询操作需要实时连接学校服务器。"
    },
    {
      question: "会保存密码吗？",
      answer: "密码可以选择保存到本地（加密存储），也可以每次手动输入，完全由你决定。"
    },
    {
      question: "如何更新？",
      answer: "应用会自动检查更新，也可以在设置中手动检查。更新时会保留所有配置。"
    }
  ]
};

export default downloadConfig;
