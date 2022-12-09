using System.Windows;

namespace FireChat.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            Opacity = 0.4; 
            var loginWindow = new LoginWindow
            {
                Topmost = true
            };
            loginWindow.Topmost = false;
            loginWindow.Show();
        }
    }
}
