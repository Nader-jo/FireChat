using FireChat.Interfaces;
using FireChat.Models;
using FireChat.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

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
        private User _currentUser;
        private Credential _currentUserCredentials;
        private bool _isAuthenticated;
        private IOrderedEnumerable<Message> _sortedMessages;
        private MainWindowViewModel vm = new MainWindowViewModel();

        public MainWindow(ISecurityRepository securityRepository, ICredentialsRepository credentialsRepository,
            IUserRepository userRepository, IMessagesRepository messagesRepository)
        {
            InitializeComponent();
            _securityRepository = securityRepository;
            _credentialsRepository = credentialsRepository;
            _userRepository = userRepository;
            _messagesRepository = messagesRepository;
            Messages.DataContext = vm;
        }

        public void Authenticate(User user, Credential credential)
        {
            UpdateContactList(user);
            _currentUserCredentials = credential;
            _isAuthenticated = true;
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
            UsernameTxt.Text = _currentUser.Username;
        }

        private void AddContactButton_Click(object sender, RoutedEventArgs e)
        {
            Opacity = 0.4;
            var addContactWindow = new AddContactWindow(_userRepository, this, _currentUser);
            addContactWindow.Topmost = true;
            addContactWindow.Show();
        }

        internal async void UpdateContactList(User newUser)
        {
            _currentUser = newUser;
            var contactUserList = await _userRepository.GetByEmail(_currentUser.ContactEmailList);
            ContactList.ItemsSource = contactUserList;
            ContactList.DisplayMemberPath = "Username";
            ContactList.SelectedIndex = 0;
        }

        private async void ContactList_SelectionChanged(object sender,
            System.Windows.Controls.SelectionChangedEventArgs e)
        {
            var messagesFromContact =
                await _messagesRepository.Read(((User)ContactList.SelectedValue).Email, _currentUser.Email);
            var messagesToContact =
                await _messagesRepository.Read(_currentUser.Email, ((User)ContactList.SelectedValue).Email);
            List<Message> messages = new();
            messages.AddRange(messagesFromContact);
            messages.AddRange(messagesToContact);
            _sortedMessages = messages.OrderBy(m => m.SentDate);
            vm.Messages = string.Join(Environment.NewLine,
                _sortedMessages.Select(m =>
                    $"{m.FromUserEmail.Replace(((User)ContactList.SelectedValue).Email, ((User)ContactList.SelectedValue).Username).Replace(_currentUser.Email, _currentUser.Username)} : {m.Content}"));
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            var newMessage = new Message(_currentUser.Email, ((User)ContactList.SelectedValue).Email, Text.Text);
            _messagesRepository.Send(newMessage);
            Text.Text = string.Empty;
            var enumerable = _sortedMessages.Append(newMessage);
            _sortedMessages = enumerable.OrderBy(m => m.SentDate);
            vm.Messages = string.Join(Environment.NewLine,
                _sortedMessages.Select(m =>
                    $"{m.FromUserEmail.Replace(((User)ContactList.SelectedValue).Email, ((User)ContactList.SelectedValue).Username).Replace(_currentUser.Email, _currentUser.Username)} : {m.Content}"));
        }


        private void Text_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SendButton_Click(new object(), new RoutedEventArgs());
            }
        }
    }
}