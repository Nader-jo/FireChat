using System;

namespace FireChat.Models;

public class Contact : IHasId
{
    public Contact(string username)
    {
        Id = Guid.NewGuid().ToString();
        Username = username;
    }

    public string Username { get; set; }
    public string Id { get; set; }
}