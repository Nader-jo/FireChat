using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace FireChat.Views
{
    /// <summary>
    /// Interaction logic for SigninWindow.xaml
    /// </summary>
    public partial class SigninWindow : Window
    {
        public SigninWindow()
        {
            InitializeComponent();
        }

        private void SigninButton_OnClick(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void CancelButton_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
            var loginWindow = new LoginWindow
            {
                Topmost = true
            };
            loginWindow.Topmost = false;
            loginWindow.Show();
        }
    }
}
