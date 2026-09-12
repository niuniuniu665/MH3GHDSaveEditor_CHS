using MaterialDesignThemes.Wpf;
using MH3GHDSaveEditor.Pages.Main;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace MH3GHDSaveEditor
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private Point _dragStartPoint;
        private bool _isDragging;

        // ===== 标题栏：按下只记录起点，不做还原 =====
        private void TitleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // 双击：最大化/还原切换
            if (e.ClickCount == 2)
            {
                ToggleMaximize();
                return;
            }

            _dragStartPoint = e.GetPosition(this);
            _isDragging = false;
        }

        // ===== 标题栏：真正移动超过阈值后，才开始拖动 =====
        private void TitleBar_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton != MouseButtonState.Pressed || _isDragging)
                return;

            var current = e.GetPosition(this);

            // 未超过系统拖动阈值 → 视为单击，不做任何事
            if (Math.Abs(current.X - _dragStartPoint.X) < SystemParameters.MinimumHorizontalDragDistance &&
                Math.Abs(current.Y - _dragStartPoint.Y) < SystemParameters.MinimumVerticalDragDistance)
                return;

            _isDragging = true;

            // 最大化状态下开始拖动：先还原并把窗口定位到鼠标下方
            if (WindowState == WindowState.Maximized)
            {
                var mouseScreen = PointToScreen(current);
                var percentX = current.X / ActualWidth;

                WindowState = WindowState.Normal;

                // PointToScreen 返回设备像素，需按 DPI 换算成 WPF 逻辑坐标
                var source = PresentationSource.FromVisual(this);
                double dpiX = 1, dpiY = 1;
                if (source?.CompositionTarget != null)
                {
                    dpiX = source.CompositionTarget.TransformToDevice.M11;
                    dpiY = source.CompositionTarget.TransformToDevice.M22;
                }
                var mouseDip = new Point(mouseScreen.X / dpiX, mouseScreen.Y / dpiY);

                Left = mouseDip.X - Width * percentX;
                Top = mouseDip.Y - _dragStartPoint.Y;

                // 防止窗口跑到屏幕外
                var wa = SystemParameters.WorkArea;
                Top = Math.Max(Top, wa.Top);
                Left = Math.Max(Left, wa.Left);
            }

            // DragMove 会阻塞直到松开左键
            DragMove();
            _isDragging = false;
        }

        private void Minimize_Click(object sender, RoutedEventArgs e)
            => WindowState = WindowState.Minimized;

        private void MaximizeRestore_Click(object sender, RoutedEventArgs e)
            => ToggleMaximize();

        private void Close_Click(object sender, RoutedEventArgs e)
            => Close();

        private void ToggleMaximize()
        {
            WindowState = WindowState == WindowState.Maximized
                ? WindowState.Normal
                : WindowState.Maximized;
        }

        private void Window_StateChanged(object sender, EventArgs e)
        {
            if (MaxIcon == null) return;

            MaxIcon.Kind = WindowState == WindowState.Maximized
                ? PackIconKind.WindowRestore
                : PackIconKind.WindowMaximize;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            string savedThemeChoice = Properties.Settings.Default.ThemeChoice;

            switch (savedThemeChoice)
            {
                case "Inherit":
                    SystemThemeMenuItem.IsChecked = true;
                    ApplyBaseTheme(BaseTheme.Inherit); // 确保应用的是 Inherit 模式
                    break;
                case "Dark":
                    DarkThemeMenuItem.IsChecked = true;
                    ApplyBaseTheme(BaseTheme.Dark);
                    break;
                case "Light":
                default:
                    LightThemeMenuItem.IsChecked = true;
                    ApplyBaseTheme(BaseTheme.Light);
                    break;
            }

            // 恢复主色
            if (!string.IsNullOrEmpty(Properties.Settings.Default.PrimaryColor))
            {
                var color = (Color)ColorConverter.ConvertFromString(Properties.Settings.Default.PrimaryColor);
                ApplyPrimaryColor(color);
            }
            else
            {
                UpdatePrimaryColorCheckState(_paletteHelper.GetTheme().PrimaryMid.Color);
            }

            UpdateDataGridBrushes();
        }

        private void LightThemeMenuItem_Click(object sender, RoutedEventArgs e)
        {
            ApplyBaseTheme(BaseTheme.Light);
        }

        private void DarkThemeMenuItem_Click(object sender, RoutedEventArgs e)
        {
            ApplyBaseTheme(BaseTheme.Dark);
        }
        private void SystemThemeMenuItem_Click(object sender, RoutedEventArgs e)
        {
            ApplyBaseTheme(BaseTheme.Inherit);
        }

        private void ApplyBaseTheme(BaseTheme baseTheme)
        {
            var theme = _paletteHelper.GetTheme();
            theme.SetBaseTheme(baseTheme);
            _paletteHelper.SetTheme(theme);

            // 更新菜单勾选状态
            LightThemeMenuItem.IsChecked = baseTheme == BaseTheme.Light;
            DarkThemeMenuItem.IsChecked = baseTheme == BaseTheme.Dark;
            SystemThemeMenuItem.IsChecked = baseTheme == BaseTheme.Inherit;

            UpdateDataGridBrushes();

            // 保存用户选择
            Properties.Settings.Default.ThemeChoice = baseTheme.ToString();
            Properties.Settings.Default.Save();
        }

        private void PrimaryColor_Click(object sender, RoutedEventArgs e)
        {
            if (!(sender is MenuItem item) || !(item.Tag is string hex))
                return;

            var color = (Color)ColorConverter.ConvertFromString(hex);
            ApplyPrimaryColor(color);
        }

        private void PrimaryColorCustom_Click(object sender, RoutedEventArgs e)
        {
            var picker = new MaterialDesignThemes.Wpf.ColorPicker
            {
                Color = _paletteHelper.GetTheme().PrimaryMid.Color,
            };

            var dlg = new Window
            {
                Title = "选择主色",
                Width = 320,
                Height = 240,
                Owner = this,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Content = picker
            };

            // 简单起见，双击颜色即应用
            picker.MouseDoubleClick += (s, args) =>
            {
                ApplyPrimaryColor(picker.Color);
                dlg.Close();
            };

            dlg.ShowDialog();
        }

        private void AutoBackupCheckBox_Click(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox checkBox)
            {
                Properties.Settings.Default.IsAutoBackup = (bool)checkBox.IsChecked;
                Properties.Settings.Default.Save();
            }
        }

        private void AutoBackupCheckBox_Initialized(object sender, EventArgs e)
        {
            if (sender is CheckBox checkBox)
            {
                checkBox.IsChecked = Properties.Settings.Default.IsAutoBackup;
            }
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (IsReadSaveData)
            {
                var result = MessageBox.Show("是否保存所做的修改?", "提示", MessageBoxButton.YesNo, MessageBoxImage.Information);

                if (result == MessageBoxResult.Yes)
                {
                    MainPage.CloseAndSaveData();
                }
            }
        }
    }
}
