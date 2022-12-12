using System;
using System.Collections.Generic;

namespace FireChat.Models
{
    public class User : IHasId
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public List<string> ContactEmailList { get; set; }


        public User(string username, string email)
        {
            Id = Guid.NewGuid().ToString();
            Username = username;
            Email = email;
            ContactEmailList = new List<string>();
        }

        public void AddContact(string contact) => ContactEmailList.Add(contact);
        public void DeleteContact(string contact) => ContactEmailList.Remove(contact);
    }
}