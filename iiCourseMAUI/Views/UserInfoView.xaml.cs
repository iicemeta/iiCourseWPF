using iiCourse.Core;
using iiCourse.Core.Models;

namespace iiCourseMAUI.Views;

/// <summary>
/// 用户信息视图
/// </summary>
public partial class UserInfoView : ContentPage
{
    private iiCoreService? _service;
    private string? _username;

    public UserInfoView()
    {
        InitializeComponent();
    }

    /// <summary>
    /// 设置服务实例
    /// </summary>
    public void SetService(iiCoreService service)
    {
        _service = service;
    }

    /// <summary>
    /// 设置用户名
    /// </summary>
    public void SetUsername(string username)
    {
        _username = username;
    }

    /// <summary>
    /// 页面出现时加载数据
    /// </summary>
    protected override async void OnAppearing()
    {
        base.OnAppearing();

        // 检查是否已登录
        if (AppShell.Instance.IsLoggedIn && _service != null)
        {
            _username = AppShell.Instance.CurrentUsername;
            await LoadUserInfoAsync();
        }
    }

    /// <summary>
    /// 加载用户信息
    /// </summary>
    public async Task LoadUserInfoAsync()
    {
        if (_service == null || string.IsNullOrEmpty(_username))
        {
            ShowError("服务未初始化或用户名未设置");
            return;
        }

        try
        {
            var userInfo = await _service.GetUserInfoAsync(_username);

            if (userInfo != null)
            {
                DisplayUserInfo(userInfo);
            }
            else
            {
                ShowError("获取用户信息失败");
            }
        }
        catch (Exception ex)
        {
            ShowError($"加载用户信息错误: {ex.Message}");
        }
    }

    /// <summary>
    /// 显示用户信息
    /// </summary>
    private void DisplayUserInfo(UserInfo userInfo)
    {
        NameLabel.Text = userInfo.Name;
        CollegeLabel.Text = userInfo.College;
        StudentIdLabel.Text = userInfo.StudentId;
        NameDetailLabel.Text = userInfo.Name;
        GenderLabel.Text = userInfo.Gender;
        CollegeDetailLabel.Text = userInfo.College;
        LoginStatusLabel.Text = "已登录";
    }

    /// <summary>
    /// 显示错误
    /// </summary>
    private void ShowError(string message)
    {
        NameLabel.Text = "--";
        CollegeLabel.Text = "--";
        StudentIdLabel.Text = "--";
        NameDetailLabel.Text = "--";
        GenderLabel.Text = "--";
        CollegeDetailLabel.Text = "--";
        LoginStatusLabel.Text = "加载失败";
    }
}
