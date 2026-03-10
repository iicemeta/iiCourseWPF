using iiCourse.Core;

namespace iiCourseMAUI;

/// <summary>
/// 应用程序 Shell 导航
/// </summary>
public partial class AppShell : Shell
{
    private static AppShell? _instance;
    public static AppShell Instance => _instance ??= new AppShell();

    public iiCoreService? Service { get; private set; }
    public string? CurrentUsername { get; private set; }
    public bool IsLoggedIn { get; private set; }

    public AppShell()
    {
        InitializeComponent();
        _instance = this;

        // 注册路由
        Routing.RegisterRoute("login", typeof(Views.LoginView));
        Routing.RegisterRoute("userinfo", typeof(Views.UserInfoView));
        Routing.RegisterRoute("classschedule", typeof(Views.ClassScheduleView));
        Routing.RegisterRoute("score", typeof(Views.ScoreView));
        Routing.RegisterRoute("spareclassroom", typeof(Views.SpareClassroomView));
        Routing.RegisterRoute("cardinfo", typeof(Views.CardInfoView));
        Routing.RegisterRoute("evaluation", typeof(Views.EvaluationView));
        Routing.RegisterRoute("settings", typeof(Views.SettingsView));
        Routing.RegisterRoute("privacy", typeof(Views.PrivacyView));

        // 初始化服务
        InitializeService();
    }

    /// <summary>
    /// 初始化服务
    /// </summary>
    public void InitializeService()
    {
        Service = new iiCoreService
        {
            LogCallback = message => Console.WriteLine(message)
        };
    }

    /// <summary>
    /// 更新登录状态
    /// </summary>
    public void UpdateLoginStatus(bool isLoggedIn, string username = "")
    {
        IsLoggedIn = isLoggedIn;
        CurrentUsername = username;

        if (isLoggedIn)
        {
            LoginStatusLabel.Text = "已登录";
            LoginStatusLabel.TextColor = Color.FromArgb("#27AE60");
            UserNameLabel.Text = username;
            LogoutButton.IsVisible = true;
        }
        else
        {
            LoginStatusLabel.Text = "未登录";
            LoginStatusLabel.TextColor = Color.FromArgb("#7F8C8D");
            UserNameLabel.Text = "";
            LogoutButton.IsVisible = false;
        }
    }

    /// <summary>
    /// 退出登录按钮点击事件
    /// </summary>
    private async void OnLogoutClicked(object? sender, EventArgs e)
    {
        var result = await DisplayAlert("确认退出", "确定要退出登录吗？", "确定", "取消");

        if (result)
        {
            await LogoutAsync();
        }
    }

    /// <summary>
    /// 退出登录
    /// </summary>
    public async Task LogoutAsync()
    {
        IsLoggedIn = false;
        CurrentUsername = null;
        UpdateLoginStatus(false);

        // 清除保存的凭据
        var credentialService = new Services.CredentialService();
        credentialService.ClearCredentials();

        // 显示登录页面
        var loginPage = new Views.LoginView();
        await Navigation.PushModalAsync(loginPage);
    }
}
