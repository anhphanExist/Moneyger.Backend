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
        Task<bool> Update(User user, string newPassword);
    }
    public class UserValidator : IUserValidator
    {
        public enum ErrorCode 
        {
            UserNotExisted,
            UserDuplicated, 
            InvalidPassword,
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
            isValid &= ValidateUsername(user);
            isValid &= await ValidateUserExisted(user);
            if (isValid)
                isValid &= await ValidatePassword(user);
            return isValid;
        }

        public async Task<bool> Create(User user)
        {
            bool isValid = true;
            isValid &= ValidateUsername(user);
            isValid &= await ValidateUserNotDuplicated(user);
            return isValid;
        }

        public async Task<bool> Update(User user, string newPassword)
        {
            bool isValid = true;
            
            isValid &= ValidateUsername(user);
            isValid &= await ValidateUserExisted(user);
            if (isValid)
                isValid &= await ValidatePassword(user);
            isValid &= ValidateNewPassword(user, newPassword);
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
            if (user.Password.Length <= 0)
            {
                user.AddError(nameof(User), nameof(User.Password), ErrorCode.StringEmpty);
                return false;
            }
            else if (user.Password.Length > 500)
            {
                user.AddError(nameof(User), nameof(User.Password), ErrorCode.StringLimited);
                return false;
            }   
            UserFilter userFilter = new UserFilter
            {
                Username = user.Username,
                Password = user.Password
            };
            int count = await unitOfWork.UserRepository.Count(userFilter);
            if (count == 0)
            {
                user.AddError(nameof(User), nameof(User.Password), ErrorCode.InvalidPassword);
                return false;
            }
            return true;
        }

        private bool ValidateUsername(User user)
        {
            if (user.Username.Length <= 0)
            {
                user.AddError(nameof(User), nameof(User.Username), ErrorCode.StringEmpty);
                return false;
            }
            else if (user.Username.Length > 500)
            {
                user.AddError(nameof(User), nameof(User.Username), ErrorCode.StringLimited);
                return false;
            }
            return true;
        }

        private bool ValidateNewPassword(User user, string newPassword)
        {
            if (newPassword == null)
            {
                user.AddError(nameof(User), nameof(newPassword), ErrorCode.InvalidPassword);
                return true;
            }
            else if (newPassword.Length <= 0)
            {
                user.AddError(nameof(User), nameof(newPassword), ErrorCode.StringEmpty);
                return false;
            }
            else if (newPassword.Length > 500)
            {
                user.AddError(nameof(User), nameof(newPassword), ErrorCode.StringLimited);
                return false;
            }
            return true;
        }
    }
}
