using System.Collections.Generic;
using System.Threading.Tasks;
using FireChat.Models;

namespace FireChat.Interfaces
{
    public interface IUserRepository
    {
		public Task<bool> Add(User user);
		public Task<bool> AddContact(User user, string contactEmail);
		public Task<List<User>> GetAll();
		public Task<User> GetByEmail(string userEmail);
		public Task<List<User>> GetByEmail(List<string> userEmailList);
		public Task<User> GetByUsername(string userUsername);
		public Task<List<User>> Search(string userUsernameOrUserEmail);
		public Task<bool> DeleteContact(User user, string contactEmail);
    }
}