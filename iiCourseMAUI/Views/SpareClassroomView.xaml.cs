using iiCourse.Core;
using iiCourse.Core.Models;
using Microsoft.Maui.Controls.Shapes;
using Microsoft.Maui.Layouts;

namespace iiCourseMAUI.Views;

public partial class SpareClassroomView : ContentPage
{
    private iiCoreService? _service;
    private List<SpareClassroom> _classrooms = new();
    private List<BuildingInfo> _buildings = new();
    private string? _selectedPeriod;
    private string? _currentCampusId;
    private string? _currentBuildingName;
    private Button? _currentCampusButton;
    private Button? _currentBuildingButton;

    public SpareClassroomView()
    {
        InitializeComponent();
    }

    public void SetService(iiCoreService service)
    {
        _service = service;
    }

    private async void OnCampusClicked(object? sender, EventArgs e)
    {
        if (sender is Button button && button.CommandParameter is string campusId)
        {
            UpdateCampusButtonStates(button);
            _currentCampusButton = button;
            _currentCampusId = campusId;

            await LoadBuildingsAsync(campusId);
        }
    }

    private async Task LoadBuildingsAsync(string campusId)
    {
        if (_service == null)
        {
            ShowStatus("服务未初始化");
            return;
        }

        try
        {
            SetLoadingState(true);
            ShowStatus("正在加载教学楼列表...");

            BuildingButtonsPanel.Children.Clear();
            _currentBuildingButton = null;

            _buildings = await _service.GetBuildingsAsync(campusId);

            if (_buildings.Count > 0)
            {
                foreach (var building in _buildings)
                {
                    var btn = new Button
                    {
                        Text = building.Name,
                        FontSize = 12,
                        Padding = new Thickness(12, 8),
                        BackgroundColor = Colors.White,
                        TextColor = Color.FromArgb("#333333"),
                        CornerRadius = 6,
                        BorderColor = Color.FromArgb("#E0E0E0"),
                        BorderWidth = 1,
                        CommandParameter = building.ID
                    };
                    btn.Clicked += OnBuildingClicked;
                    BuildingButtonsPanel.Children.Add(btn);
                }
                ShowStatus($"已加载 {_buildings.Count} 个教学楼");
            }
            else
            {
                ShowStatus("该校区暂无教学楼数据");
            }
        }
        catch (Exception ex)
        {
            ShowStatus($"加载失败: {ex.Message}");
        }
        finally
        {
            SetLoadingState(false);
        }
    }

    private async void OnBuildingClicked(object? sender, EventArgs e)
    {
        if (sender is Button button && button.CommandParameter is string buildingId)
        {
            if (int.TryParse(buildingId, out int id))
            {
                _currentBuildingButton = button;
                _selectedPeriod = null;
                _currentBuildingName = button.Text;
                await LoadSpareClassroomsAsync(id);
                UpdateBuildingButtonStates(button);
            }
        }
    }

    private async Task LoadSpareClassroomsAsync(int buildingId)
    {
        if (_service == null)
        {
            ShowStatus("服务未初始化");
            return;
        }

        try
        {
            SetLoadingState(true);
            ShowStatus("正在查询空教室...");

            _classrooms = await _service.GetSpareClassroomAsync(buildingId);

            if (_classrooms.Count > 0)
            {
                GeneratePeriodFilterButtons();
                DisplayClassroomsByPeriod();
                ShowStatus($"找到 {_classrooms.Count} 个空闲时段");
            }
            else
            {
                ShowEmptyState();
                PeriodFilterPanel.IsVisible = false;
                ShowStatus("暂无空教室");
            }
        }
        catch (Exception ex)
        {
            ShowStatus($"查询失败: {ex.Message}");
            ShowEmptyState();
            PeriodFilterPanel.IsVisible = false;
        }
        finally
        {
            SetLoadingState(false);
        }
    }

    private void GeneratePeriodFilterButtons()
    {
        PeriodFilterButtons.Children.Clear();
        PeriodFilterPanel.IsVisible = true;

        var availablePeriods = _classrooms
            .Select(c => c.Period)
            .Distinct()
            .OrderBy(p => int.TryParse(p, out var n) ? n : 999)
            .ToList();

        var allButton = CreatePeriodFilterButton("全部", null, true);
        PeriodFilterButtons.Children.Add(allButton);

        foreach (var period in availablePeriods)
        {
            var btn = CreatePeriodFilterButton($"第{period}节", period, false);
            PeriodFilterButtons.Children.Add(btn);
        }
    }

    private Button CreatePeriodFilterButton(string text, string? periodValue, bool isActive)
    {
        var button = new Button
        {
            Text = text,
            FontSize = 11,
            Padding = new Thickness(6, 4),
            MinimumWidthRequest = 52,
            HeightRequest = 28,
            BackgroundColor = isActive ? Color.FromArgb("#FF6B35") : Colors.White,
            TextColor = isActive ? Colors.White : Color.FromArgb("#333333"),
            CornerRadius = 6,
            BorderColor = isActive ? Color.FromArgb("#FF6B35") : Color.FromArgb("#E0E0E0"),
            BorderWidth = 1,
            CommandParameter = periodValue
        };

        button.Clicked += OnPeriodFilterClicked;
        return button;
    }

    private void OnPeriodFilterClicked(object? sender, EventArgs e)
    {
        if (sender is Button button)
        {
            _selectedPeriod = button.CommandParameter as string;

            foreach (var child in PeriodFilterButtons.Children)
            {
                if (child is Button btn)
                {
                    bool isActive = btn == button;
                    btn.BackgroundColor = isActive ? Color.FromArgb("#FF6B35") : Colors.White;
                    btn.TextColor = isActive ? Colors.White : Color.FromArgb("#333333");
                    btn.BorderColor = isActive ? Color.FromArgb("#FF6B35") : Color.FromArgb("#E0E0E0");
                }
            }

            DisplayClassroomsByPeriod();
        }
    }

    private void DisplayClassroomsByPeriod()
    {
        ClassroomPanel.Children.Clear();

        var filteredClassrooms = _selectedPeriod == null
            ? _classrooms
            : _classrooms.Where(c => c.Period == _selectedPeriod).ToList();

        var groupedByPeriod = filteredClassrooms
            .GroupBy(c => c.Period)
            .OrderBy(g => int.TryParse(g.Key, out var n) ? n : 999);

        int totalCount = 0;

        foreach (var periodGroup in groupedByPeriod)
        {
            var periodRow = CreatePeriodRow(periodGroup.Key, periodGroup.ToList());
            ClassroomPanel.Children.Add(periodRow);
            totalCount += periodGroup.Count();
        }

        ResultCountLabel.Text = _selectedPeriod == null
            ? $"共 {totalCount} 个空闲时段"
            : $"第{_selectedPeriod}节: {totalCount} 个教室";
    }

    private Border CreatePeriodRow(string period, List<SpareClassroom> classrooms)
    {
        var border = new Border
        {
            BackgroundColor = Colors.White,
            StrokeShape = new RoundRectangle { CornerRadius = 8 },
            Padding = new Thickness(12),
            Margin = new Thickness(0, 0, 0, 10)
        };

        var mainStack = new StackLayout();

        var headerGrid = new Grid
        {
            ColumnDefinitions = 
            {
                new ColumnDefinition { Width = GridLength.Auto },
                new ColumnDefinition { Width = GridLength.Star },
                new ColumnDefinition { Width = GridLength.Auto }
            }
        };

        var periodBadge = new Border
        {
            BackgroundColor = Color.FromArgb("#FF6B35"),
            StrokeShape = new RoundRectangle { CornerRadius = 6 },
            Padding = new Thickness(12, 6),
            Margin = new Thickness(0, 0, 12, 0),
            VerticalOptions = LayoutOptions.Center
        };

        string periodDisplay = GetPeriodDisplayText(period);
        periodBadge.Content = new Label
        {
            Text = periodDisplay,
            FontSize = 14,
            FontAttributes = FontAttributes.Bold,
            TextColor = Colors.White
        };

        headerGrid.Children.Add(periodBadge);
        Grid.SetColumn(periodBadge, 0);

        var countLabel = new Label
        {
            Text = $"{classrooms.Count} 个空闲教室",
            FontSize = 13,
            TextColor = Color.FromArgb("#7F8C8D"),
            Margin = new Thickness(12, 0, 0, 0),
            VerticalOptions = LayoutOptions.Center
        };
        headerGrid.Children.Add(countLabel);
        Grid.SetColumn(countLabel, 2);

        mainStack.Children.Add(headerGrid);

        var classroomGrid = new FlexLayout
        {
            Wrap = FlexWrap.Wrap,
            JustifyContent = FlexJustify.Start,
            Margin = new Thickness(0, 12, 0, 0)
        };

        foreach (var classroom in classrooms.OrderBy(c => c.ClassroomName))
        {
            var tag = new Border
            {
                BackgroundColor = Color.FromArgb("#E8F5E9"),
                StrokeShape = new RoundRectangle { CornerRadius = 6 },
                Padding = new Thickness(8, 6),
                Margin = new Thickness(4)
            };

            tag.Content = new Label
            {
                Text = classroom.ClassroomName,
                FontSize = 12,
                FontAttributes = FontAttributes.Bold,
                TextColor = Color.FromArgb("#2D5A3D")
            };

            classroomGrid.Children.Add(tag);
        }

        mainStack.Children.Add(classroomGrid);
        border.Content = mainStack;

        return border;
    }

    private string GetPeriodDisplayText(string period)
    {
        if (!int.TryParse(period, out int periodNumber))
            return $"第{period}节";

        if (!string.IsNullOrEmpty(_currentBuildingName))
        {
            return ClassTime.GetPeriodDisplayText(_currentBuildingName, periodNumber);
        }

        return ClassTime.GetPeriodDisplayText(periodNumber);
    }

    private void ShowEmptyState()
    {
        ClassroomPanel.Children.Clear();
        ResultCountLabel.Text = "";

        var emptyBorder = new Border
        {
            BackgroundColor = Color.FromArgb("#F9F9F9"),
            StrokeShape = new RoundRectangle { CornerRadius = 8 },
            Padding = new Thickness(12)
        };

        var stack = new StackLayout { Spacing = 10 };
        stack.Children.Add(new Label { Text = "🔍", FontSize = 32, HorizontalOptions = LayoutOptions.Center });
        stack.Children.Add(new Label
        {
            Text = "暂无空教室",
            FontSize = 14,
            TextColor = Color.FromArgb("#888888"),
            HorizontalOptions = LayoutOptions.Center
        });

        emptyBorder.Content = stack;
        ClassroomPanel.Children.Add(emptyBorder);
    }

    private void UpdateCampusButtonStates(Button activeButton)
    {
        EastCampusButton.BackgroundColor = Colors.White;
        EastCampusButton.TextColor = Color.FromArgb("#333333");
        WestCampusButton.BackgroundColor = Colors.White;
        WestCampusButton.TextColor = Color.FromArgb("#333333");

        activeButton.BackgroundColor = Color.FromArgb("#FF6B35");
        activeButton.TextColor = Colors.White;
    }

    private void UpdateBuildingButtonStates(Button activeButton)
    {
        foreach (var child in BuildingButtonsPanel.Children)
        {
            if (child is Button btn)
            {
                btn.BackgroundColor = Colors.White;
                btn.TextColor = Color.FromArgb("#333333");
            }
        }

        activeButton.BackgroundColor = Color.FromArgb("#FF6B35");
        activeButton.TextColor = Colors.White;
    }

    private void ShowStatus(string message)
    {
        StatusLabel.Text = message;
    }

    private void SetLoadingState(bool isLoading)
    {
        EastCampusButton.IsEnabled = !isLoading;
        WestCampusButton.IsEnabled = !isLoading;

        foreach (var child in BuildingButtonsPanel.Children)
        {
            if (child is Button btn)
            {
                btn.IsEnabled = !isLoading;
            }
        }
    }
}
