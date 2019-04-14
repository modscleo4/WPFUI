using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Modscleo4.WPFUI.Controls
{
    public class SearchBox : Control
    {
        #region Searchbox

        public static readonly DependencyProperty ValueProperty;
        public string Value
        {
            get
            {
                return (string)GetValue(ValueProperty);
            }

            set
            {
                SetValue(ValueProperty, value);
            }
        }

        public static readonly DependencyProperty PlaceholderProperty;
        public string Placeholder
        {
            get
            {
                return (string)GetValue(PlaceholderProperty);
            }

            set
            {
                SetValue(PlaceholderProperty, value);
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

        private Button BtnSearch;
        private PlaceholderTextBox Searchbox;

        static SearchBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SearchBox), new FrameworkPropertyMetadata(typeof(SearchBox)));

            SearchEvent = EventManager.RegisterRoutedEvent("Search", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(SearchBox));

            ValueProperty = DependencyProperty.Register("Value", typeof(string), typeof(SearchBox), new FrameworkPropertyMetadata(String.Empty));
            PlaceholderProperty = DependencyProperty.Register("Placeholder", typeof(string), typeof(SearchBox), new FrameworkPropertyMetadata("Search"));
        }

        public override void OnApplyTemplate()
        {
            if (GetTemplateChild("BtnSearch") is Button)
            {
                BtnSearch = GetTemplateChild("BtnSearch") as Button;

                BtnSearch.Click += new RoutedEventHandler(BtnSearch_Click);
            }

            if (GetTemplateChild("Searchbox") is PlaceholderTextBox)
            {
                Searchbox = GetTemplateChild("Searchbox") as PlaceholderTextBox;

                Searchbox.KeyDown += new KeyEventHandler(Searchbox_KeyDown);
                Searchbox.ValueChanged += new RoutedEventHandler(Searchbox_ValueChanged);
            }
        }

        private void Searchbox_ValueChanged(object sender, RoutedEventArgs e)
        {
            Value = Searchbox.Value;
        }

        private void Searchbox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                RaiseSearchEvent();
            }
        }

        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            RaiseSearchEvent();
        }
    }
}
