using System.Windows;
using System.Windows.Controls;

namespace Modscleo4.WPFUI.Controls
{
    public class TitlebarLabel : Label
    {
        static TitlebarLabel()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TitlebarLabel), new FrameworkPropertyMetadata(typeof(TitlebarLabel)));
        }
    }
}
