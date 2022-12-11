using FireChat.Interfaces;
using FireChat.Models;
using FireChat.Repositories;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;

namespace FireChat.Views
{
	/// <summary>
	/// Interaction logic for AddContactWindow.xaml
	/// </summary>
	public partial class AddContactWindow : Window
	{
		private readonly IUserRepository _userRepository;
		private MainWindow _mainWindow;
		private User _currentUser;
		private List<User> _userList;

		public AddContactWindow(IUserRepository userRepository, MainWindow mainWindow, User currentUser)
		{
			InitializeComponent();
			_userRepository = userRepository;
			_mainWindow = mainWindow;
			_currentUser = currentUser;
		}

		private async void SearchButton_Click(object sender, RoutedEventArgs e)
		{
			if (string.IsNullOrEmpty(ContactNameSearchText.Text.Trim()))
			{
				return;
			}
			_userList = await _userRepository.Search(ContactNameSearchText.Text.Trim());
			if (_userList != null && _userList.Count != 0)
			{
				AddContactButton.IsEnabled = true;
				ResultUserList.ItemsSource = _userList.Select(u => $"{u.Username} ({u.Email})");
			}
		}

		private void CancelButton_Click(object sender, RoutedEventArgs e)
		{
			_mainWindow.Opacity = 1;
			Close();
		}

		private async void AddContactButton_Click(object sender, RoutedEventArgs e)
		{
			var regex = new Regex(@"\(([^)]+)\)");
			var selectedStr = ResultUserList.SelectedItem.ToString();
			if (selectedStr == null)
			{
				return;
			}
			var matches = regex.Matches(selectedStr);
			await _userRepository.AddContact(_currentUser, matches.FirstOrDefault().Value.Trim('(').Trim(')'));
			var newUser =await _userRepository.GetByUsername(_currentUser.Username);
			_mainWindow.Opacity = 1;
			_mainWindow.UpdateContactList(newUser);
			Close();
		}
	}
}
