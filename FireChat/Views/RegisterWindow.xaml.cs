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

        public RegisterWindow(ISecurityRepository securityRepository, MainWindow parentWindow)
        {
            InitializeComponent();
            _securityRepository = securityRepository;
            _parentWindow = parentWindow;
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


            return (true, string.Empty);
        }

        private void RegisterButton_OnClick(object sender, RoutedEventArgs e)
        {
            var (isValid, errorMessage) = ValidateData();
            if (!isValid)
            {
                MessageBox.Show(this, errorMessage, "Error validating input data", MessageBoxButton.OK,
                    MessageBoxImage.Asterisk);
                return;
            }

            var newUser = new User(UserNameText.Text, new Credential()
                {
                    AutoLogin = false,
                    Email = EmailText.Text,
                    Password = Password1Text.Password
                }
            );
            _securityRepository.Register(newUser);
        }

        private void CancelButton_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}