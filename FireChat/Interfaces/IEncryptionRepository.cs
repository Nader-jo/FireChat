namespace FireChat.Interfaces
{
    internal interface IEncryptionRepository
    {
        public string EncryptData(string data);
        public string DecryptData(string data);
    }
}
