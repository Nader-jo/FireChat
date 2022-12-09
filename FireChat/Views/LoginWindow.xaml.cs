using System.Windows;
using FireChat.Interfaces;
using FireChat.Models;

namespace FireChat.Views
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
		private readonly ISecurityRepository _securityRepository;
		private readonly ICredentialsRepository _credentialsRepository;
		private MainWindow _parentWindow;

        public LoginWindow(ISecurityRepository securityRepository, ICredentialsRepository credentialsRepository, MainWindow parentWindow)
        {
            InitializeComponent();
            _securityRepository = securityRepository;
            _credentialsRepository = credentialsRepository;
			_parentWindow = parentWindow;
        }

        private void RegisterButton_OnClick(object sender, RoutedEventArgs e)
        {
            var RegisterWindow = new RegisterWindow(_securityRepository, _parentWindow);
            RegisterWindow.Show();
            RegisterWindow.Topmost = true;
        }

        private async void LoginButton_OnClickAsync(object sender, RoutedEventArgs e)
        {
            var credential = new Credential()
            {
                Email = EmailText.Text,
                Password = PasswordText.Password
            };
            var result = await _securityRepository.Login(credential);

			if (result)
            {
				_credentialsRepository.SaveCredentials(credential);
				_parentWindow.Authenticate();
                Close();
            }
        }

        private void CancelButton_OnClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void LoginWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            var SavedCred = _credentialsRepository.LoadCredentials();
            EmailText.Text = SavedCred.Email;
            PasswordText.Password = SavedCred.Password;
			AutoLoginchk.IsChecked = SavedCred.AutoLogin;
		}
    }
}