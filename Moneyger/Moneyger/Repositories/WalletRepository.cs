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
    public interface IWalletRepository
    {
        Task<Wallet> Get(WalletFilter filter);
        Task<Wallet> Get(Guid Id);
        Task<bool> Create(Wallet wallet);
        Task<bool> Update(Wallet wallet);
        Task<bool> Delete(Wallet wallet);
        Task<int> Count(WalletFilter filter);
        Task<List<Wallet>> List(WalletFilter filter);
    }
    public class WalletRepository : IWalletRepository
    {
        private WASContext wASContext;
        public WalletRepository(WASContext wASContext)
        {
            this.wASContext = wASContext;
        }
        public async Task<int> Count(WalletFilter filter)
        {
            IQueryable<WalletDAO> walletDAOs = wASContext.Wallet;
            walletDAOs = DynamicFilter(walletDAOs, filter);
            return await walletDAOs.CountAsync();
        }

        public async Task<bool> Create(Wallet wallet)
        {
            WalletDAO walletDAO = await wASContext.Wallet.Where(w => w.Id == wallet.Id).FirstOrDefaultAsync();
            if( walletDAO == null)
            {
                walletDAO = new WalletDAO()
                {
                    Id = wallet.Id,
                    Name = wallet.Name,
                    Balance = wallet.Balance,
                    UserId = wallet.UserId
                };
                await wASContext.Wallet.AddAsync(walletDAO);
            }
            else
            {
                walletDAO.Name = wallet.Name;
                walletDAO.Balance = wallet.Balance;
                walletDAO.UserId = wallet.UserId;
            }
            await wASContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Delete(Wallet wallet)
        {
            WalletDAO walletDAO = wASContext.Wallet.Where(w => w.Id.Equals(wallet.Id)).FirstOrDefault();
            try
            {
                wASContext.Wallet.Remove(walletDAO);
                await wASContext.SaveChangesAsync();
            }
            catch
            {
                wASContext.Wallet.Update(walletDAO);
                await wASContext.SaveChangesAsync();
            }
            return true;
        }

        public async Task<Wallet> Get(WalletFilter filter)
        {
            IQueryable<WalletDAO> wallets =  wASContext.Wallet.AsNoTracking();
            WalletDAO walletDAO = DynamicFilter(wallets, filter).FirstOrDefault();
            return new Wallet()
            {
                Id = walletDAO.Id,
                Name = walletDAO.Name,
                Balance = walletDAO.Balance,
                UserId = walletDAO.UserId
            };
        }

        public async Task<Wallet> Get(Guid Id)
        {
            WalletDAO walletDAO = wASContext.Wallet
                .Where(w => w.Id.Equals(Id))
                .AsNoTracking()
                .FirstOrDefault();
            return new Wallet()
            {
                Id = walletDAO.Id,
                Name = walletDAO.Name,
                Balance = walletDAO.Balance,
                UserId = walletDAO.UserId
            };
        }

        public async Task<List<Wallet>> List(WalletFilter filter)
        {
            if (filter == null) return new List <Wallet>();
            IQueryable<WalletDAO> query = wASContext.Wallet;
            query = DynamicFilter(query, filter);
            query = DynamicOrder(query, filter);
            List<Wallet> list = await query.Select(q => new Wallet()
            {
                Id = q.Id,
                Name = q.Name,
                Balance = q.Balance,
                UserId = q.UserId
            })
            .ToListAsync();
            return list;
        }

        public async Task<bool> Update(Wallet wallet)
        {
            await wASContext.Wallet.Where(w => w.Id.Equals(wallet.Id)).UpdateFromQueryAsync(w => new WalletDAO
            {
                Id = wallet.Id,
                Name = wallet.Name,
                Balance = wallet.Balance,
                UserId = wallet.UserId
            });
            await wASContext.SaveChangesAsync();
            return true;
        }

        private IQueryable<WalletDAO> DynamicFilter(IQueryable<WalletDAO> query, WalletFilter filter)
        {
            if(filter == null)
                return query.Where(q => 1 == 0);
            query = query.Where(q => q.UserId.Equals(filter.UserId));
            if (filter.Id != null)
                query = query.Where(q => q.Id, filter.Id);
            if (filter.Name != null)
                query = query.Where(q => q.Name, filter.Name);
            if (filter.Balance != null)
                query = query.Where(q => q.Balance, filter.Balance);
            
            return query;
        }

        private IQueryable<WalletDAO> DynamicOrder(IQueryable<WalletDAO> query, WalletFilter filter)
        {
            switch (filter.OrderType)
            {
                case OrderType.ASC:
                    switch (filter.OrderBy)
                    {
                        case WalletOrder.Name:
                            query = query.OrderBy(q => q.Name);
                            break;
                        case WalletOrder.Balance:
                            query = query.OrderBy(q => q.Balance);
                            break;
                        default:
                            query = query.OrderBy(q => q.CX);
                            break;
                    }
                    break;
                case OrderType.DESC:
                    switch (filter.OrderBy)
                    {
                        case WalletOrder.Name:
                            query = query.OrderByDescending(q => q.Name);
                            break;
                        case WalletOrder.Balance:
                            query = query.OrderByDescending(q => q.Balance);
                            break;
                        default:
                            query = query.OrderByDescending(e => e.CX);
                            break;
                    }
                    break;
                default:
                    query = query.OrderBy(q => q.CX);
                    break;
            }
            return query.Skip(filter.Skip).Take(filter.Take);
        }


    }
}

