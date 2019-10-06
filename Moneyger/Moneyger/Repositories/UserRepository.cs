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
        Task<User> Get(UserFilter filter);
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

        private IQueryable<UserDAO> DynamicFilter(IQueryable<UserDAO> query, UserFilter userFilter)
        {
            if (!string.IsNullOrEmpty(userFilter.Username))
            {
                query = query.Where(u => u.Username.Equals(userFilter.Username) && 
                    u.Password.Equals(userFilter.Password));
            }
            return query;
        }

        public async Task<User> Get(UserFilter filter)
        {
            IQueryable<UserDAO> users = wASContext.User;
            UserDAO userDAO = DynamicFilter(users, filter).FirstOrDefault();
            if (userDAO == null)
                return null;
            else
                return new User
                {
                    Id = userDAO.Id,
                    Password = userDAO.Password,
                    Username = userDAO.Username
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
    }
}
