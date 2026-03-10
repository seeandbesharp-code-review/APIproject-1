using DTOs;
using Entities;
using Repositories;

namespace Servers
{
    public interface IUserService
    {
        Task<ResultValidUser<UserDTO>> AddUser(UserDTO user,string password);
        Task<UserDTO> GetUserById(int id);
        Task<UserDTO> Login(string email,string password);
        Task<ResultValidUser<bool>> UpdateUser(int id, UserDTO user, string password);

        Task<bool> ExistsUserWithTheSameEmail(int id,string email);
        bool IsValidEmail(string email);
    }
}