using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using static Modscleo4.WPFUI.NativeMethods;

namespace Modscleo4.WPFUI.Controls
{
    public class Window : System.Windows.Window
    {
        #region Theme Color

        private static Color ThemeColor
        {
            get
            {
                if (WinBuild() >= 17134)
                {
                    var regBaseKey = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Registry64);
                    var regKey = regBaseKey.OpenSubKey(@"Software\Microsoft\Windows\DWM", RegistryKeyPermissionCheck.ReadSubTree);
                    if (regKey != null)
                    {
                        var value = regKey.GetValue("ColorPrevalence");
                        if (value != null && !Convert.ToBoolean(value))
                        {
                            return Color.FromArgb(0xFF, 0x40, 0x40, 0x40);
                        }
                    }
                }

                var dwmParams = new DwmColorizationParams();
                DwmGetColorizationParameters(ref dwmParams);

                return Color.FromArgb(
                        (byte)(dwmParams.ColorizationColor >> 24),
                        (byte)(dwmParams.ColorizationColor >> 16),
                        (byte)(dwmParams.ColorizationColor >> 8),
                        (byte)(dwmParams.ColorizationColor >> 0));
            }
        }

        private static readonly DependencyProperty ThemeColorBrushProperty;

        public SolidColorBrush ThemeColorBrush
        {
            get
            {
                return (SolidColorBrush)GetValue(ThemeColorBrushProperty);
            }

            set
            {
                SetValue(ThemeColorBrushProperty, value);
            }
        }

        #endregion Theme Color

        #region Blur

        private uint _blurOpacity;
        public double BlurOpacity
        {
            get
            {
                return _blurOpacity;
            }

            set
            {
                _blurOpacity = (uint)value;
                Blur();
            }
        }

        private readonly uint _blurBackgroundColor = 0x550000; /* BGR color format */

        internal void Blur(bool enable = true)
        {
            var WinBuild = NativeMethods.WinBuild();
            if (WinBuild == 7600 || WinBuild == 7601)
            {
                var bb = new DwmBlurbehind
                {
                    dwFlags = DwmBlurBehindDwFlags.DwmBbEnable,
                    Enabled = true
                };

                var hwnd = new WindowInteropHelper(this).Handle;

                HwndSource.FromHwnd(hwnd).CompositionTarget.BackgroundColor = Colors.Transparent;

                DwmEnableBlurBehindWindow(hwnd, ref bb);

                const int dwmwaNcrenderingPolicy = 2;
                var dwmncrpDisabled = 2;

                DwmSetWindowAttribute(hwnd, dwmwaNcrenderingPolicy, ref dwmncrpDisabled, sizeof(int));
            }
            else if (WinBuild >= 17134)
            {
                var windowHelper = new WindowInteropHelper(this);

                var accent = new AccentPolicy
                {
                    AccentState = (enable) ? AccentState.ACCENT_ENABLE_BLURBEHIND : AccentState.ACCENT_DISABLED,
                    AccentFlags = AccentFlags.DrawNoBorder,
                    GradientColor = (_blurOpacity << 24) | (_blurBackgroundColor & 0xFFFFFF),
                    AnimationId = 0
                };

                var accentStructSize = Marshal.SizeOf(accent);

                var accentPtr = Marshal.AllocHGlobal(accentStructSize);
                Marshal.StructureToPtr(accent, accentPtr, false);

                var data = new WindowCompositionAttributeData
                {
                    Attribute = WindowCompositionAttribute.WCA_ACCENT_POLICY,
                    SizeOfData = accentStructSize,
                    Data = accentPtr
                };

                SetWindowCompositionAttribute(windowHelper.Handle, ref data);

                Marshal.FreeHGlobal(accentPtr);
            }
        }

        #endregion Blur

        #region Startup Location

        public static readonly DependencyProperty WindowStartupLocationProperty;
        public new WindowStartupLocation WindowStartupLocation
        {
            get
            {
                return (WindowStartupLocation)GetValue(WindowStartupLocationProperty);
            }

            set
            {
                SetValue(WindowStartupLocationProperty, value);
                base.WindowStartupLocation = value;
            }
        }

        #endregion Startup Location

        #region Searchbox

        public static readonly DependencyProperty SearchboxVisibilityProperty;
        public Visibility SearchboxVisibility
        {
            get
            {
                return (Visibility)GetValue(SearchboxVisibilityProperty);
            }

            set
            {
                SetValue(SearchboxVisibilityProperty, value);
            }
        }

        public static readonly DependencyProperty SearchboxValueProperty;
        public string SearchboxValue
        {
            get
            {
                return (string)GetValue(SearchboxValueProperty);
            }

            set
            {
                SetValue(SearchboxValueProperty, value);
            }
        }

        public static readonly DependencyProperty SearchboxPlaceholderProperty;
        public string SearchboxPlaceholder
        {
            get
            {
                return (string)GetValue(SearchboxPlaceholderProperty);
            }

            set
            {
                SetValue(SearchboxPlaceholderProperty, value);
            }
        }

        #endregion Searchbox

        #region Search Event

        public static readonly RoutedEvent SearchEvent;
        public event RoutedEventHandler Search
        {
            add
            {
                AddHandler(SearchEvent, value);
            }

            remove
            {
                RemoveHandler(SearchEvent, value);
            }
        }

        protected virtual void RaiseSearchEvent()
        {
            RoutedEventArgs args = new RoutedEventArgs(SearchEvent);
            RaiseEvent(args);
        }

        #endregion Search Event

        #region Extra Buttons

        public static readonly DependencyProperty ExtraButtonsProperty;
        public List<TitlebarButton> ExtraButtons
        {
            get
            {
                return (List<TitlebarButton>)GetValue(ExtraButtonsProperty);
            }

            set
            {
                SetValue(ExtraButtonsProperty, value);
            }
        }

        #endregion Extra Buttons

        #region MenuBar

        public static readonly DependencyProperty MenuBarProperty;
        public List<IMenuItem> MenuBar
        {
            get
            {
                return (List<IMenuItem>)GetValue(MenuBarProperty);
            }

            set
            {
                SetValue(MenuBarProperty, value);
            }
        }

        #endregion MenuBar

        #region Show App Icon

        public static readonly DependencyProperty ShowIconProperty;
        public Visibility ShowIcon
        {
            get
            {
                return (Visibility)GetValue(ShowIconProperty);
            }

            set
            {
                SetValue(ShowIconProperty, value);
            }
        }

        #endregion MenuBar

        private const int WM_DWMCOLORIZATIONCOLORCHANGED = 0x320;

        private IntPtr hwnd;
        private HwndSource hsource;

        private Button BtnMinimize;
        private Button BtnMaximize;
        private Button BtnRestore;
        private Button BtnClose;
        private Image WindowIcon;
        private SearchBox Searchbox;

        public Window() : base()
        {
            ThemeColorBrush = new SolidColorBrush(ThemeColor);
            ExtraButtons = new List<TitlebarButton>();
            MenuBar = new List<IMenuItem>();

            SourceInitialized += new EventHandler(Window_SourceInitialized);
            Activated += new EventHandler(Window_Activated);
            Closing += new CancelEventHandler(Window_Closing);
        }

        static Window()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Window), new FrameworkPropertyMetadata(typeof(Window)));
            ResizeModeProperty.OverrideMetadata(typeof(Window), new FrameworkPropertyMetadata(ResizeMode.CanResize));

            ThemeColorBrushProperty = DependencyProperty.Register("ThemeColorBrush", typeof(SolidColorBrush), typeof(Window), new FrameworkPropertyMetadata(null));

            WindowStartupLocationProperty = DependencyProperty.Register("WindowStartupLocation", typeof(WindowStartupLocation), typeof(Window), new FrameworkPropertyMetadata(WindowStartupLocation.CenterScreen));

            SearchEvent = EventManager.RegisterRoutedEvent("Search", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(Window));

            SearchboxVisibilityProperty = DependencyProperty.Register("SearchboxVisibility", typeof(Visibility), typeof(Window), new FrameworkPropertyMetadata(Visibility.Collapsed));
            SearchboxValueProperty = DependencyProperty.Register("SearchboxValue", typeof(string), typeof(Window), new FrameworkPropertyMetadata(string.Empty));
            SearchboxPlaceholderProperty = DependencyProperty.Register("SearchboxPlaceholder", typeof(string), typeof(Window), new FrameworkPropertyMetadata("Search"));

            ExtraButtonsProperty = DependencyProperty.Register("ExtraButtons", typeof(List<TitlebarButton>), typeof(Window), new FrameworkPropertyMetadata(null));
            MenuBarProperty = DependencyProperty.Register("MenuBar", typeof(List<IMenuItem>), typeof(Window), new FrameworkPropertyMetadata(null));

            ShowIconProperty = DependencyProperty.Register("ShowIcon", typeof(Visibility), typeof(Window), new FrameworkPropertyMetadata(Visibility.Visible));
        }

        private void Window_SourceInitialized(object sender, EventArgs e)
        {
            if ((hwnd = new WindowInteropHelper(this).Handle) == IntPtr.Zero)
            {
                throw new InvalidOperationException("Could not get window handle.");
            }

            hsource = HwndSource.FromHwnd(hwnd);
            hsource.AddHook(WndProc);
        }

        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            switch (msg)
            {
                case WM_DWMCOLORIZATIONCOLORCHANGED:
                    ThemeColorBrush = new SolidColorBrush(ThemeColor);
                    return IntPtr.Zero;
                default:
                    return IntPtr.Zero;
            }
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            if (!(this is DialogWindow))
            {
                Application.Current.MainWindow = this;
            }

            Blur(true);
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (!(this is DialogWindow))
            {
                Activate();
            }
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.WindowStartupLocation = WindowStartupLocation;

            base.OnInitialized(e);
        }

        public override void OnApplyTemplate()
        {
            if (GetTemplateChild("BtnMinimize") is Button)
            {
                BtnMinimize = GetTemplateChild("BtnMinimize") as Button;

                BtnMinimize.Click += new RoutedEventHandler(BtnMinimize_Click);
            }

            if (GetTemplateChild("BtnMaximize") is Button)
            {
                BtnMaximize = GetTemplateChild("BtnMaximize") as Button;

                BtnMaximize.Click += new RoutedEventHandler(BtnMaximize_Click);
            }

            if (GetTemplateChild("BtnRestore") is Button)
            {
                BtnRestore = GetTemplateChild("BtnRestore") as Button;

                BtnRestore.Click += new RoutedEventHandler(BtnRestore_Click);
            }

            if (GetTemplateChild("BtnClose") is Button)
            {
                BtnClose = GetTemplateChild("BtnClose") as Button;

                BtnClose.Click += new RoutedEventHandler(BtnClose_Click);
            }

            if (GetTemplateChild("WindowIcon") is Image)
            {
                WindowIcon = GetTemplateChild("WindowIcon") as Image;

                if (Icon == null)
                {
                    WindowIcon.Source = Imaging.CreateBitmapSourceFromHIcon(System.Drawing.Icon.ExtractAssociatedIcon(
                                            System.Reflection.Assembly.GetEntryAssembly().ManifestModule.Name).Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
                }
                else
                {
                    WindowIcon.Source = Icon;
                }
            }

            if (GetTemplateChild("Searchbox") is SearchBox)
            {
                Searchbox = GetTemplateChild("Searchbox") as SearchBox;

                Searchbox.Search += new RoutedEventHandler(Searchbox_Search);
            }

            base.OnApplyTemplate();
        }

        public void CenterScreen()
        {
            Rect area = SystemParameters.WorkArea;
            Left = area.Left + (area.Width - ActualWidth) / 2;
            Top = area.Top + (area.Height - ActualHeight) / 2;
        }

        public void CenterOwner()
        {
            if (Owner.WindowState == WindowState.Maximized)
            {
                Left = (Owner.ActualWidth - ActualWidth) / 2;
                Top = (Owner.ActualHeight - ActualHeight) / 2;
            }
            else
            {
                Left = Owner.Left + (Owner.ActualWidth - ActualWidth) / 2;
                Top = Owner.Top + (Owner.ActualHeight - ActualHeight) / 2;
            }
        }

        private void Searchbox_Search(object sender, RoutedEventArgs e)
        {
            SearchboxValue = Searchbox.Value;
            RaiseSearchEvent();
        }

        private void BtnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void BtnMaximize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Maximized;
        }

        private void BtnRestore_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Normal;
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
