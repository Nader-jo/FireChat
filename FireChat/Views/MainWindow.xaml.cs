using System.ComponentModel;
using FireChat.Interfaces;
using System.Windows;
using FireChat.Models;

namespace FireChat.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ISecurityRepository _securityRepository;
		private readonly ICredentialsRepository _credentialsRepository;
		private readonly IUserRepository _userRepository;
		private readonly IMessagesRepository _messagesRepository;
		private User currentUser;
		private Credential currentUserCredentials;
		private bool IsAuthenticated;

		public MainWindow(ISecurityRepository securityRepository, ICredentialsRepository credentialsRepository, IUserRepository userRepository, IMessagesRepository messagesRepository)
        {
            InitializeComponent();
            _securityRepository = securityRepository;
            _credentialsRepository = credentialsRepository;
            _userRepository = userRepository;
            _messagesRepository = messagesRepository;
		}

        public void Authenticate(User user, Credential credential)
        {
			currentUser = user;
			currentUserCredentials = credential;
			IsAuthenticated = true;
        }

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            Opacity = 0.4;
            var loginWindow = new LoginWindow(_securityRepository, _credentialsRepository, _userRepository, this);
            loginWindow.Topmost = true;
			loginWindow.Show();
        }

        private void MainWindow_OnClosing(object? sender, CancelEventArgs e)
        {
            Application.Current.Shutdown();
        }

        public void LoadUserContacts()
        {
            UsernameTxt.Text = currentUser.Username;
		}
    }
}