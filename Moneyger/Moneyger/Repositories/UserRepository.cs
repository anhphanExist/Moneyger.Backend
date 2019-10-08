using Moneyger.Common;
using Moneyger.Entities;
using Moneyger.Repositories.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moneyger.Repositories
{
    public interface IUserRepository
    {
        Task<User> Get(Guid Id);
        Task<bool> Create(User user);
        Task<bool> Update(User user);
        Task<bool> Delete(Guid Id);
    }
    public class UserRepository : IUserRepository
    {
        private WASContext wASContext;
        public UserRepository(WASContext wASContext)
        {
            this.wASContext = wASContext;
        }
        public async Task<User> Get(Guid Id)
        {
            UserDAO user = wASContext.UserDAO
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
            wASContext.UserDAO
                .Where(u => u.Id.Equals(user.Id))
                .UpdateFromQuery(u => new UserDAO
                {
                    Password = user.Password
                });
            wASContext.SaveChanges();
            return true;
        }

        public async Task<bool> Delete(Guid Id)
        {
            try
            {
                UserDAO user = wASContext.UserDAO.Where(u => u.Id == Id).Select(c => new UserDAO()
                {
                    Id = user.Id,
                    Username = user.Username,
                    Password = user.Password
                }).FirstOrDefault();
                wASContext.UserDAO.Remove(user);
                wASContext.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
            return true;
        }
    }
}
