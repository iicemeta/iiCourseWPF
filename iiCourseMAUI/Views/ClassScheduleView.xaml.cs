using iiCourse.Core;
using iiCourse.Core.Models;
using Microsoft.Maui.Controls.Shapes;

namespace iiCourseMAUI.Views;

public partial class ClassScheduleView : ContentPage
{
    private iiCoreService? _service;
    private List<ClassInfo> _classes = new();
    private List<SelectedTimeClassInfo> _customClasses = new();
    private List<SchoolYearInfo> _schoolYears = new();
    private WeekDateInfo? _currentWeekDates;
    private int _selectedWeek = 1;
    private bool _isCustomQueryMode = false;

    private readonly List<Button> _weekButtons = new();
    private readonly string[] _dayNames = { "节", "一", "二", "三", "四", "五", "六", "日" };

    public ClassScheduleView()
    {
        InitializeComponent();
        InitializeScheduleGrid();
        InitializeWeekButtons();
    }

    public void SetService(iiCoreService service)
    {
        _service = service;
    }

    private void InitializeScheduleGrid()
    {
        ScheduleGrid.Children.Clear();

        for (int col = 0; col < 8; col++)
        {
            var headerBorder = new Border
            {
                BackgroundColor = Color.FromArgb("#FF6B35"),
                StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(col == 0 ? 6 : 0, col == 7 ? 6 : 0, 0, 0) },
                Padding = new Thickness(4, 8),
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Fill
            };

            var label = new Label
            {
                Text = _dayNames[col],
                FontSize = 10,
                FontAttributes = FontAttributes.Bold,
                TextColor = Colors.White,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            };

            headerBorder.Content = label;
            Grid.SetRow(headerBorder, 0);
            Grid.SetColumn(headerBorder, col);
            ScheduleGrid.Children.Add(headerBorder);
        }

        for (int row = 1; row <= 11; row++)
        {
            var cellBorder = new Border
            {
                BackgroundColor = Colors.White,
                Padding = new Thickness(2),
                MinimumHeightRequest = 50,
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Fill
            };

            var label = new Label
            {
                Text = row.ToString(),
                FontSize = 10,
                TextColor = Color.FromArgb("#7F8C8D"),
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            };

            cellBorder.Content = label;
            Grid.SetRow(cellBorder, row);
            Grid.SetColumn(cellBorder, 0);
            ScheduleGrid.Children.Add(cellBorder);
        }
    }

    private void InitializeWeekButtons()
    {
        WeekButtonsPanel.Children.Clear();
        _weekButtons.Clear();

        for (int i = 1; i <= 20; i++)
        {
            var button = new Button
            {
                Text = $"第{i}周",
                FontSize = 11,
                Padding = new Thickness(8, 5),
                MinimumWidthRequest = 52,
                HeightRequest = 28,
                BackgroundColor = Colors.White,
                TextColor = Color.FromArgb("#333333"),
                CornerRadius = 6,
                BorderColor = Color.FromArgb("#E0E0E0"),
                BorderWidth = 1
            };

            int week = i;
            button.Clicked += (s, e) => OnWeekButtonClicked(week);
            WeekButtonsPanel.Children.Add(button);
            _weekButtons.Add(button);
        }
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (AppShell.Instance.IsLoggedIn && _service != null)
        {
            await LoadSchoolYearsAsync();
            await LoadClassScheduleAsync();
        }
    }

    public async Task LoadSchoolYearsAsync()
    {
        if (_service == null) return;

        try
        {
            _schoolYears = await _service.GetSchoolYearsAsync();

            YearPicker.Items.Clear();
            YearPicker.Items.Add("请选择");

            foreach (var year in _schoolYears)
            {
                YearPicker.Items.Add(year.SCHOOL_YEAR);
            }

            YearPicker.SelectedIndex = 0;
        }
        catch (Exception ex)
        {
            ShowStatus($"加载学年列表失败: {ex.Message}");
        }
    }

    public async Task LoadClassScheduleAsync()
    {
        if (_service == null)
        {
            ShowStatus("服务未初始化");
            return;
        }

        try
        {
            SetLoadingState(true);
            ShowStatus("正在加载课程表...");

            _classes = await _service.GetClassInfoAsync();
            _isCustomQueryMode = false;

            if (_classes.Any())
            {
                DisplaySchedule();
                ShowStatus($"共加载 {_classes.Count} 门课程");
                DateRangeLabel.Text = "";
            }
            else
            {
                ShowStatus("暂无课程信息");
                ClearSchedule();
            }
        }
        catch (InvalidOperationException ex) when (ex.Message.Contains("假期"))
        {
            ShowStatus(ex.Message);
            ClearSchedule();
        }
        catch (Exception ex)
        {
            ShowStatus($"加载课程表失败: {ex.Message}");
            ClearSchedule();
        }
        finally
        {
            SetLoadingState(false);
        }
    }

    private void DisplaySchedule()
    {
        ClearSchedule();

        foreach (var classInfo in _classes)
        {
            if (int.TryParse(classInfo.SKXQ, out int weekday) &&
                int.TryParse(classInfo.SKJC, out int startPeriod))
            {
                int duration = 1;
                if (int.TryParse(classInfo.CXJC, out int parsedDuration))
                {
                    duration = parsedDuration;
                }

                int endPeriod = startPeriod + duration - 1;

                if (weekday >= 1 && weekday <= 7 &&
                    startPeriod >= 1 && startPeriod <= 11 &&
                    endPeriod <= 11)
                {
                    AddClassToGrid(classInfo.KCMC, classInfo.JXDD, classInfo.JSXM, weekday, startPeriod, endPeriod);
                }
            }
        }
    }

    private void AddClassToGrid(string courseName, string classroom, string teacher, int weekday, int startPeriod, int endPeriod)
    {
        var border = new Border
        {
            BackgroundColor = Color.FromArgb("#FFF5EB"),
            StrokeShape = new RoundRectangle { CornerRadius = 4 },
            Margin = new Thickness(1),
            Padding = new Thickness(4)
        };

        var stackLayout = new StackLayout { Spacing = 2 };

        stackLayout.Children.Add(new Label
        {
            Text = courseName,
            FontSize = 11,
            FontAttributes = FontAttributes.Bold,
            TextColor = Color.FromArgb("#2D5A3D"),
            LineBreakMode = LineBreakMode.WordWrap
        });

        stackLayout.Children.Add(new Label
        {
            Text = classroom,
            FontSize = 10,
            TextColor = Color.FromArgb("#646464"),
            LineBreakMode = LineBreakMode.WordWrap
        });

        stackLayout.Children.Add(new Label
        {
            Text = teacher,
            FontSize = 10,
            TextColor = Color.FromArgb("#646464"),
            LineBreakMode = LineBreakMode.WordWrap
        });

        border.Content = stackLayout;

        Grid.SetRow(border, startPeriod);
        Grid.SetRowSpan(border, endPeriod - startPeriod + 1);
        Grid.SetColumn(border, weekday);
        ScheduleGrid.Children.Add(border);
    }

    private void ClearSchedule()
    {
        var toRemove = ScheduleGrid.Children
            .OfType<VisualElement>()
            .Where(child => Grid.GetRow(child) > 0 && Grid.GetColumn(child) > 0)
            .ToList();

        foreach (var child in toRemove)
        {
            ScheduleGrid.Children.Remove(child);
        }
    }

    private async void OnWeekButtonClicked(int week)
    {
        _selectedWeek = week;
        UpdateWeekButtonSelection();

        if (YearPicker.SelectedIndex > 0 && SemesterPicker.SelectedIndex > 0)
        {
            await QueryCustomScheduleAsync();
        }
    }

    private void UpdateWeekButtonSelection()
    {
        for (int i = 0; i < _weekButtons.Count; i++)
        {
            if (i + 1 == _selectedWeek)
            {
                _weekButtons[i].BackgroundColor = Color.FromArgb("#FF6B35");
                _weekButtons[i].TextColor = Colors.White;
                _weekButtons[i].BorderColor = Color.FromArgb("#FF6B35");
            }
            else
            {
                _weekButtons[i].BackgroundColor = Colors.White;
                _weekButtons[i].TextColor = Color.FromArgb("#333333");
                _weekButtons[i].BorderColor = Color.FromArgb("#E0E0E0");
            }
        }
    }

    private async Task QueryCustomScheduleAsync()
    {
        if (_service == null)
        {
            ShowStatus("服务未初始化");
            return;
        }

        var schoolYear = YearPicker.SelectedIndex > 0 ? YearPicker.Items[YearPicker.SelectedIndex] : null;
        var semester = SemesterPicker.SelectedIndex > 0 ? SemesterPicker.SelectedIndex.ToString() : null;

        if (string.IsNullOrEmpty(schoolYear) || string.IsNullOrEmpty(semester))
        {
            ShowStatus("请选择学年和学期");
            return;
        }

        var parameters = new CustomQueryParams
        {
            SchoolYear = schoolYear,
            Semester = semester,
            LearnWeek = _selectedWeek.ToString()
        };

        try
        {
            SetLoadingState(true);
            ShowStatus($"正在查询第{_selectedWeek}周课程表...");

            var result = await _service.QueryCustomScheduleAsync(parameters);
            _customClasses = result.classes;
            _currentWeekDates = result.dates;
            _isCustomQueryMode = true;

            if (_customClasses.Any())
            {
                DisplayCustomSchedule();
                ShowStatus(result.message);

                if (_currentWeekDates != null)
                {
                    DateRangeLabel.Text = $"({_currentWeekDates.Date1} - {_currentWeekDates.Date7})";
                }
            }
            else
            {
                ShowStatus("该时间段暂无课程");
                ClearSchedule();
                DateRangeLabel.Text = "";
            }
        }
        catch (Exception ex)
        {
            ShowStatus($"查询失败: {ex.Message}");
            ClearSchedule();
        }
        finally
        {
            SetLoadingState(false);
        }
    }

    private void DisplayCustomSchedule()
    {
        ClearSchedule();

        foreach (var classInfo in _customClasses)
        {
            int weekday = classInfo.SKXQ;
            int startPeriod = classInfo.SKJC;
            int duration = classInfo.CXJC;
            int endPeriod = startPeriod + duration - 1;

            if (weekday >= 1 && weekday <= 7 &&
                startPeriod >= 1 && startPeriod <= 11 &&
                endPeriod <= 11)
            {
                AddClassToGrid(classInfo.KCMC, classInfo.JXDD, classInfo.JSXM, weekday, startPeriod, endPeriod);
            }
        }
    }

    private async void OnRefreshClicked(object? sender, EventArgs e)
    {
        if (_isCustomQueryMode)
        {
            await QueryCustomScheduleAsync();
        }
        else
        {
            await LoadClassScheduleAsync();
        }
    }

    private async void OnQueryClicked(object? sender, EventArgs e)
    {
        await QueryCustomScheduleAsync();
    }

    private void OnQueryParamChanged(object? sender, EventArgs e)
    {
    }

    private void ShowStatus(string message)
    {
        StatusLabel.Text = message;
    }

    private void SetLoadingState(bool isLoading)
    {
        RefreshButton.IsEnabled = !isLoading;
        QueryButton.IsEnabled = !isLoading;
        YearPicker.IsEnabled = !isLoading;
        SemesterPicker.IsEnabled = !isLoading;

        foreach (var button in _weekButtons)
        {
            button.IsEnabled = !isLoading;
        }
    }
}
