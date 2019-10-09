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
        Task<bool> Create(Transaction transaction);
        Task<bool> Update(Transaction transaction);
        Task<bool> Delete(Guid Id);
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
            wASContext.SaveChanges();
            return true;
        }

        public async Task<bool> Delete(Guid Id)
        {
            try
            {
                TransactionDAO transaction = wASContext.Transaction.Where(t => t.Id == Id).Select(t => new TransactionDAO()
                {
                    
                    CX = t.CX,
                    WalletId = t.WalletId,
                    CategoryId = t.CategoryId,
                    Amount = t.Amount,
                    Note = t.Note,
                    Date = t.Date
                }).FirstOrDefault();
                wASContext.Transaction.Remove(transaction);
                wASContext.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
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
                CX = transaction.CX,
                WalletId = transaction.WalletId,
                CategoryId = transaction.CategoryId,
                Amount = transaction.Amount,
                Note = transaction.Note,
                Date = transaction.Date
            };
        }

        public async Task<List<Transaction>> List(TransactionFilter filter)
        {
            IQueryable<TransactionDAO> query = wASContext.Transaction;
            query = DynamicFilter(query, filter);
            query = DynamicOrder(query, filter);
            List<Transaction> list = query
                .Select(transaction => new Transaction
                {
                    Id = transaction.Id,
                    CX = transaction.CX,
                    WalletId = transaction.WalletId,
                    CategoryId = transaction.CategoryId,
                    Amount = transaction.Amount,
                    Note = transaction.Note,
                    Date = transaction.Date
                })
                .ToList();
            return list;
        }

        public async Task<bool> Update(Transaction transaction)
        {
            wASContext.Transaction
                .Where(u => u.Id.Equals(transaction.Id))
                .UpdateFromQuery(u => new TransactionDAO
                {
                    WalletId = transaction.WalletId,
                    CX = transaction.CX, 
                    CategoryId = transaction.CategoryId,
                    Amount = transaction.Amount,
                    Note = transaction.Note,
                    Date = transaction.Date
                });
            wASContext.SaveChanges();
            return true;
        }

        private IQueryable<TransactionDAO> DynamicFilter(IQueryable<TransactionDAO> query, TransactionFilter filter)
        {
            if (filter == null)
                return query.Where(q => 1 == 0);
            if (filter.CategoryId != null)
            {
                if (filter.CategoryId.Equal != null)
                    query = query.Where(q => q.CategoryId.Equals(filter.CategoryId.Equal));
                if (filter.CategoryId.NotEqual != null)
                    query = query.Where(q => q.CategoryId.Equals(filter.CategoryId.NotEqual));
                
            }
            if (filter.WalletId != null)
            {
                if (filter.WalletId.Equal != null)
                    query = query.Where(q => q.WalletId.Equals(filter.WalletId.Equal));
                if (filter.WalletId.NotEqual != null)
                    query = query.Where(q => q.WalletId.Equals(filter.WalletId.NotEqual));
               
            }

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
