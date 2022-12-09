using System.Collections.Generic;
using System.Threading.Tasks;
using FireChat.Interfaces;
using FireChat.Models;

namespace FireChat.Repositories
{
    internal class MessagesRepository : BaseRepository, IMessagesRepository
    {
        public async Task<bool> Delete(Message message) => await base.Delete(message);

        public async Task<List<Message>> Read(User fromUser, string toUserEmail)
        {
            var messages = await Read<Message>();
            return messages.FindAll(m =>
                (m.FromUserEmail == fromUser.Credential.Email && m.ToUserEmail == toUserEmail) ||
                (m.FromUserEmail == toUserEmail && m.ToUserEmail == fromUser.Credential.Email));
        }

        public async Task<List<Message>> Read(User user)
        {
            var messages = await Read<Message>();
            return messages.FindAll(m => m.ToUserEmail == user.Credential.Email);
        }

        public async Task<bool> Send(Message message) => await Create(message);

        public async Task<bool> Update(Message message) => await base.Update(message);
    }
}