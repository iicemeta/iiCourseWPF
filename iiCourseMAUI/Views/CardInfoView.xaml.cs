using iiCourse.Core;
using iiCourse.Core.Models;

namespace iiCourseMAUI.Views;

public partial class CardInfoView : ContentPage
{
    private iiCoreService? _service;

    public CardInfoView()
    {
        InitializeComponent();
    }

    public void SetService(iiCoreService service)
    {
        _service = service;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (AppShell.Instance.IsLoggedIn && _service != null)
        {
            await LoadCardInfoAsync();
        }
    }

    public async Task LoadCardInfoAsync()
    {
        if (_service == null)
        {
            ShowStatus("服务未初始化");
            return;
        }

        try
        {
            SetLoadingState(true);
            ShowStatus("正在加载一卡通信息...");

            var cardInfo = await _service.GetCardInfoAsync();

            if (cardInfo != null)
            {
                DisplayCardInfo(cardInfo);
                ShowStatus("加载完成");
            }
            else
            {
                ShowError("获取一卡通信息失败");
            }
        }
        catch (Exception ex)
        {
            ShowError($"加载失败: {ex.Message}");
        }
        finally
        {
            SetLoadingState(false);
        }
    }

    private void DisplayCardInfo(CardInfo cardInfo)
    {
        BalanceLabel.Text = cardInfo.Balance;
        LastConsumeLabel.Text = !string.IsNullOrEmpty(cardInfo.LastConsumeTime)
            ? cardInfo.LastConsumeTime
            : "暂无消费记录";
    }

    private void ShowError(string message)
    {
        BalanceLabel.Text = "--";
        LastConsumeLabel.Text = "--";
        ShowStatus(message);
    }

    private void ShowStatus(string message)
    {
        StatusLabel.Text = message;
    }

    private void SetLoadingState(bool isLoading)
    {
        RefreshButton.IsEnabled = !isLoading;
        RefreshButton.Text = isLoading ? "加载中..." : "刷新信息";
    }

    private async void OnRefreshClicked(object? sender, EventArgs e)
    {
        await LoadCardInfoAsync();
    }
}
