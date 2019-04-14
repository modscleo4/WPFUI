using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Modscleo4.WPFUI.Controls
{
    public class MenuBarItem : Control
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

        static MenuBarItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MenuBarItem), new FrameworkPropertyMetadata(typeof(MenuBarItem)));

            TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(Window), new FrameworkPropertyMetadata(String.Empty));
        }
    }
}
