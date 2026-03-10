using System.Text.Json;
using Entities;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Repositories
{
    public class UserRepository : IUserRepository
    {
        dbSHOPContext _dbSHOPContext;
        public UserRepository(dbSHOPContext dbSHOPContext)
        {
            _dbSHOPContext = dbSHOPContext;
        }

        public async Task<User> GetUserById(int id)
        {
            return await _dbSHOPContext.FindAsync<User>(id);
        }


        public async Task<User> AddUser(User user)
        {
            await _dbSHOPContext.AddAsync(user);
            await _dbSHOPContext.SaveChangesAsync();
            return user;
        }



        public async Task<User> Login(string email, string password)
        {

            User? user = await _dbSHOPContext.Users.FirstOrDefaultAsync(x=>x.UserEmail==email &&
            x.UserPassword==password);

            return user;
        }


        public async Task UpdateUser( User updatedUser)
        {
            _dbSHOPContext.Users.Update(updatedUser);
            await _dbSHOPContext.SaveChangesAsync();

        }

        public async Task<User> GetUserByEmail(string email)
        {
            return await _dbSHOPContext.Users.AsNoTracking().FirstOrDefaultAsync(x=>x.UserEmail==email);
        }


    }
}
