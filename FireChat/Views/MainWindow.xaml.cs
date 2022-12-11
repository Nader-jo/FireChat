using System.ComponentModel;
using FireChat.Interfaces;
using System.Windows;
using FireChat.Models;
using System;
using System.Collections.Generic;
using System.Linq;

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
		private IOrderedEnumerable<Message> _sortedMessages;

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
			UpdateContactList(user);
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

		private void AddContactButton_Click(object sender, RoutedEventArgs e)
		{
			Opacity = 0.4;
			var addContactWindow = new AddContactWindow(_userRepository, this, currentUser);
			addContactWindow.Topmost = true;
			addContactWindow.Show();
		}

		internal async void UpdateContactList(User newUser)
		{
			currentUser = newUser;
			var ContactUserList = await _userRepository.GetByEmail(currentUser.ContactEmailList);
			ContactList.ItemsSource = ContactUserList;
			ContactList.DisplayMemberPath = "Username";

		}

		private async void ContactList_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
		{
			var messagesFromContact = await _messagesRepository.Read(((User)ContactList.SelectedValue).Email, currentUser.Email);
			var messagesToContact = await _messagesRepository.Read(currentUser.Email, ((User)ContactList.SelectedValue).Email);
			List<Message> messages = new();
			messages.AddRange(messagesFromContact);
			messages.AddRange(messagesToContact);
			_sortedMessages = messages.OrderBy(m => m.SentDate);
			Messages.Text = string.Join(Environment.NewLine, _sortedMessages.Select(m => $"{m.FromUserEmail.Replace(((User)ContactList.SelectedValue).Email, ((User)ContactList.SelectedValue).Username).Replace(currentUser.Email, currentUser.Username)} : {m.Content}"));
		}

		private void SendButton_Click(object sender, RoutedEventArgs e)
		{
			var newMessage = new Message(currentUser.Email, ((User)ContactList.SelectedValue).Email, Text.Text);
			_messagesRepository.Send(newMessage);
			Text.Text = string.Empty;
			UpdateMessageList(newMessage);
		}

		private void UpdateMessageList(Message message)
		{
			_sortedMessages.Append(message);
			_sortedMessages = _sortedMessages.OrderBy(m => m.SentDate);
			Messages.Text = string.Join(Environment.NewLine, _sortedMessages.Select(m => $"{m.FromUserEmail.Replace(((User)ContactList.SelectedValue).Email, ((User)ContactList.SelectedValue).Username).Replace(currentUser.Email, currentUser.Username)} : {m.Content}"));
		}
	}
}