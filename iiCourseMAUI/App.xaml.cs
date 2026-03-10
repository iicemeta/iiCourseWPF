using iiCourseMAUI.Services;

namespace iiCourseMAUI;

/// <summary>
/// 应用程序主入口
/// </summary>
public partial class App : Application
{
    private readonly CredentialService _credentialService;
    private Window? _mainWindow;

    public App()
    {
        InitializeComponent();
        _credentialService = new CredentialService();
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        _mainWindow = new Window(new AppShell());

        // 启动时检查登录状态
        _mainWindow.Created += async (s, e) =>
        {
            await CheckLoginStatusAsync();
        };

        return _mainWindow;
    }

    /// <summary>
    /// 检查登录状态
    /// </summary>
    private async Task CheckLoginStatusAsync()
    {
        try
        {
            // 等待 Shell 完全加载
            await Task.Delay(100);

            var hasCredentials = await _credentialService.HasSavedCredentialsAsync();

            if (!hasCredentials)
            {
                // 没有保存的凭据，显示登录页面
                await ShowLoginPageAsync();
            }
            else
            {
                // 有保存的凭据，尝试自动登录
                var credentials = await _credentialService.LoadCredentialsAsync();

                if (credentials != null && !string.IsNullOrEmpty(credentials.Username))
                {
                    if (credentials.RememberPassword && !string.IsNullOrEmpty(credentials.Password))
                    {
                        // 尝试自动登录
                        var success = await AttemptAutoLoginAsync(credentials.Username, credentials.Password);

                        if (!success)
                        {
                            await ShowLoginPageAsync();
                        }
                    }
                    else
                    {
                        // 只记住用户名，显示登录页面
                        await ShowLoginPageAsync();
                    }
                }
                else
                {
                    await ShowLoginPageAsync();
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"检查登录状态失败: {ex.Message}");
            await ShowLoginPageAsync();
        }
    }

    /// <summary>
    /// 显示登录页面
    /// </summary>
    private async Task ShowLoginPageAsync()
    {
        try
        {
            // 使用模态页面显示登录
            var loginPage = new Views.LoginView();
            await MainThread.InvokeOnMainThreadAsync(() =>
            {
                _mainWindow?.Page?.Navigation.PushModalAsync(loginPage);
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"显示登录页面失败: {ex.Message}");
        }
    }

    /// <summary>
    /// 尝试自动登录
    /// </summary>
    private async Task<bool> AttemptAutoLoginAsync(string username, string password)
    {
        try
        {
            var service = AppShell.Instance.Service;
            if (service == null) return false;

            var (success, _) = await service.LoginAsync(username, password);

            if (success)
            {
                // 自动登录成功
                AppShell.Instance.UpdateLoginStatus(true, username);
                return true;
            }

            return false;
        }
        catch
        {
            return false;
        }
    }
}
