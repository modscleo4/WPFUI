using System.Windows;
using System.Windows.Controls;

namespace Modscleo4.WPFUI.Controls
{
    public class MenuItem : Control, IMenuItem
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

        #region Click Event

        public static readonly RoutedEvent ClickEvent;

        public event RoutedEventHandler Click
        {
            add
            {
                AddHandler(ClickEvent, value);
            }

            remove
            {
                RemoveHandler(ClickEvent, value);
            }
        }

        protected virtual void RaiseClickEvent()
        {
            RoutedEventArgs args = new RoutedEventArgs(ClickEvent);
            RaiseEvent(args);
        }

        #endregion Click Event

        private TitlebarButton Button;

        static MenuItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MenuItem), new FrameworkPropertyMetadata(typeof(MenuItem)));

            ClickEvent = EventManager.RegisterRoutedEvent("Click", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(MenuItem));

            TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(MenuItem), new FrameworkPropertyMetadata(string.Empty));
        }

        public override void OnApplyTemplate()
        {
            if (GetTemplateChild("Button") is TitlebarButton)
            {
                Button = GetTemplateChild("Button") as TitlebarButton;

                Button.Click += new RoutedEventHandler(Button_Click);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            RaiseClickEvent();
        }
    }
}
