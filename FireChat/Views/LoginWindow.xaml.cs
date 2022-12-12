using System.Threading.Tasks;
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
        private readonly IUserRepository _userRepository;
        private readonly MainWindow _parentWindow;
        private User _user;
        private Credential _userCredential;

        public LoginWindow(ISecurityRepository securityRepository, ICredentialsRepository credentialsRepository,
            IUserRepository userRepository, MainWindow parentWindow)
        {
            InitializeComponent();
            _securityRepository = securityRepository;
            _credentialsRepository = credentialsRepository;
            _userRepository = userRepository;
            _parentWindow = parentWindow;
        }

        private void RegisterButton_OnClick(object sender, RoutedEventArgs e)
        {
            var registerWindow = new RegisterWindow(_securityRepository, _parentWindow, this);
            registerWindow.Show();
            registerWindow.Topmost = true;
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
                if (_user is null)
                {
                    _user = await _userRepository.GetByEmail(EmailText.Text);
                }

                _parentWindow.Authenticate(_user, credential);
                _parentWindow.LoadUserContacts();
                _parentWindow.Opacity = 1;
                Close();
            }
        }

        private void CancelButton_OnClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void LoginWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            var savedCred = _credentialsRepository.LoadCredentials();
            EmailText.Text = savedCred.Email;
            PasswordText.Password = savedCred.Password;
            AutoLoginchk.IsChecked = savedCred.AutoLogin;
        }

        public async Task SetUserAndCredentials(User newUser, Credential credential)
        {
            await SetUser(newUser);
            EmailText.Text = credential.Email;
            PasswordText.Password = credential.Password;
            _userCredential = credential;
            LoginButton_OnClickAsync(new object(), new RoutedEventArgs());
        }

        internal async Task SetUser(User newUser)
        {
            try
            {
                _user = await _userRepository.GetByEmail(newUser.Email);
                if (_user == null)
                {
                    await _userRepository.Add(newUser);
                    _user = newUser;
                }
            }
            catch
            {
                // ignored
            }
        }
    }
}