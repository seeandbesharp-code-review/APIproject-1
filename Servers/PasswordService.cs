using Zxcvbn;
using DTOs;

namespace Servers
{
    public class PasswordService : IPasswordService
    {
        public Password CheckPassword(string password)
        {
            Password password1 = new Password();
            password1.ThePassword = password;
            var result = Zxcvbn.Core.EvaluatePassword(password1.ThePassword);
            password1.Level = result.Score;
            return password1;
        }

        // Generates a BCrypt hash — salt is embedded inside the returned string
        public string HashPassword(string plainPassword)
            => BCrypt.Net.BCrypt.HashPassword(plainPassword);

        // Extracts the salt from hashedPassword automatically and compares
        public bool VerifyPassword(string plainPassword, string hashedPassword)
            => BCrypt.Net.BCrypt.Verify(plainPassword, hashedPassword);
    }
}
