using Moneyger.Common;
using Moneyger.Entities;
using Moneyger.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Moneyger.Services
{
    public interface IUserService : IServiceScoped
    {
        Task<User> Login(UserFilter userFilter);
        Task<User> ChangePassword(UserFilter userFilter, string newPassword);
    }
    public class UserService : IUserService
    {
        private IUOW unitOfWork;
        public UserService(IUOW unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<User> Login(UserFilter userFilter)
        {
            User user = await GetUser(userFilter);
            return user;
        }

        public async Task<User> ChangePassword(UserFilter userFilter, string newPassword)
        {
            User user = await GetUser(userFilter);
            user.Password = newPassword;
            await unitOfWork.UserRepository.Update(user);
            return user;
        }

        private async Task<User> GetUser(UserFilter userFilter)
        {
            User user = await unitOfWork.UserRepository.Get(userFilter);
            return user;
        }
    }
}
