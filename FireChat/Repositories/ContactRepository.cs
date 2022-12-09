using FireChat.Interfaces;
using FireChat.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FireChat.Repositories
{
    internal class ContactRepository : BaseRepository, IContactRepository
    {
		public async Task<bool> Add(User user)
		{
			return await Create(new Contact(user.Username, user.Credential.Email));
		}

		public async Task<bool> Add(User user, Contact contact)
        {
            user.AddContact(contact);
            return await Create(contact);
        }

        public async Task<bool> Search(User user, string username)
        {
            var allContacts = await Get(user);
            return allContacts.Exists(c => c.Username == username);
        }

        public async Task<List<Contact>> Get(User user)
        {
            return await Read<Contact>();
        }

        public async Task<bool> Delete(User user, Contact contact)
        {
            user.DeleteContact(contact);
            return await Delete(contact);
        }
	}
}