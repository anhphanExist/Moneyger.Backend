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
        Task<User> Login(User user);
        Task<User> ChangePassword(User user, string newPassword);
        Task<User> Create(User user);
    }
    public class UserService : IUserService
    {
        private IUOW UnitOfWork;
        private IUserValidator UserValidator;
        public UserService(IUOW UnitOfWork, IUserValidator UserValidator)
        {
            this.UnitOfWork = UnitOfWork;
            this.UserValidator = UserValidator;
        }

        public async Task<User> Login(User user)
        {
            if (!await UserValidator.Login(user))
                return user;

            UserFilter userFilter = new UserFilter
            {
                Username = user.Username,
                Password = user.Password
            };
            return await UnitOfWork.UserRepository.Get(userFilter);
        }

        public async Task<User> ChangePassword(User user, string newPassword)
        {
            if (!await UserValidator.Update(user, newPassword))
                return user;

            using (UnitOfWork.Begin())
            {
                try
                {
                    UserFilter filter = new UserFilter
                    {
                        Username = user.Username,
                        Password = user.Password
                    };
                    user = await Get(filter);
                    user.Password = newPassword;
                    
                    await UnitOfWork.UserRepository.Update(user);
                    await UnitOfWork.Commit();
                    return await Get(new UserFilter
                    {
                        Username = user.Username,
                        Password = newPassword
                    });
                }
                catch (Exception e)
                {
                    await UnitOfWork.Rollback();
                    user.AddError(nameof(UserValidator), nameof(User.Password), CommonEnum.ErrorCode.SystemError);
                    return user;
                }
            }
        }

        public async Task<User> Get(UserFilter filter)
        {
            return await UnitOfWork.UserRepository.Get(filter);
        }

        public async Task<User> Get(Guid Id)
        {
            return await UnitOfWork.UserRepository.Get(Id);
        }

        public async Task<User> Create(User user)
        {
            if (!await UserValidator.Create(user))
                return user;
            
            using (UnitOfWork.Begin())
            {
                try
                {
                    user.Id = Guid.NewGuid();
                    await UnitOfWork.UserRepository.Create(user);
                    await UnitOfWork.Commit();
                    return await Get(new UserFilter
                    {
                        Username = user.Username,
                        Password = user.Password
                    });
                }
                catch (Exception e)
                {
                    await UnitOfWork.Rollback();
                    user.AddError(nameof(UserValidator), nameof(User.Password), CommonEnum.ErrorCode.SystemError);
                    return user;
                }
            }
        }
    }
}
