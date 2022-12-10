using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using FireChat.Interfaces;
using FireChat.Models;
using FireChat.Repositories;

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
		private MainWindow _parentWindow;
		private User user;
		private Credential userCredential;

		public LoginWindow(ISecurityRepository securityRepository, ICredentialsRepository credentialsRepository, IUserRepository userRepository, MainWindow parentWindow)
		{
			InitializeComponent();
			_securityRepository = securityRepository;
			_credentialsRepository = credentialsRepository;
			_userRepository = userRepository;
			_parentWindow = parentWindow;
		}

		private void RegisterButton_OnClick(object sender, RoutedEventArgs e)
		{
			var RegisterWindow = new RegisterWindow(_securityRepository, _parentWindow, this);
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
				if (user is null)
				{
					user = await _userRepository.GetByEmail(EmailText.Text);
				}
				_parentWindow.Authenticate(user, credential);
				//_parentWindow.LoadUserContacts();
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
			var SavedCred = _credentialsRepository.LoadCredentials();
			EmailText.Text = SavedCred.Email;
			PasswordText.Password = SavedCred.Password;
			AutoLoginchk.IsChecked = SavedCred.AutoLogin;
		}

		public async Task SetUserAndCredentials(User newUser, Credential credential)
		{
			await SetUser(newUser);
			EmailText.Text = credential.Email;
			PasswordText.Password = credential.Password;
			userCredential = credential;
			LoginButton_OnClickAsync(new object(), new RoutedEventArgs());
		}

		internal async Task SetUser(User newUser)
		{
			try
			{
				user = await _userRepository.GetByEmail(newUser.Email);
				if (user == null)
				{
					await _userRepository.Add(newUser);
					user = newUser;
				}
			}
			catch
			{
			}

		}
	}
}