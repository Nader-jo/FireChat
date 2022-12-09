namespace FireChat.Models
{
	public class AuthResult
	{
		public string kind { get; set; }
		public string idToken { get; set; }
		public string email { get; set; }
		public string refreshToken { get; set; }
		public string expiresIn { get; set; }
		public string localId { get; set; }
	}

	public class ErrorDetails
	{
		public int code { get; set; }
		public string message { get; set; }
	}

	public class Error
	{
		public ErrorDetails error { get; set; }
	}
}
