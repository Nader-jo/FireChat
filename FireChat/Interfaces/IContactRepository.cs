using System.Collections.Generic;
using System.Threading.Tasks;
using FireChat.Models;

namespace FireChat.Interfaces
{
    internal interface IContactRepository
    {
        public Task<bool> Add(User user, Contact contact);
        public Task<bool> Search(User user, string username);
        public Task<List<Contact>> Get(User user);
        public Task<bool> Delete(User user, Contact contact);
    }
}