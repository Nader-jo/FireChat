using FireChat.Interfaces;
using FireChat.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FireChat.Repositories
{
	internal class UserRepository : BaseRepository, IUserRepository
	{
		public async Task<bool> Add(User user)
		{
			return await Create(user);
		}

		public async Task<bool> AddContact(User user, string contactEmail)
		{
			user.AddContact(contactEmail);
			return await Update(user);
		}

		public async Task<List<User>> GetAll()
		{
			return await Read<User>();
		}

		public async Task<User> GetByEmail(string userEmail)
		{
			return (await GetAll()).FirstOrDefault(u => u.Email == userEmail);
		}

		public async Task<User> GetByUsername(string username)
		{
			return (await GetAll()).FirstOrDefault(u => u.Username == username);
		}

		public async Task<bool> DeleteContact(User user, string contactEmail)
		{
			user.DeleteContact(contactEmail);
			return await Update(user);
		}

		public async Task<List<User>> Search(string userUsernameOrUserEmail)
		{
			return (await GetAll()).FindAll(u =>
				u.Username.ToLower().Contains(userUsernameOrUserEmail) ||
				u.Email.ToLower().Contains(userUsernameOrUserEmail));
		}
	}
}