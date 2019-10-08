using Moneyger.Common;
using Moneyger.Entities;
using Moneyger.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Moneyger.Services
{
    public interface IUserValidator : IServiceScoped
    {
        Task<bool> Login(User user);
        Task<bool> Create(User user);
        Task<bool> Update(User user);
    }
    public class UserValidator : IUserValidator
    {
        public enum ErrorCode 
        {
            UserNotExisted,
            UserDuplicated, 
            WrongPassword,
            StringLimited,
            StringEmpty 
        }

        private IUOW unitOfWork;
        public UserValidator(IUOW unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<bool> Login(User user)
        {
            bool isValid = true;
            isValid &= await ValidateUserExisted(user);
            isValid &= await ValidatePassword(user);
            return isValid;
        }

        public async Task<bool> Create(User user)
        {
            bool isValid = true;
            isValid &= await ValidateUserNotDuplicated(user);
            return isValid;
        }

        public async Task<bool> Update(User user)
        {
            bool isValid = true;
            isValid &= await ValidateUserExisted(user);
            return isValid;
        }

        private async Task<bool> ValidateUserNotDuplicated(User user)
        {
            UserFilter userFilter = new UserFilter
            {
                Username = user.Username
            };
            int count = await unitOfWork.UserRepository.Count(userFilter);
            if (count > 0)
            {
                user.AddError(nameof(User), nameof(User.Username), ErrorCode.UserDuplicated);
                return false;
            }
            return true;
        }

        private async Task<bool> ValidateUserExisted(User user)
        {
            UserFilter userFilter = new UserFilter
            {
                Username = user.Username
            };
            int count = await unitOfWork.UserRepository.Count(userFilter);
            if (count == 0)
            {
                user.AddError(nameof(User), nameof(User.Username), ErrorCode.UserNotExisted);
                return false;
            }
            return true;
        }

        private async Task<bool> ValidatePassword(User user)
        {
            UserFilter userFilter = new UserFilter
            {
                Username = user.Username,
                Password = user.Password
            };
            int count = await unitOfWork.UserRepository.Count(userFilter);
            if (count == 0)
            {
                user.AddError(nameof(User), nameof(User.Password), ErrorCode.WrongPassword);
                return false;
            }
            return true;
        }
    }
}
