using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace Modscleo4.WPFUI.Controls
{
    public class DialogWindow : Control
    {
        private readonly System.Windows.Window main;
        private readonly Window window;
        private readonly string message;
        private readonly MessageBoxButton messageBoxButton;
        private readonly MessageBoxImage messageBoxImage;

        public MessageBoxResult Result { get; private set; }

        public DialogWindow(System.Windows.Window parent, string message, string title, MessageBoxButton messageBoxButton, MessageBoxImage messageBoxImage) : base()
        {
            main = Application.Current.MainWindow;

            window = new Window
            {
                ResizeMode = ResizeMode.NoResize,
                Owner = parent,
                Title = title,
                Content = this,
                Width = 0,
                Height = 0,
                MaxWidth = 640
            };

            this.message = message;
            this.messageBoxButton = messageBoxButton;
            this.messageBoxImage = messageBoxImage;

            window.ShowDialog();
        }

        static DialogWindow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DialogWindow), new FrameworkPropertyMetadata(typeof(DialogWindow)));
        }

        public override void OnApplyTemplate()
        {
            if (GetTemplateChild("LabelContent") is TextBlock LabelContent)
            {
                LabelContent.Text = message;
            }

            if (GetTemplateChild("ButtonOk") is Button ButtonOk)
            {
                if (messageBoxButton == MessageBoxButton.OK || messageBoxButton == MessageBoxButton.OKCancel)
                {
                    ButtonOk.Visibility = Visibility.Visible;

                    ButtonOk.Click += new RoutedEventHandler(ButtonOk_Click);
                    ButtonOk.Focus();
                }
            }

            if (GetTemplateChild("ButtonYes") is Button ButtonYes && GetTemplateChild("ButtonNo") is Button ButtonNo)
            {
                if (messageBoxButton == MessageBoxButton.YesNo || messageBoxButton == MessageBoxButton.YesNoCancel)
                {
                    ButtonYes.Visibility = Visibility.Visible;
                    ButtonNo.Visibility = Visibility.Visible;
                    ButtonNo.Focus();

                    ButtonYes.Click += new RoutedEventHandler(ButtonYes_Click);
                    ButtonNo.Click += new RoutedEventHandler(ButtonNo_Click);
                }
            }

            if (GetTemplateChild("ButtonCancel") is Button ButtonCancel)
            {
                if (messageBoxButton == MessageBoxButton.OKCancel || messageBoxButton == MessageBoxButton.YesNoCancel)
                {
                    ButtonCancel.Visibility = Visibility.Visible;
                    ButtonCancel.Focus();

                    ButtonCancel.Click += new RoutedEventHandler(ButtonCancel_Click);
                }
            }

            if (GetTemplateChild("Image") is Image Image)
            {
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
            }

            window.Loaded += new RoutedEventHandler(Window_Loaded);
            window.Closing += new CancelEventHandler(Window_Closing);

            base.OnApplyTemplate();
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (Application.Current.MainWindow == window)
            {
                Application.Current.MainWindow = main;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            window.SizeToContent = SizeToContent.WidthAndHeight;
            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;

            // Manually center the MessageBox
            if (window.Owner != null)
            {
                if (window.Owner.WindowState == WindowState.Maximized)
                {
                    window.Left = (window.Owner.Width - window.ActualWidth) / 2;
                    window.Top = (window.Owner.Height - window.ActualHeight) / 2;
                }
                else
                {
                    window.Left = window.Owner.Left + (window.Owner.Width - window.ActualWidth) / 2;
                    window.Top = window.Owner.Top + (window.Owner.Height - window.ActualHeight) / 2;
                }
            }
        }

        private void Close()
        {
            window.Close();
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
