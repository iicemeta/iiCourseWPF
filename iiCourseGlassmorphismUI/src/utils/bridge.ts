// WebView2 通信桥接
// 用于前端与 C# 后端进行双向通信

interface WebMessage {
  type: string;
  data?: any;
}

type MessageHandler = (data: any) => void;

class WebViewBridge {
  private handlers: Map<string, MessageHandler[]> = new Map();
  private isWebView: boolean;

  constructor() {
    // 检测是否在 WebView2 环境中
    this.isWebView = typeof (window as any).chrome?.webview !== 'undefined';
    
    if (this.isWebView) {
      // 监听来自 C# 的消息
      (window as any).chrome.webview.addEventListener('message', (event: any) => {
        this.handleMessage(event.data);
      });
    }
  }

  // 发送消息到 C# 后端
  public send(type: string, data?: any): void {
    const message: WebMessage = { type, data };
    
    if (this.isWebView) {
      (window as any).chrome.webview.postMessage(message);
    } else {
      // 开发环境：打印到控制台
      console.log('[Bridge → C#]', message);
    }
  }

  // 注册消息处理器
  public on(type: string, handler: MessageHandler): void {
    if (!this.handlers.has(type)) {
      this.handlers.set(type, []);
    }
    this.handlers.get(type)!.push(handler);
  }

  // 移除消息处理器
  public off(type: string, handler: MessageHandler): void {
    const handlers = this.handlers.get(type);
    if (handlers) {
      const index = handlers.indexOf(handler);
      if (index > -1) {
        handlers.splice(index, 1);
      }
    }
  }

  // 处理收到的消息
  private handleMessage(message: WebMessage): void {
    const handlers = this.handlers.get(message.type);
    if (handlers) {
      handlers.forEach(handler => handler(message.data));
    }
  }

  // 导航到指定页面
  public navigate(view: string): void {
    this.send('navigate', view);
  }

  // 登录
  public login(username: string, password: string): void {
    this.send('login', { username, password });
  }

  // 请求数据
  public requestData(dataType: string): void {
    this.send('requestData', dataType);
  }

  // 窗口控制
  public minimizeWindow(): void {
    this.send('window', 'minimize');
  }

  public maximizeWindow(): void {
    this.send('window', 'maximize');
  }

  public closeWindow(): void {
    this.send('window', 'close');
  }
}

// 单例导出
export const bridge = new WebViewBridge();

// 组合式函数供 Vue 使用
export function useBridge() {
  return {
    bridge,
    navigate: (view: string) => bridge.navigate(view),
    login: (username: string, password: string) => bridge.login(username, password),
    requestData: (type: string) => bridge.requestData(type),
    onMessage: (type: string, handler: MessageHandler) => {
      bridge.on(type, handler);
      return () => bridge.off(type, handler);
    }
  };
}
