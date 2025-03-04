using Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Repositories;
using Zxcvbn;

namespace Services
{
    public class UserService : IUserService
    {
        IUserRepository _iuserRepository;

        public UserService(IUserRepository iuserRepository)
        {
            _iuserRepository = iuserRepository;
        }

        public async Task<User> GetById(int id)
        {
            return await _iuserRepository.GetById(id);
        }

        public Task<User> PostLoginS(string username, string password)
        {

            return _iuserRepository.PostLoginR(username, password);
        }
        public async Task<User> Post(User user)
        {
            int result = CheckPassword(user.Password);
            if (result <= 3)
                return null;
            await _iuserRepository.Post(user);
            return user;
        }

        public int CheckPassword(string password)
        {
            var result = Zxcvbn.Core.EvaluatePassword(password);
            return result.Score;
        }
        public async Task<User> Put(int id, User user)
        {
            int result = CheckPassword(user.Password);
            if (result <= 3)
                return null;
            return await _iuserRepository.Put(id, user);
        }


        public async Task<User> CheckIfUserExist(User user)
        {
            return await _iuserRepository.CheckIfUserExist(user);
        }
    }
}
