using Microsoft.EntityFrameworkCore;
using Moneyger.Common;
using Moneyger.Entities;
using Moneyger.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Moneyger.Repositories
{
    public interface IUserRepository
    {
        Task<int> Count(UserFilter filter);
        Task<User> Get(UserFilter filter);
        Task<User> Get(Guid Id);
        Task<bool> Create(User user);
        Task<bool> Update(User user);
    }
    public class UserRepository : IUserRepository
    {
        private WASContext wASContext;
        public UserRepository(WASContext wASContext)
        {
            this.wASContext = wASContext;
        }

        public async Task<int> Count(UserFilter filter)
        {
            IQueryable<UserDAO> users = wASContext.User;
            users = DynamicFilter(users, filter);
            return users.Count();
        }

        public async Task<User> Get(UserFilter filter)
        {
            IQueryable<UserDAO> users = wASContext.User.AsNoTracking();
            UserDAO userDAO = DynamicFilter(users, filter).FirstOrDefault();
            return new User
            {
                Id = userDAO.Id,
                Password = userDAO.Password,
                Username = userDAO.Username
            };
        }

        public async Task<User> Get(Guid Id)
        {
            UserDAO user = wASContext.User
                .Where(u => u.Id.Equals(Id))
                .AsNoTracking()
                .FirstOrDefault();
            return new User
            {
                Id = user.Id,
                Username = user.Username,
                Password = user.Password
            };
        }

        public async Task<bool> Create(User user)
        {
            wASContext.Add(new UserDAO
            {
                Id = user.Id,
                Username = user.Username,
                Password = user.Password
            });
            wASContext.SaveChanges();
            return true;
        }

        public async Task<bool> Update(User user)
        {
            wASContext.User
                .Where(u => u.Id.Equals(user.Id))
                .UpdateFromQuery(u => new UserDAO
                {
                    Password = user.Password
                });
            wASContext.SaveChanges();
            return true;
        }

        private IQueryable<UserDAO> DynamicFilter(IQueryable<UserDAO> query, UserFilter filter)
        {
            if (filter.Username != null)
                query = query.Where(u => u.Username.Equals(filter.Username));
            if (filter.Password != null)
                query = query.Where(u => u.Password.Equals(filter.Password));
            return query;
        }
    }
}
