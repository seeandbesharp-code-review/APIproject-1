using DTOs;

namespace Servers
{
    public interface IPasswordService
    {
        Password CheckPassword(string password);
        string HashPassword(string plainPassword);
        bool VerifyPassword(string plainPassword, string hashedPassword);
    }
}