using System;
using System.Configuration;
using System.IO;
using System.Text.Json;
using System.Windows;
using FireChat.Interfaces;
using FireChat.Models;

namespace FireChat.Repositories
{
    internal class CredentialsRepository : ICredentialsRepository
    {
        private static readonly string DirPath = ConfigurationManager.AppSettings.Get("DirPath");
        private readonly IEncryptionRepository _encryptionRepository;

        public CredentialsRepository(IEncryptionRepository encryptionRepository)
        {
            _encryptionRepository = encryptionRepository;
        }

        private static readonly string Path = @$"{DirPath}/credentials.json";

        public Credential LoadCredentials()
        {
            Credential credentials = new();
            if (File.Exists(Environment.ExpandEnvironmentVariables(Path)))
            {
                using StreamReader r = new(Environment.ExpandEnvironmentVariables(Path));
                string json = r.ReadToEnd();
                credentials = JsonSerializer.Deserialize<Credential>(json) ?? new Credential();
            }

            CreateDirectoryIfNotExisting();
            Credential decryptedCredentials = new()
            {
                Email = _encryptionRepository.DecryptData(credentials.Email),
                Password = _encryptionRepository.DecryptData(credentials.Password),
                AutoLogin = credentials.AutoLogin
            };

            return decryptedCredentials;
        }

        public void SaveCredentials(Credential credentials)
        {
            Credential encryptedCredentials;
            CreateDirectoryIfNotExisting();
            try
            {
                encryptedCredentials = new()
                {
                    Email = _encryptionRepository.EncryptData(credentials.Email),
                    Password = _encryptionRepository.EncryptData(credentials.Password),
                    AutoLogin = credentials.AutoLogin
                };
            }
            catch (Exception e)
            {
                MessageBox.Show("Error occurred while encrypting credentials: " + e.Message, "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                var jsonText = JsonSerializer.Serialize(encryptedCredentials);
                File.WriteAllText(Environment.ExpandEnvironmentVariables(Path), jsonText);
            }
            catch (Exception e)
            {
                MessageBox.Show("Error occurred while saving credentials: " + e.Message, "Error", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void CreateDirectoryIfNotExisting()
        {
            if (Directory.Exists(Environment.ExpandEnvironmentVariables(DirPath)))
            {
                return;
            }

            try
            {
                Directory.CreateDirectory(Environment.ExpandEnvironmentVariables(DirPath));
            }
            catch (Exception e)
            {
                MessageBox.Show("Error occurred while creating appdata directory: " + e.Message, "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void DeleteCredentials()
        {
            if (File.Exists(Environment.ExpandEnvironmentVariables(Path)))
            {
                try
                {
                    File.Delete(Environment.ExpandEnvironmentVariables(Path));
                }
                catch (Exception e)
                {
                    MessageBox.Show("Error occurred while deleting credentials: " + e.Message, "Error",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}