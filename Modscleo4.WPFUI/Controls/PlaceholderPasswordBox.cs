using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Modscleo4.WPFUI.Controls
{
    public class PlaceholderPasswordBox : Control
    {
        private Label labelPlaceholder;
        private PasswordBox passwordBox;

        public static readonly DependencyProperty ValueProperty;
        public static readonly DependencyProperty PlaceholderProperty;

        public string Placeholder
        {
            get
            {
                return GetValue(PlaceholderProperty).ToString();
            }

            set
            {
                SetValue(PlaceholderProperty, value);
            }
        }

        public string Value
        {
            get
            {
                return GetValue(ValueProperty).ToString();
            }

            set
            {
                SetValue(ValueProperty, value);
            }
        }

        public PlaceholderPasswordBox() : base()
        {
            GotFocus += new RoutedEventHandler(PlaceholderPasswordBox_GotFocus);
        }

        private void PlaceholderPasswordBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (passwordBox != null)
            {
                passwordBox.Focus();
            }
        }

        static PlaceholderPasswordBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PlaceholderPasswordBox), new FrameworkPropertyMetadata(typeof(PlaceholderPasswordBox)));

            ValueProperty = DependencyProperty.Register("Value", typeof(string), typeof(PlaceholderPasswordBox), new FrameworkPropertyMetadata(String.Empty));
            PlaceholderProperty = DependencyProperty.Register("Placeholder", typeof(string), typeof(PlaceholderPasswordBox), new FrameworkPropertyMetadata(String.Empty));
        }

        public override void OnApplyTemplate()
        {
            if (GetTemplateChild("PasswordBox") is PasswordBox)
            {
                passwordBox = GetTemplateChild("PasswordBox") as PasswordBox;

                passwordBox.PasswordChanged += PasswordBox_PasswordChanged;
                passwordBox.GotFocus += PasswordBox_GotFocus;
                passwordBox.LostFocus += PasswordBox_LostFocus;
            }

            if (GetTemplateChild("LabelPlaceholder") is Label)
            {
                labelPlaceholder = GetTemplateChild("LabelPlaceholder") as Label;
            }

            base.OnApplyTemplate();
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            Value = passwordBox.Password;
        }

        private void PasswordBox_GotFocus(object sender, RoutedEventArgs e)
        {
            labelPlaceholder.Visibility = Visibility.Hidden;
        }

        private void PasswordBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Value.Length == 0)
            {
                labelPlaceholder.Visibility = Visibility.Visible;
            }
            else
            {
                labelPlaceholder.Visibility = Visibility.Hidden;
            }
        }
    }
}
