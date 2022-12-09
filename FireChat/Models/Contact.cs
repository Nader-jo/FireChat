using System;

namespace FireChat.Models;

public class Contact : IHasId
{
    public Contact(string username, string email)
    {
        Id = Guid.NewGuid().ToString();
        Username = username;
        Email = email;
    }

	public string Username { get; set; }
	public string Email { get; set; }
	public string Id { get; set; }
}