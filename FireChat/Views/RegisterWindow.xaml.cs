using FireChat.Interfaces;
using FireChat.Models;
using System.Collections.Generic;
using System.Windows;

namespace FireChat.Views
{
    /// <summary>
    /// Interaction logic for RegisterWindow.xaml
    /// </summary>
    public partial class RegisterWindow : Window
    {
		private readonly ISecurityRepository _securityRepository;
		private MainWindow _parentWindow;
		private LoginWindow _loginWindow;

		public RegisterWindow(ISecurityRepository securityRepository, MainWindow parentWindow, LoginWindow loginWindow)
        {
            InitializeComponent();
            _securityRepository = securityRepository;
            _parentWindow = parentWindow;
            _loginWindow = loginWindow;
		}

        private (bool, string) ValidateData()
        {
            // check 1 empty strings
            if (string.IsNullOrEmpty(EmailText.Text) || string.IsNullOrEmpty(UserNameText.Text) ||
                string.IsNullOrEmpty(Password1Text.Password) || string.IsNullOrEmpty(Password2Text.Password))
            {
                return (false, "All fields are required, Please fill them all");
            }

            // check 2 passwords are not identical
            if (Password1Text.Password != Password2Text.Password)
            {
                return (false, "Password not matching!");
            }

			// check 4 username is used

			return (true, string.Empty);
        }

        private async void RegisterButton_OnClick(object sender, RoutedEventArgs e)
        {
            var (isValid, errorMessage) = ValidateData();
            if (!isValid)
            {
                MessageBox.Show(this, errorMessage, "Error validating input data", MessageBoxButton.OK,
                    MessageBoxImage.Asterisk);
                return;
            }
            var credential = new Credential
            {
                Email = EmailText.Text,
                Password = Password1Text.Password
            };

            var newUser = new User(UserNameText.Text, EmailText.Text);
            var result = await _securityRepository.Register(credential);
            if (result) {
			    await _loginWindow.SetUserAndCredentials(newUser, credential);
				Close();
			}
        }

        private void CancelButton_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}