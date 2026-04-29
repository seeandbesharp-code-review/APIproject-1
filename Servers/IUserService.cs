using DTOs;
using Entities;
using Repositories;

namespace Servers
{
    public interface IUserService
    {
        Task<ResultValidUser<UserDTO>> AddUser(UserWithPasswordDTO user);
        Task<UserDTO> GetUserById(int id);
        Task<UserDTO> Login(string email,string password);
        Task<ResultValidUser<bool>> UpdateUser(int id, UserWithPasswordDTO user);

        Task<bool> ExistsUserWithTheSameEmail(int id,string email);
        bool IsValidEmail(string email);
    }
}