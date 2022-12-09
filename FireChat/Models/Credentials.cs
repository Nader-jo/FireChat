namespace FireChat.Models
{
    public class Credential
    {
        public string Password { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public bool AutoLogin { get; set; } = false;
    }
}
