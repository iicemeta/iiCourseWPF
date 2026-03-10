using iiCourse.Core;
using Microsoft.Maui.Controls.Shapes;
using Newtonsoft.Json.Linq;

namespace iiCourseMAUI.Views;

public partial class ScoreView : ContentPage
{
    private iiCoreService? _service;

    public ScoreView()
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
            await LoadScoresAsync();
        }
    }

    public async Task LoadScoresAsync()
    {
        if (_service == null)
        {
            ShowStatus("服务未初始化");
            return;
        }

        try
        {
            SetLoadingState(true);
            ShowStatus("正在加载成绩...");

            var scoreData = await _service.GetExamScoreAsync();

            if (!string.IsNullOrEmpty(scoreData))
            {
                DisplayScores(scoreData);
                ShowStatus("成绩加载完成");
            }
            else
            {
                ShowStatus("暂无成绩数据");
                ShowEmptyState();
            }
        }
        catch (Exception ex)
        {
            ShowStatus($"加载成绩失败: {ex.Message}");
            ShowEmptyState();
        }
        finally
        {
            SetLoadingState(false);
        }
    }

    private void DisplayScores(string scoreData)
    {
        ScoreGrid.Children.Clear();
        ScoreGrid.RowDefinitions.Clear();

        AddHeaderRow();

        try
        {
            var jsonArray = JArray.Parse(scoreData);
            int index = 0;

            foreach (var item in jsonArray)
            {
                AddScoreRow(item, index);
                index++;
            }

            if (index == 0)
            {
                ShowEmptyState();
            }
        }
        catch
        {
            ShowEmptyState();
        }
    }

    private void AddHeaderRow()
    {
        ScoreGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

        var headers = new[] { "课程名称", "学分", "成绩", "绩点", "性质", "学期" };
        for (int col = 0; col < headers.Length; col++)
        {
            var border = new Border
            {
                BackgroundColor = Color.FromArgb("#FF6B35"),
                Padding = new Thickness(10, 8),
                HorizontalOptions = LayoutOptions.Fill
            };

            var label = new Label
            {
                Text = headers[col],
                FontSize = 11,
                FontAttributes = FontAttributes.Bold,
                TextColor = Colors.White,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            };

            border.Content = label;
            Grid.SetRow(border, 0);
            Grid.SetColumn(border, col);
            ScoreGrid.Children.Add(border);
        }
    }

    private void AddScoreRow(JToken item, int index)
    {
        int rowIndex = index + 1;
        ScoreGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

        var courseName = item["COURSENAME"]?.ToString() ?? "--";
        var credit = item["CREDIT"]?.ToString() ?? "--";
        var score = item["SCORE_NUMERIC"]?.ToString() ?? "--";
        var gpa = "--";
        var nature = item["EXAMPROPERTY"]?.ToString() ?? "--";
        var year = item["XN"]?.ToString() ?? "--";
        var semester = item["XQ"]?.ToString() ?? "--";

        var values = new[] { courseName, credit, score, gpa, nature, $"{year}-{semester}" };

        for (int col = 0; col < values.Length; col++)
        {
            var border = new Border
            {
                BackgroundColor = index % 2 == 0 ? Colors.White : Color.FromArgb("#FAFAFA"),
                Padding = new Thickness(10, 10),
                HorizontalOptions = LayoutOptions.Fill
            };

            var label = new Label
            {
                Text = values[col],
                FontSize = 13,
                TextColor = col == 2 ? GetScoreColor(score) : Color.FromArgb("#333333"),
                HorizontalOptions = col == 0 ? LayoutOptions.Start : LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                LineBreakMode = LineBreakMode.WordWrap
            };

            if (col == 2) label.FontAttributes = FontAttributes.Bold;

            border.Content = label;
            Grid.SetRow(border, rowIndex);
            Grid.SetColumn(border, col);
            ScoreGrid.Children.Add(border);
        }
    }

    private Color GetScoreColor(string score)
    {
        if (double.TryParse(score, out double scoreValue))
        {
            if (scoreValue >= 90) return Color.FromArgb("#4CAF50");
            if (scoreValue >= 80) return Color.FromArgb("#2196F3");
            if (scoreValue >= 60) return Color.FromArgb("#FF9800");
            return Color.FromArgb("#F44336");
        }
        return Color.FromArgb("#333333");
    }

    private void ShowEmptyState()
    {
        ScoreGrid.Children.Clear();
        ScoreGrid.RowDefinitions.Clear();
        AddHeaderRow();

        ScoreGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

        var border = new Border
        {
            BackgroundColor = Colors.White,
            Padding = new Thickness(20)
        };
        Grid.SetColumnSpan(border, 6);

        var label = new Label
        {
            Text = "暂无成绩数据",
            FontSize = 13,
            TextColor = Color.FromArgb("#888888"),
            HorizontalOptions = LayoutOptions.Center
        };

        border.Content = label;
        Grid.SetRow(border, 1);
        ScoreGrid.Children.Add(border);
    }

    private async void OnRefreshClicked(object? sender, EventArgs e)
    {
        await LoadScoresAsync();
    }

    private void ShowStatus(string message)
    {
        StatusLabel.Text = message;
    }

    private void SetLoadingState(bool isLoading)
    {
        RefreshButton.IsEnabled = !isLoading;
        RefreshButton.Text = isLoading ? "加载中..." : "刷新成绩";
    }
}
