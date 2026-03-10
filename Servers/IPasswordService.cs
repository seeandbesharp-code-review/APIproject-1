using Entities;
using DTOs;


namespace Servers
{
    public interface IPasswordService
    {
        Password CheckPassword(string password);
    }
}