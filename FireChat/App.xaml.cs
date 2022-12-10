using FireChat.Interfaces;
using FireChat.Repositories;
using FireChat.Views;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace FireChat
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly ServiceProvider _serviceProvider;
        public static string UserId = string.Empty;
        public static string IdToken = string.Empty;

        public App()
        {
            ServiceCollection services = new();
            ConfigureServices(services);
            _serviceProvider = services.BuildServiceProvider();
        }

        private void ConfigureServices(ServiceCollection services)
        {
            services.AddScoped<IEncryptionRepository, EncryptionRepository>();
            services.AddScoped<IMessagesRepository, MessagesRepository>();
            services.AddScoped<ISecurityRepository, SecurityRepository>();
            services.AddScoped<ICredentialsRepository, CredentialsRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddSingleton<MainWindow>();
        }

        private void OnStartup(object sender, StartupEventArgs e)
        {
            var mainWindow = _serviceProvider.GetService<MainWindow>();
            mainWindow.Show();
        }
    }
}