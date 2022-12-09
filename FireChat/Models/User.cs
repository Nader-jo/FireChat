using System.Collections.Generic;

namespace FireChat.Models
{
    public class User
    {
        public User(string username, Credential credential)
        {
            Username = username;
            Credential = credential;
            Contacts = new List<Contact>();
        }

        public string Username { get; set; }
        public Credential Credential { get; set; }
        public List<Contact> Contacts { get; set; }

        public void AddContact(Contact contact) => Contacts.Add(contact);

        public void DeleteContact(Contact contact) => Contacts.Remove(contact);
    }
}