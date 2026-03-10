using iiCourse.Core;
using iiCourse.Core.Models;
using Microsoft.Maui.Controls.Shapes;

namespace iiCourseMAUI.Views;

public partial class EvaluationView : ContentPage
{
    private iiCoreService? _service;

    public EvaluationView()
    {
        InitializeComponent();
    }

    public void SetService(iiCoreService service)
    {
        _service = service;
    }

    public async Task LoadReviewsAsync()
    {
        if (_service == null)
        {
            ShowStatus("服务未初始化");
            return;
        }

        try
        {
            SetLoadingState(true);
            ShowStatus("正在加载评教列表...");

            var reviews = await _service.GetStudentReviewsAsync();

            if (reviews.Code == 200 && reviews.Data != null)
            {
                DisplayReviews(reviews.Data);
                ShowStatus($"已加载 {reviews.Data.Count} 个评教任务");
            }
            else
            {
                ShowEmptyState();
                ShowStatus(reviews.Message ?? "获取评教列表失败");
            }
        }
        catch (Exception ex)
        {
            ShowEmptyState();
            ShowStatus($"错误: {ex.Message}");
        }
        finally
        {
            SetLoadingState(false);
        }
    }

    private void DisplayReviews(List<StudentReview> reviews)
    {
        ReviewPanel.Children.Clear();

        foreach (var review in reviews)
        {
            var card = CreateReviewCard(review);
            ReviewPanel.Children.Add(card);
        }
    }

    private Border CreateReviewCard(StudentReview review)
    {
        var border = new Border
        {
            BackgroundColor = Color.FromArgb("#FFFDF5"),
            StrokeShape = new RoundRectangle { CornerRadius = 8 },
            Padding = new Thickness(12),
            Margin = new Thickness(0, 0, 0, 12)
        };

        var grid = new Grid
        {
            ColumnDefinitions = 
            {
                new ColumnDefinition { Width = GridLength.Auto },
                new ColumnDefinition { Width = GridLength.Star },
                new ColumnDefinition { Width = GridLength.Auto }
            }
        };

        var statusBadge = new Border
        {
            BackgroundColor = Color.FromArgb("#FF6B35"),
            StrokeShape = new RoundRectangle { CornerRadius = 6 },
            Padding = new Thickness(12, 6),
            Margin = new Thickness(0, 0, 16, 0),
            VerticalOptions = LayoutOptions.Center
        };

        statusBadge.Content = new Label
        {
            Text = "待评教",
            FontSize = 13,
            FontAttributes = FontAttributes.Bold,
            TextColor = Colors.White
        };
        grid.Children.Add(statusBadge);

        var infoStack = new StackLayout { Margin = new Thickness(0, 0, 16, 0), Spacing = 4 };
        infoStack.Children.Add(new Label
        {
            Text = $"{review.YearSemester} - {review.Category}",
            FontSize = 15,
            FontAttributes = FontAttributes.Bold,
            TextColor = Color.FromArgb("#333333")
        });
        infoStack.Children.Add(new Label
        {
            Text = $"批次: {review.Batch} | 类型: {review.CourseType}",
            FontSize = 13,
            TextColor = Color.FromArgb("#7F8C8D")
        });
        infoStack.Children.Add(new Label
        {
            Text = $"时间: {review.StartTime} ~ {review.EndTime}",
            FontSize = 12,
            TextColor = Color.FromArgb("#95A5A6")
        });
        grid.Children.Add(infoStack);
        Grid.SetColumn(infoStack, 1);

        var arrow = new Label
        {
            Text = "›",
            FontSize = 20,
            TextColor = Color.FromArgb("#BDC3C7"),
            VerticalOptions = LayoutOptions.Center
        };
        grid.Children.Add(arrow);
        Grid.SetColumn(arrow, 2);

        border.Content = grid;
        return border;
    }

    private void ShowEmptyState()
    {
        ReviewPanel.Children.Clear();

        var emptyBorder = new Border
        {
            BackgroundColor = Color.FromArgb("#F9F9F9"),
            StrokeShape = new RoundRectangle { CornerRadius = 8 },
            Padding = new Thickness(12)
        };

        var stack = new StackLayout { Spacing = 10 };
        stack.Children.Add(new Label { Text = "📋", FontSize = 32, HorizontalOptions = LayoutOptions.Center });
        stack.Children.Add(new Label
        {
            Text = "暂无评教任务",
            FontSize = 14,
            TextColor = Color.FromArgb("#888888"),
            HorizontalOptions = LayoutOptions.Center
        });

        emptyBorder.Content = stack;
        ReviewPanel.Children.Add(emptyBorder);
    }

    private async void OnLoadReviewsClicked(object? sender, EventArgs e)
    {
        await LoadReviewsAsync();
    }

    private async void OnFinishAllClicked(object? sender, EventArgs e)
    {
        if (_service == null)
        {
            ShowStatus("服务未初始化");
            return;
        }

        var result = await DisplayAlert("确认", "这将自动完成所有待评教任务，是否继续？", "确定", "取消");

        if (!result) return;

        try
        {
            SetLoadingState(true);
            ShowStatus("正在处理评教...");

            var finishResult = await _service.FinishStudentReviewsAsync();

            if (finishResult.Code == 200 && finishResult.Data != null)
            {
                var message = finishResult.Data.Count > 0
                    ? $"已完成 {finishResult.Data.Count} 个评教"
                    : "没有需要完成的评教";

                await DisplayAlert("成功", message, "确定");
                ShowStatus("评教完成");

                await LoadReviewsAsync();
            }
            else
            {
                await DisplayAlert("错误", finishResult.Message ?? "操作失败", "确定");
                ShowStatus(finishResult.Message ?? "操作失败");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("错误", $"错误: {ex.Message}", "确定");
            ShowStatus($"错误: {ex.Message}");
        }
        finally
        {
            SetLoadingState(false);
        }
    }

    private void ShowStatus(string message)
    {
        StatusLabel.Text = message;
    }

    private void SetLoadingState(bool isLoading)
    {
        LoadReviewsButton.IsEnabled = !isLoading;
        FinishAllButton.IsEnabled = !isLoading;
        LoadReviewsButton.Text = isLoading ? "加载中..." : "加载评教列表";
    }
}
