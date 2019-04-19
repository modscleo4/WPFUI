using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Modscleo4.WPFUI.Controls
{
    public class PlaceholderTextBox : Control
    {
        private Label labelPlaceholder;
        private TextBox textBox;

        #region Properties

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

        #endregion Properties

        #region Events

        public static readonly RoutedEvent ValueChangedEvent = EventManager.RegisterRoutedEvent("ValueChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(PlaceholderTextBox));

        public event RoutedEventHandler ValueChanged
        {
            add
            {
                AddHandler(ValueChangedEvent, value);
            }

            remove
            {
                RemoveHandler(ValueChangedEvent, value);
            }
        }

        protected virtual void RaiseValueChangedEvent()
        {
            RoutedEventArgs args = new RoutedEventArgs(ValueChangedEvent);
            RaiseEvent(args);
        }

        #endregion Events

        public PlaceholderTextBox() : base()
        {
            GotFocus += new RoutedEventHandler(PlaceholderTextBox_GotFocus);
        }

        private void PlaceholderTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (textBox != null)
            {
                textBox.Focus();
            }
        }

        static PlaceholderTextBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PlaceholderTextBox), new FrameworkPropertyMetadata(typeof(PlaceholderTextBox)));

            ValueProperty = DependencyProperty.Register("Value", typeof(string), typeof(PlaceholderTextBox), new FrameworkPropertyMetadata(String.Empty));
            PlaceholderProperty = DependencyProperty.Register("Placeholder", typeof(string), typeof(PlaceholderTextBox), new FrameworkPropertyMetadata(String.Empty));
        }

        public override void OnApplyTemplate()
        {
            if (GetTemplateChild("TextBox") is TextBox)
            {
                textBox = GetTemplateChild("TextBox") as TextBox;

                textBox.TextChanged += TextBox_TextChanged;
                textBox.GotFocus += TextBox_GotFocus;
                textBox.LostFocus += TextBox_LostFocus;
            }

            if (GetTemplateChild("LabelPlaceholder") is Label)
            {
                labelPlaceholder = GetTemplateChild("LabelPlaceholder") as Label;
            }

            base.OnApplyTemplate();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Value = textBox.Text;
            RaiseValueChangedEvent();
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            labelPlaceholder.Visibility = Visibility.Hidden;
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
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
