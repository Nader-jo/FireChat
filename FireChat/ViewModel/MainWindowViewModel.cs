using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace FireChat.ViewModel
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private string _messages;

        public string Messages
        {
            get => _messages;
            set
            {
                _messages = value;
                NotifyPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChangedEventHandler? handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}