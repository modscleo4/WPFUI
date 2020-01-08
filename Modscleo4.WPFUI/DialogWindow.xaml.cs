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
        private readonly string message;
        private readonly MessageBoxButton messageBoxButton;
        private readonly MessageBoxImage messageBoxImage;

        public MessageBoxResult Result { get; private set; }

        public DialogWindow(Window parent, string message, string title, MessageBoxButton messageBoxButton, MessageBoxImage messageBoxImage) : base()
        {
            InitializeComponent();

            Owner = parent;
            Title = title;

            this.message = message;
            this.messageBoxButton = messageBoxButton;
            this.messageBoxImage = messageBoxImage;

            Loaded += new RoutedEventHandler(DialogWindow_Loaded);
            Closing += new CancelEventHandler(DialogWindow_Closing);

            ShowDialog();
        }

        public override void OnApplyTemplate()
        {
            LabelContent.Text = message;

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

            base.OnApplyTemplate();
        }

        private void DialogWindow_Closing(object sender, CancelEventArgs e)
        {
            Application.Current.MainWindow = main;
        }

        private void DialogWindow_Loaded(object sender, RoutedEventArgs e)
        {
            SizeToContent = SizeToContent.WidthAndHeight;
            UpdateLayout();

            // Manually center the MessageBox
            if (Owner != null)
            {
                if (Owner.WindowState == WindowState.Maximized)
                {
                    Left = (Owner.ActualWidth - ActualWidth) / 2;
                    Top = (Owner.ActualHeight - ActualHeight) / 2;
                }
                else
                {
                    Left = Owner.Left + (Owner.ActualWidth - ActualWidth) / 2;
                    Top = Owner.Top + (Owner.ActualHeight - ActualHeight) / 2;
                }
            }
            else
            {
                CenterScreen();
            }
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
