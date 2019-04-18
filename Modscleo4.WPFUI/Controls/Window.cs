using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;
using static Modscleo4.WPFUI.NativeMethods;

namespace Modscleo4.WPFUI.Controls
{
    public class Window : System.Windows.Window
    {
        #region Theme Color

        public Color ThemeColor
        {
            get
            {
                var colorSetEx = GetImmersiveColorFromColorSetEx((uint)GetImmersiveUserColorSetPreference(false, false), GetImmersiveColorTypeFromName(Marshal.StringToHGlobalUni("ImmersiveStartSelectionBackground")), false, 0);

                return Color.FromArgb(
                    (byte)((0xFF000000 & colorSetEx) >> 24),
                    (byte)((0x000000FF & colorSetEx) >> 0),
                    (byte)((0x0000FF00 & colorSetEx) >> 8),
                    (byte)((0x00FF0000 & colorSetEx) >> 16));
            }
        }

        public SolidColorBrush ThemeColorBrush
        {
            get
            {
                return new SolidColorBrush(ThemeColor);
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
            var windowHelper = new WindowInteropHelper(this);

            var accent = new AccentPolicy
            {
                AccentState = (enable) ? AccentState.ACCENT_ENABLE_ACRYLICBLURBEHIND : AccentState.ACCENT_DISABLED,
                AccentFlags = 2,
                GradientColor = (_blurOpacity << 24) | (_blurBackgroundColor & 0xFFFFFF)
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

        #region ExtraButtons

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

        #endregion ExtraButtons

        #region MenuBar

        public static readonly DependencyProperty MenuBarProperty;
        public List<TitlebarButton> MenuBar
        {
            get
            {
                return (List<TitlebarButton>)GetValue(MenuBarProperty);
            }

            set
            {
                SetValue(MenuBarProperty, value);
            }
        }

        #endregion MenuBar

        private Button BtnMinimize;
        private Button BtnMaximize;
        private Button BtnRestore;
        private Button BtnClose;
        private SearchBox Searchbox;

        public Window() : base()
        {
            Loaded += new RoutedEventHandler(Window_Loaded);

            WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        static Window()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Window), new FrameworkPropertyMetadata(typeof(Window)));
            ResizeModeProperty.OverrideMetadata(typeof(Window), new FrameworkPropertyMetadata(ResizeMode.CanMinimize));

            WindowStartupLocationProperty = DependencyProperty.Register("WindowStartupLocation", typeof(WindowStartupLocation), typeof(Window), new FrameworkPropertyMetadata(WindowStartupLocation.CenterScreen));

            SearchEvent = EventManager.RegisterRoutedEvent("Search", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(Window));

            SearchboxVisibilityProperty = DependencyProperty.Register("SearchboxVisibility", typeof(Visibility), typeof(Window), new FrameworkPropertyMetadata(Visibility.Collapsed));
            SearchboxValueProperty = DependencyProperty.Register("SearchboxValue", typeof(string), typeof(Window), new FrameworkPropertyMetadata(String.Empty));
            SearchboxPlaceholderProperty = DependencyProperty.Register("SearchboxPlaceholder", typeof(string), typeof(Window), new FrameworkPropertyMetadata("Search"));

            ExtraButtonsProperty = DependencyProperty.Register("ExtraButtons", typeof(List<TitlebarButton>), typeof(Window), new FrameworkPropertyMetadata(new List<TitlebarButton>()));
            MenuBarProperty = DependencyProperty.Register("MenuBar", typeof(List<TitlebarButton>), typeof(Window), new FrameworkPropertyMetadata(new List<TitlebarButton>()));
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

            if (GetTemplateChild("Searchbox") is SearchBox)
            {
                Searchbox = GetTemplateChild("Searchbox") as SearchBox;

                Searchbox.Search += new RoutedEventHandler(Searchbox_Search);
            }

            base.OnApplyTemplate();
        }

        private void Searchbox_Search(object sender, RoutedEventArgs e)
        {
            SearchboxValue = Searchbox.Value;
            RaiseSearchEvent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Blur(true);
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
