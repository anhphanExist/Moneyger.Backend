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
        Task<User> Create(UserFilter userFilter);
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
            User user = await unitOfWork.UserRepository.Get(userFilter);
            return user;
        }

        public async Task<User> ChangePassword(UserFilter userFilter, string newPassword)
        {
            User user = await unitOfWork.UserRepository.Get(userFilter);
            user.Password = newPassword;
            await unitOfWork.UserRepository.Update(user);
            return user;
        }

        public async Task<User> Create(UserFilter userFilter)
        {
            User existedUser = await unitOfWork.UserRepository.Get(userFilter);
            User newUser = new User
            {
                Id = Guid.NewGuid(),
                Username = userFilter.Username,
                Password = userFilter.Password
            };
            await unitOfWork.UserRepository.Create(newUser);
            return newUser;
        }
    }
}
