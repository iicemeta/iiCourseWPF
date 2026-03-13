using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using iiCourse.Core.ViewModels;
using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.Wpf;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace iiCourseWPF
{
    /// <summary>
    /// 主窗口 - WebView2 宿主，负责加载 Web UI 并处理与 C# 后端的通信
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainViewModel? _viewModel;
        private WebView2? _webView;

        public MainWindow()
        {
            InitializeComponent();
            _webView = WebView;
            InitializeWebView();
        }

        /// <summary>
        /// 初始化 WebView2
        /// </summary>
        private async void InitializeWebView()
        {
            if (_webView == null) return;

            try
            {
                // 确保 WebView2 运行时已安装
                var env = await CoreWebView2Environment.CreateAsync();
                await _webView.EnsureCoreWebView2Async(env);

                // 配置 WebView2 设置
                _webView.CoreWebView2.Settings.AreDefaultContextMenusEnabled = false;
                _webView.CoreWebView2.Settings.AreDevToolsEnabled = true;
                _webView.CoreWebView2.Settings.IsStatusBarEnabled = false;
                _webView.CoreWebView2.Settings.IsZoomControlEnabled = false;

                // 添加消息接收器，处理前端发来的消息
                _webView.CoreWebView2.WebMessageReceived += OnWebMessageReceived;

                // 添加导航完成事件，用于调试
                _webView.CoreWebView2.NavigationCompleted += (s, e) =>
                {
                    System.Diagnostics.Debug.WriteLine($"导航完成: {_webView.Source}");
                    System.Diagnostics.Debug.WriteLine($"导航结果: {e.WebErrorStatus}");

                    // 注入脚本捕获控制台错误
                    _webView.CoreWebView2.ExecuteScriptAsync(@"
                        (function() {
                            const originalError = console.error;
                            console.error = function(...args) {
                                window.chrome.webview.postMessage({type: 'console', level: 'error', message: args.join(' ')});
                                originalError.apply(console, args);
                            };
                            const originalLog = console.log;
                            console.log = function(...args) {
                                window.chrome.webview.postMessage({type: 'console', level: 'log', message: args.join(' ')});
                                originalLog.apply(console, args);
                            };
                        })();
                    ");
                };

                // 导航到本地 Web UI
                var exeLocation = Assembly.GetExecutingAssembly().Location;
                var exeDir = Path.GetDirectoryName(exeLocation) ?? "";
                var webUiPath = Path.Combine(exeDir, "WebUI", "index.html");

                System.Diagnostics.Debug.WriteLine($"EXE 位置: {exeLocation}");
                System.Diagnostics.Debug.WriteLine($"EXE 目录: {exeDir}");
                System.Diagnostics.Debug.WriteLine($"WebUI 路径: {webUiPath}");
                System.Diagnostics.Debug.WriteLine($"文件是否存在: {File.Exists(webUiPath)}");

                if (File.Exists(webUiPath))
                {
                    var webUiFolder = Path.Combine(exeDir, "WebUI");

                    // 使用 SetVirtualHostNameToFolderMapping 将本地文件夹映射到虚拟域名
                    // 这样可以使用 https://app.local/ 访问本地文件，避免 file:// 的 CORS 问题
                    _webView.CoreWebView2.SetVirtualHostNameToFolderMapping(
                        "app.local",
                        webUiFolder,
                        CoreWebView2HostResourceAccessKind.Allow);

                    System.Diagnostics.Debug.WriteLine($"虚拟主机映射: app.local -> {webUiFolder}");
                    System.Diagnostics.Debug.WriteLine($"导航到: https://app.local/index.html");

                    // 导航到虚拟域名
                    _webView.Source = new Uri("https://app.local/index.html");
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("WebUI 文件不存在，使用备用 HTML");
                    // 如果本地文件不存在，加载内嵌的 HTML
                    _webView.NavigateToString(GetFallbackHtml());
                }

                // 初始化 ViewModel
                InitializeViewModel();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"初始化 WebView2 失败: {ex}");
                MessageBox.Show($"初始化失败: {ex.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// 初始化 ViewModel
        /// </summary>
        private void InitializeViewModel()
        {
            _viewModel = new MainViewModel();
            DataContext = _viewModel;

            // 监听 ViewModel 属性变化，同步到前端
            _viewModel.PropertyChanged += (s, e) =>
            {
                switch (e.PropertyName)
                {
                    case nameof(MainViewModel.CurrentView):
                        SendMessageToWeb("navigate", _viewModel.CurrentView);
                        break;
                    case nameof(MainViewModel.IsLoggedIn):
                        SendMessageToWeb("loginStatus", new { isLoggedIn = _viewModel.IsLoggedIn });
                        break;
                    case nameof(MainViewModel.CurrentName):
                    case nameof(MainViewModel.CurrentStudentId):
                        SendMessageToWeb("userInfo", new
                        {
                            name = _viewModel.CurrentName,
                            studentId = _viewModel.CurrentStudentId
                        });
                        break;
                }
            };

            // 监听登录完成事件
            if (_viewModel.LoginViewModel != null)
            {
                _viewModel.LoginViewModel.LoginCompleted += (success, username) =>
                {
                    SendMessageToWeb("loginCompleted", new { success, username });
                    if (!success)
                    {
                        MessageBox.Show(
                            "登录失败，请检查用户名和密码。",
                            "登录错误",
                            MessageBoxButton.OK,
                            MessageBoxImage.Error);
                    }
                };
            }
        }

        /// <summary>
        /// 接收来自 Web 前端的消息
        /// </summary>
        private void OnWebMessageReceived(object? sender, CoreWebView2WebMessageReceivedEventArgs e)
        {
            try
            {
                var message = JsonConvert.DeserializeObject<WebMessage>(e.WebMessageAsJson);
                if (message == null) return;

                switch (message.Type)
                {
                    case "navigate":
                        _viewModel?.NavigateCommand.Execute(message.Data?.ToString());
                        break;
                    case "login":
                        try
                        {
                            // 将登录数据反序列化为强类型对象
                            var loginData = message.Data is JObject jObj 
                                ? jObj.ToObject<LoginData>() 
                                : JsonConvert.DeserializeObject<LoginData>(message.Data?.ToString() ?? "");
                            
                            if (loginData != null && _viewModel?.LoginViewModel != null)
                            {
                                System.Diagnostics.Debug.WriteLine($"收到登录请求: Username={loginData.Username}");
                                _viewModel.LoginViewModel.Username = loginData.Username ?? "";
                                _viewModel.LoginViewModel.Password = loginData.Password ?? "";
                                _viewModel.LoginViewModel.LoginCommand.Execute(null);
                            }
                            else
                            {
                                System.Diagnostics.Debug.WriteLine($"登录数据解析失败: Data={message.Data}");
                            }
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine($"登录消息处理异常: {ex.Message}");
                        }
                        break;
                    case "requestData":
                        HandleDataRequest(message.Data?.ToString() ?? "");
                        break;
                    case "window":
                        HandleWindowCommand(message.Data?.ToString() ?? "");
                        break;
                    case "console":
                        // 处理前端控制台消息
                        if (message.Data is JObject consoleData)
                        {
                            var level = consoleData["level"]?.ToString();
                            var msg = consoleData["message"]?.ToString();
                            System.Diagnostics.Debug.WriteLine($"[Web {level?.ToUpper()}] {msg}");
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"消息处理错误: {ex.Message}");
            }
        }

        /// <summary>
        /// 处理数据请求
        /// </summary>
        private void HandleDataRequest(string dataType)
        {
            switch (dataType)
            {
                case "schedule":
                    // 发送课表数据
                    SendMessageToWeb("scheduleData", _viewModel?.ClassScheduleViewModel?.Classes);
                    break;
                case "scores":
                    // 发送成绩数据
                    SendMessageToWeb("scoresData", _viewModel?.ScoreViewModel?.Scores);
                    break;
                case "userInfo":
                    // 发送用户信息
                    SendMessageToWeb("userInfoData", _viewModel?.UserInfoViewModel);
                    break;
            }
        }

        /// <summary>
        /// 处理窗口命令
        /// </summary>
        private void HandleWindowCommand(string command)
        {
            Dispatcher.Invoke(() =>
            {
                switch (command)
                {
                    case "minimize":
                        WindowState = WindowState.Minimized;
                        break;
                    case "maximize":
                        WindowState = WindowState == WindowState.Maximized 
                            ? WindowState.Normal 
                            : WindowState.Maximized;
                        break;
                    case "close":
                        Close();
                        break;
                }
            });
        }

        /// <summary>
        /// 发送消息到 Web 前端
        /// </summary>
        private void SendMessageToWeb(string type, object? data)
        {
            if (_webView?.CoreWebView2 == null) return;

            var message = new WebMessage
            {
                Type = type,
                Data = data
            };

            var json = JsonConvert.SerializeObject(message);
            _webView.CoreWebView2.PostWebMessageAsJson(json);
        }

        /// <summary>
        /// 获取备用 HTML（当本地文件不存在时）
        /// </summary>
        private string GetFallbackHtml()
        {
            return @"<!DOCTYPE html>
<html>
<head>
    <meta charset='utf-8'>
    <title>iiCourse</title>
    <style>
        body { 
            font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', sans-serif;
            background: #1a1a2e;
            color: #fff;
            display: flex;
            align-items: center;
            justify-content: center;
            height: 100vh;
            margin: 0;
        }
        .loading { text-align: center; }
        .spinner {
            width: 40px;
            height: 40px;
            border: 3px solid #ff6b35;
            border-top-color: transparent;
            border-radius: 50%;
            animation: spin 1s linear infinite;
            margin: 0 auto 20px;
        }
        @keyframes spin { to { transform: rotate(360deg); } }
    </style>
</head>
<body>
    <div class='loading'>
        <div class='spinner'></div>
        <p>正在加载 iiCourse...</p>
        <p style='color: #888; font-size: 14px;'>请确保 WebUI 文件已正确部署</p>
    </div>
</body>
</html>";
        }

        /// <summary>
        /// 窗口关闭事件
        /// </summary>
        protected override void OnClosed(EventArgs e)
        {
            _viewModel?.Dispose();
            _webView?.Dispose();
            base.OnClosed(e);
        }
    }

    /// <summary>
    /// Web 消息结构
    /// </summary>
    public class WebMessage
    {
        public string Type { get; set; } = "";
        public object? Data { get; set; }
    }

    /// <summary>
    /// 登录数据
    /// </summary>
    public class LoginData
    {
        public string Username { get; set; } = "";
        public string Password { get; set; } = "";
    }
}
