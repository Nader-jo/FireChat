using FireChat.Models;

namespace FireChat.Interfaces
{
    public interface ICredentialsRepository
    {
        public Credential LoadCredentials();
        public void SaveCredentials(Credential credentials);
        public void DeleteCredentials();
    }
}