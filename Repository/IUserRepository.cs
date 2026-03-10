using Entitys;

namespace Repository
{
    public interface IUserRepository
    {
        Task<User> AddUser(User user);
        Task<User> GetUserById(int id);
        Task<User> Login(string email, string password);
        Task UpdateUser(User updatedUser);

        Task<User> GetUserByEmail(string email);
    }
}