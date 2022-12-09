using FireChat.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FireChat.Interfaces;

public interface IMessagesRepository
{
    public Task<bool> Send<T>(T item) where T : HasId;
    public Task<bool> Update<T>(T item) where T : HasId;
    public Task<bool> Delete<T>(T item) where T : HasId;
    public Task<List<T>> Read<T>() where T : HasId;
}