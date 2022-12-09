using System.Collections.Generic;

namespace FireChat.Models
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public Credential credential { get; set; }
        public List<Contact> Contacts { get; set; }
    }
}