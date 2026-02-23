using DTOs;
using Entitys;
using Repository;

namespace Servers
{
    public interface IUserService
    {
        Task<ResultValidUser<UserDTO>> AddUser(UserDTO user,string password);
        void DeleteUser(int id);
        Task<UserDTO> GetUserById(int id);
        Task<UserDTO> Login(string email,string password);
        Task<ResultValidUser<bool>> UpdateUser(int id, UserDTO user, string password);

        Task<bool> ExistsUserWithTheSameEmail(int id,string email);
    }
}