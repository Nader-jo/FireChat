using System.ComponentModel;
using FireChat.Interfaces;
using System.Windows;

namespace FireChat.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ISecurityRepository _securityRepository;
		private readonly ICredentialsRepository _credentialsRepository;
		private bool IsAuthenticated;

        public MainWindow(ISecurityRepository securityRepository, ICredentialsRepository credentialsRepository)
        {
            InitializeComponent();
            _securityRepository = securityRepository;
            _credentialsRepository = credentialsRepository;

		}

        public void Authenticate()
        {
            IsAuthenticated = true;
        }

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            Opacity = 0.4;
            var loginWindow = new LoginWindow(_securityRepository, _credentialsRepository, this);
            loginWindow.Topmost = true;
			loginWindow.Show();
        }

        private void MainWindow_OnClosing(object? sender, CancelEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}