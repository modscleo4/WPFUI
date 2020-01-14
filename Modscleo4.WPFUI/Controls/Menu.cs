using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Modscleo4.WPFUI.Controls
{
    public class Menu : ItemsControl, IMenuItem
    {
        #region Text

        public static readonly DependencyProperty TextProperty;
        public string Text
        {
            get
            {
                return (string)GetValue(TextProperty);
            }

            set
            {
                SetValue(TextProperty, value);
            }
        }

        #endregion Text

        #region Is Open

        public static readonly DependencyProperty IsOpenProperty;

        public bool IsOpen
        {
            get
            {
                return (bool)GetValue(IsOpenProperty);
            }

            set
            {
                SetValue(IsOpenProperty, value);
            }
        }

        #endregion Is Open

        private MenuItem Title;
        private MenuItem PopupClose;
        private Popup Popup;

        static Menu()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Menu), new FrameworkPropertyMetadata(typeof(Menu)));

            TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(Menu), new FrameworkPropertyMetadata(string.Empty));
            IsOpenProperty = DependencyProperty.Register("IsOpen", typeof(bool), typeof(Menu), new FrameworkPropertyMetadata(false));
        }

        public override void OnApplyTemplate()
        {
            if (GetTemplateChild("Title") is MenuItem)
            {
                Title = GetTemplateChild("Title") as MenuItem;

                Title.Click += new RoutedEventHandler(Title_Click);
            }

            if (GetTemplateChild("PopupClose") is MenuItem)
            {
                PopupClose = GetTemplateChild("PopupClose") as MenuItem;

                PopupClose.Click += new RoutedEventHandler(PopupClose_Click);
            }

            if (GetTemplateChild("Popup") is Popup)
            {
                Popup = GetTemplateChild("Popup") as Popup;
            }

            base.OnApplyTemplate();
        }

        private void Title_Click(object sender, RoutedEventArgs e)
        {
            IsOpen = true;
        }

        private void PopupClose_Click(object sender, RoutedEventArgs e)
        {
            IsOpen = false;
        }
    }
}
