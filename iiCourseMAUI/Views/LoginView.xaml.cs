using iiCourse.Core;
using iiCourseMAUI.Services;

namespace iiCourseMAUI.Views;

/// <summary>
/// 登录视图
/// </summary>
public partial class LoginView : ContentPage
{
    private iiCoreService? _service;
    private readonly CredentialService _credentialService;

    public LoginView()
    {
        InitializeComponent();
        _credentialService = new CredentialService();
    }

    /// <summary>
    /// 页面出现时加载保存的凭据
    /// </summary>
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadSavedCredentialsAsync();
    }

    /// <summary>
    /// 加载保存的凭据
    /// </summary>
    private async Task LoadSavedCredentialsAsync()
    {
        var credentials = await _credentialService.LoadCredentialsAsync();
        if (credentials != null)
        {
            UsernameEntry.Text = credentials.Username;

            if (credentials.RememberPassword && !string.IsNullOrEmpty(credentials.Password))
            {
                PasswordEntry.Text = credentials.Password;
                RememberPasswordCheckBox.IsChecked = true;
            }
        }
    }

    /// <summary>
    /// 登录按钮点击事件
    /// </summary>
    private async void OnLoginClicked(object? sender, EventArgs e)
    {
        var username = UsernameEntry.Text?.Trim() ?? string.Empty;
        var password = PasswordEntry.Text ?? string.Empty;
        var rememberPassword = RememberPasswordCheckBox.IsChecked;

        if (string.IsNullOrEmpty(username))
        {
            ShowStatus("请输入学号", false);
            return;
        }

        if (string.IsNullOrEmpty(password))
        {
            ShowStatus("请输入密码", false);
            return;
        }

        await PerformLoginAsync(username, password, rememberPassword);
    }

    /// <summary>
    /// 执行登录操作
    /// </summary>
    private async Task PerformLoginAsync(string username, string password, bool rememberPassword)
    {
        try
        {
            SetLoadingState(true);
            HideStatus();

            _service = AppShell.Instance.Service;

            if (_service == null)
            {
                ShowStatus("服务未初始化", false);
                SetLoadingState(false);
                return;
            }

            var (success, message) = await _service.LoginAsync(username, password);

            SetLoadingState(false);

            if (success)
            {
                // 登录成功，保存凭据
                await _credentialService.SaveCredentialsAsync(username, password, rememberPassword);

                ShowStatus("登录成功!", true);
                await Task.Delay(500);

                // 更新全局登录状态
                AppShell.Instance.UpdateLoginStatus(true, username);

                // 关闭模态登录页面
                await Navigation.PopModalAsync();
            }
            else
            {
                ShowStatus($"登录失败: {message}", false);
            }
        }
        catch (Exception ex)
        {
            SetLoadingState(false);
            ShowStatus($"登录异常: {ex.Message}", false);
        }
    }

    /// <summary>
    /// 切换密码显示
    /// </summary>
    private void OnTogglePasswordClicked(object? sender, EventArgs e)
    {
        PasswordEntry.IsPassword = !PasswordEntry.IsPassword;
        TogglePasswordButton.Text = PasswordEntry.IsPassword ? "👁" : "👁‍🗨";
    }

    /// <summary>
    /// 点击记住密码标签
    /// </summary>
    private void OnRememberLabelTapped(object? sender, TappedEventArgs e)
    {
        RememberPasswordCheckBox.IsChecked = !RememberPasswordCheckBox.IsChecked;
    }

    /// <summary>
    /// 设置加载状态
    /// </summary>
    private void SetLoadingState(bool isLoading)
    {
        LoginButton.IsEnabled = !isLoading;
        UsernameEntry.IsEnabled = !isLoading;
        PasswordEntry.IsEnabled = !isLoading;
        RememberPasswordCheckBox.IsEnabled = !isLoading;
        LoadingPanel.IsVisible = isLoading;
    }

    /// <summary>
    /// 显示状态信息
    /// </summary>
    private void ShowStatus(string message, bool isSuccess)
    {
        StatusLabel.Text = message;
        StatusLabel.TextColor = isSuccess ? Color.FromArgb("#27AE60") : Color.FromArgb("#E74C3C");
        StatusLabel.IsVisible = true;
    }

    /// <summary>
    /// 隐藏状态信息
    /// </summary>
    private void HideStatus()
    {
        StatusLabel.IsVisible = false;
    }
}
