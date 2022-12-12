using FireChat.Interfaces;
using FireChat.Models;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace FireChat.Views
{
    /// <summary>
    /// Interaction logic for AddContactWindow.xaml
    /// </summary>
    public partial class AddContactWindow : Window
    {
        private readonly IUserRepository _userRepository;
        private readonly MainWindow _mainWindow;
        private readonly User _currentUser;
        private List<User> _userList;

        public AddContactWindow(IUserRepository userRepository, MainWindow mainWindow, User currentUser)
        {
            InitializeComponent();
            _userRepository = userRepository;
            _mainWindow = mainWindow;
            _currentUser = currentUser;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            _mainWindow.Opacity = 1;
            Close();
        }

        private async void AddContactButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedStr = ResultUserList.SelectedItem.ToString();
            if (selectedStr == null)
            {
                return;
            }

            int startIndex = selectedStr.IndexOf('(');
            string contactEmail = string.Empty;
            if (startIndex >= 0)
            {
                int endIndex = selectedStr.IndexOf(')', startIndex);
                if (endIndex > startIndex)
                {
                    contactEmail = selectedStr.Substring(startIndex + 1, endIndex - startIndex - 1);
                }
            }

            await _userRepository.AddContact(_currentUser, contactEmail.Trim('(').Trim(')'));
            _mainWindow.Opacity = 1;
            _mainWindow.UpdateContactList(_currentUser);
            Close();
        }

        private async void ContactNameSearchText_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(ContactNameSearchText.Text.Trim()))
            {
                ResultUserList.ItemsSource = null;
                return;
            }

            _userList = await _userRepository.Search(ContactNameSearchText.Text.Trim());
            if (_userList != null && _userList.Count != 0)
            {
                ResultUserList.ItemsSource = _userList.FindAll(u => u.Email != _currentUser.Email && !_currentUser.ContactEmailList.Contains(u.Email))
                    .Select(u => $"{u.Username} ({u.Email})");
            }
        }

        private void ResultUserList_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            AddContactButton.IsEnabled = true;
        }
    }
}