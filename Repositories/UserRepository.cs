using Entities;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Repositories
{
    public class UserRepository : IUserRepository
    {
        //string path = "M:/Store/Repositories/users.txt";
       ManagerApiContext _managerApiContext;

        public UserRepository(ManagerApiContext managerApiContext)
        {
           _managerApiContext = managerApiContext;
        }


        public async Task<User> GetById(int id)
        {
            return await _managerApiContext.Users.FirstOrDefaultAsync(u => u.Id == id);

        }
        public async Task<User> PostLoginR(string username, string password)
        {
           //you can do return 
           return await _managerApiContext.Users.FirstOrDefaultAsync(u=>u.UserName==username&& u.Password==password);
        }
        public async Task<User> Post(User user)
        {
          var user1=await _managerApiContext.Users.AddAsync(user);
          await _managerApiContext.SaveChangesAsync();
          return user1.Entity;
        }
        public async Task<User> Put(int id,User user1)
        {
            user1.Id = id;
            var user= _managerApiContext.Users.Update(user1);
            await _managerApiContext.SaveChangesAsync();
            return user.Entity;
        }
    }
}
