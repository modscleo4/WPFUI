using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace Modscleo4.WPFUI
{
    /// <summary>
    /// Lógica interna para DialogWindow.xaml
    /// </summary>
    public partial class DialogWindow : Controls.Window
    {
        private readonly Window main = Application.Current.MainWindow;

        public MessageBoxResult Result { get; private set; }

        public DialogWindow(Window parent, string message, string title, MessageBoxButton messageBoxButton, MessageBoxImage messageBoxImage) : base()
        {
            InitializeComponent();

            Closing += new CancelEventHandler(DialogWindow_Closing);

            Owner = parent;
            Title = title;
            LabelContent.Text = message;

            if (Owner == null)
            {
                WindowStartupLocation = WindowStartupLocation.CenterScreen;
            }

            if (messageBoxButton == MessageBoxButton.OK || messageBoxButton == MessageBoxButton.OKCancel)
            {
                ButtonOk.Visibility = Visibility.Visible;

                ButtonOk.Click += new RoutedEventHandler(ButtonOk_Click);
                ButtonOk.Focus();
            }

            if (messageBoxButton == MessageBoxButton.YesNo || messageBoxButton == MessageBoxButton.YesNoCancel)
            {
                ButtonYes.Visibility = Visibility.Visible;
                ButtonNo.Visibility = Visibility.Visible;
                ButtonNo.Focus();

                ButtonYes.Click += new RoutedEventHandler(ButtonYes_Click);
                ButtonNo.Click += new RoutedEventHandler(ButtonNo_Click);
            }

            if (messageBoxButton == MessageBoxButton.OKCancel || messageBoxButton == MessageBoxButton.YesNoCancel)
            {
                ButtonCancel.Visibility = Visibility.Visible;
                ButtonCancel.Focus();

                ButtonCancel.Click += new RoutedEventHandler(ButtonCancel_Click);
            }

            if (messageBoxImage != MessageBoxImage.None)
            {
                Image.Visibility = Visibility.Visible;
                switch (messageBoxImage)
                {
                    case MessageBoxImage.Information:
                        Image.Source = Imaging.CreateBitmapSourceFromHIcon(System.Drawing.SystemIcons.Information.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
                        System.Media.SystemSounds.Asterisk.Play();
                        break;
                    case MessageBoxImage.Question:
                        Image.Source = Imaging.CreateBitmapSourceFromHIcon(System.Drawing.SystemIcons.Question.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
                        System.Media.SystemSounds.Question.Play();
                        break;
                    case MessageBoxImage.Exclamation:
                        Image.Source = Imaging.CreateBitmapSourceFromHIcon(System.Drawing.SystemIcons.Exclamation.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
                        System.Media.SystemSounds.Exclamation.Play();
                        break;
                    case MessageBoxImage.Error:
                        Image.Source = Imaging.CreateBitmapSourceFromHIcon(System.Drawing.SystemIcons.Error.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
                        System.Media.SystemSounds.Hand.Play();
                        break;
                }
            }

            ShowDialog();
        }

        private void DialogWindow_Closing(object sender, CancelEventArgs e)
        {
            Application.Current.MainWindow = main;
        }

        private void ButtonOk_Click(object sender, RoutedEventArgs e)
        {
            Result = MessageBoxResult.OK;
            Close();
        }

        private void ButtonYes_Click(object sender, RoutedEventArgs e)
        {
            Result = MessageBoxResult.Yes;
            Close();
        }

        private void ButtonNo_Click(object sender, RoutedEventArgs e)
        {
            Result = MessageBoxResult.No;
            Close();
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            Result = MessageBoxResult.Cancel;
            Close();
        }
    }
}
