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
    public interface ITransactionRepository
    {
        Task<Transaction> Get(Guid Id);
        Task<Transaction> Get(TransactionFilter filter);
        Task<bool> Create(Transaction transaction);
        Task<bool> Update(Transaction transaction);
        Task<bool> Delete(Transaction transaction);
        Task<int> Count(TransactionFilter filter);
        Task<List<Transaction>> List(TransactionFilter filter);
    }
    public class TransactionRepository : ITransactionRepository
    {
        private WASContext wASContext;

        public TransactionRepository(WASContext wASContext)
        {
            this.wASContext = wASContext;
        }
        public async Task<int> Count(TransactionFilter filter)
        {
            IQueryable<TransactionDAO> transactions = wASContext.Transaction;
            transactions = DynamicFilter(transactions, filter);
            return await transactions.CountAsync();
        }

        public async Task<bool> Create(Transaction transaction)
        {
            
            wASContext.Add(new TransactionDAO
            {
                Id = transaction.Id,
                WalletId = transaction.WalletId,
                CategoryId = transaction.CategoryId,
                Amount = transaction.Amount,
                Note = transaction.Note,
                Date = transaction.Date
            });
            await wASContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Delete(Transaction transaction)
        {
            TransactionDAO transactionDAO = wASContext.Transaction.Where(t => t.Id.Equals(transaction.Id)).FirstOrDefault();
            try
            {
                wASContext.Transaction.Remove(transactionDAO);
                await wASContext.SaveChangesAsync();
            } catch
            {
                wASContext.Transaction.Update(transactionDAO);
                await wASContext.SaveChangesAsync();
            }

            return true;
        }

        public async Task<Transaction> Get(Guid Id)
        {
            TransactionDAO transaction = wASContext.Transaction
                .Where(t => t.Id.Equals(Id))
                .AsNoTracking()
                .FirstOrDefault();
            return new Transaction
            {
                Id = transaction.Id,
                WalletId = transaction.WalletId,
                WalletName = transaction.Wallet.Name,
                CategoryId = transaction.CategoryId,
                CategoryName = transaction.Category.Name,
                Amount = transaction.Amount,
                Note = transaction.Note,
                Date = transaction.Date
            };
        }

        public async Task<Transaction> Get(TransactionFilter filter)
        {
            IQueryable<TransactionDAO> transactionDAOs = wASContext.Transaction.AsNoTracking();
            TransactionDAO transactionDAO = DynamicFilter(transactionDAOs, filter).FirstOrDefault();
            return new Transaction
            {
                Id = transactionDAO.Id,
                WalletId = transactionDAO.WalletId,
                WalletName = transactionDAO.Wallet.Name,
                CategoryId = transactionDAO.CategoryId,
                CategoryName = transactionDAO.Category.Name,
                Amount = transactionDAO.Amount,
                Date = transactionDAO.Date,
                Note = transactionDAO.Note
            };
        }

        public async Task<List<Transaction>> List(TransactionFilter filter)
        {
            IQueryable<TransactionDAO> query = wASContext.Transaction;
            query = DynamicFilter(query, filter);
            query = DynamicOrder(query, filter);
            List<Transaction> list = await query.Select(transaction => new Transaction()
            {
                    Id = transaction.Id,
                    WalletId = transaction.WalletId,
                    WalletName = transaction.Wallet.Name,
                    CategoryId = transaction.CategoryId,
                    CategoryName = transaction.Category.Name,
                    Amount = transaction.Amount,
                    Note = transaction.Note,
                    Date = transaction.Date
            }).ToListAsync();
            return list;
        }

        public async Task<bool> Update(Transaction transaction)
        {
            wASContext.Transaction
                .Where(u => u.Id.Equals(transaction.Id))
                .UpdateFromQuery(u => new TransactionDAO
                {
                    WalletId = transaction.WalletId,
                    CategoryId = transaction.CategoryId,
                    Amount = transaction.Amount,
                    Note = transaction.Note,
                    Date = transaction.Date
                });
            await wASContext.SaveChangesAsync();
            return true;
        }

        private IQueryable<TransactionDAO> DynamicFilter(IQueryable<TransactionDAO> query, TransactionFilter filter)
        {
            if (filter == null)
                return query.Where(q => 1 == 0);
            query = query.Where(q => q.Wallet.UserId, filter.UserId);
            if (filter.WalletId != null)
                query = query.Where(q => q.WalletId, filter.WalletId);
            if (filter.WalletName != null)
                query = query.Where(q => q.Wallet.Name, filter.WalletName);
            if (filter.CategoryId != null)
                query = query.Where(q => q.CategoryId, filter.CategoryId);
            if (filter.CategoryName != null)
                query = query.Where(q => q.Category.Name, filter.CategoryName);
            if (filter.Id != null)
                query = query.Where(q => q.Id, filter.Id);
            if (filter.Date != null)
                query = query.Where(q => 
                    q.Date.Day == filter.Date.Equal.Value.Day && 
                    q.Date.Month == filter.Date.Equal.Value.Month && 
                    q.Date.Year == filter.Date.Equal.Value.Year
                );
            return query;
        }

        private IQueryable<TransactionDAO> DynamicOrder(IQueryable<TransactionDAO> query, TransactionFilter filter)
        {
            switch (filter.OrderType)
            {
                case OrderType.ASC:
                    switch (filter.OrderBy)
                    {
                        case TransactionOrder.WalletId:
                            query = query.OrderBy(g => g.WalletId);
                            break;
                        case TransactionOrder.CategoryId:
                            query = query.OrderBy(g => g.CategoryId);
                            break;
                        case TransactionOrder.Id:
                            query = query.OrderBy(g => g.Id);
                            break;
                    }
                    break;
                case OrderType.DESC:
                    switch (filter.OrderBy)
                    {
                        case TransactionOrder.WalletId:
                            query = query.OrderByDescending(g => g.WalletId);
                            break;
                        case TransactionOrder.CategoryId:
                            query = query.OrderByDescending(g => g.CategoryId);
                            break;
                        case TransactionOrder.Id:
                            query = query.OrderByDescending(g => g.Id);
                            break;
                    }
                    break;
                default:
                    query = query.OrderBy(e => e.Id);
                    break;
            }
            return query.Skip(filter.Skip).Take(filter.Take);
        }
    }
}
