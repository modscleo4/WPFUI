using System.Windows;
using System.Windows.Controls;

namespace Modscleo4.WPFUI.Controls
{
    public class TitlebarButton : Button
    {
        static TitlebarButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TitlebarButton), new FrameworkPropertyMetadata(typeof(TitlebarButton)));
        }
    }
}
