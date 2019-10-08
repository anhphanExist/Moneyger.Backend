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
        Task<bool> Create(Wallet wallet);
        Task<bool> Update(Wallet wallet);
        Task<bool> Delete(Guid Id);
        Task<int> Count(WalletFilter filter);
        Task<List<Wallet>> List(WalletFilter filter);
    }
    public class WalletRepository : ITransactionRepository
    {
        public Task<int> Count(TransactionFilter filter)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Create(Transaction transaction)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Delete(Guid Id)
        {
            throw new NotImplementedException();
        }

        public Task<Transaction> Get(TransactionFilter filter)
        {
            throw new NotImplementedException();
        }

        public Task<List<Transaction>> List(TransactionFilter filter)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update(Transaction transaction)
        {
            throw new NotImplementedException();
        }
    }
}
