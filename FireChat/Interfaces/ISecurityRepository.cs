using FireChat.Models;
using System.Threading.Tasks;

namespace FireChat.Interfaces;

public interface ISecurityRepository
{
    public Task<bool> Login(Credential credential);
    public Task<bool> Register(User user);
}