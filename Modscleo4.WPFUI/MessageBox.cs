using Modscleo4.WPFUI.Controls;
using System.Windows;

namespace Modscleo4.WPFUI
{
    public static class MessageBox
    {
        private static DialogWindow dialog;

        #region Show (no parent)

        public static MessageBoxResult Show(string message)
        {
            return Show(Application.Current.MainWindow, message, "", MessageBoxButton.OK, MessageBoxImage.None);
        }

        public static MessageBoxResult Show(string message, string title)
        {
            return Show(Application.Current.MainWindow, message, title, MessageBoxButton.OK, MessageBoxImage.None);
        }

        public static MessageBoxResult Show(string message, string title, MessageBoxButton messageBoxButton)
        {
            return Show(Application.Current.MainWindow, message, title, messageBoxButton, MessageBoxImage.None);
        }

        public static MessageBoxResult Show(string message, string title, MessageBoxButton messageBoxButton, MessageBoxImage messageBoxImage)
        {
            return Show(Application.Current.MainWindow, message, title, messageBoxButton, messageBoxImage);
        }

        #endregion Show (no parent)

        #region Show (parent)

        public static MessageBoxResult Show(System.Windows.Window owner, string message)
        {
            return Show(owner, message, "", MessageBoxButton.OK, MessageBoxImage.None);
        }

        public static MessageBoxResult Show(System.Windows.Window owner, string message, string title)
        {
            return Show(owner, message, title, MessageBoxButton.OK, MessageBoxImage.None);
        }

        public static MessageBoxResult Show(System.Windows.Window owner, string message, string title, MessageBoxButton messageBoxButton)
        {
            return Show(owner, message, title, messageBoxButton, MessageBoxImage.None);
        }

        public static MessageBoxResult Show(System.Windows.Window owner, string message, string title, MessageBoxButton messageBoxButton, MessageBoxImage messageBoxImage)
        {
            dialog = new DialogWindow(owner, message, title, messageBoxButton, messageBoxImage);
            return dialog.Result;
        }

        #endregion Show (parent)
    }
}
