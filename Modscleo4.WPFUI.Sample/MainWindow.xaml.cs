using System.ComponentModel;
using System.Windows;

namespace Modscleo4.WPFUI.Sample
{
    /// <summary>
    /// Interação lógica para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Controls.Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to exit?", "WPFUI Sample", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result != MessageBoxResult.Yes)
            {
                e.Cancel = true;
            }
        }

        private void btnSettings_Click(object sender, RoutedEventArgs e)
        {
            new MainWindow().ShowDialog();
        }
    }
}
