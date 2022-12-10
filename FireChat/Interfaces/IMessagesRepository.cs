using FireChat.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FireChat.Interfaces;

public interface IMessagesRepository
{
    public Task<bool> Send(Message message);
    public Task<bool> Update(Message message);
    public Task<bool> Delete(Message message);
    public Task<List<Message>> Read(string fromUserEmail, string toUserEmail);
}