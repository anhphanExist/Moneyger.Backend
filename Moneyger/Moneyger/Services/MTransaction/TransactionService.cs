using Moneyger.Common;
using Moneyger.Entities;
using Moneyger.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Moneyger.Services.MTransaction
{
    public interface ITransactionService
    {
        Task<Transaction> Get(Guid Id);
        Task<Transaction> Get(TransactionFilter filter);
        Task<Transaction> Create(Transaction transaction);
        Task<Transaction> Update(Transaction transaction);
        Task<Transaction> Delete(Transaction transaction);
        Task<int> Count(TransactionFilter filter);
        Task<List<Transaction>> List(TransactionFilter filter);
    }
    public class TransactionService : ITransactionService
    {
        private IUOW UnitOfWork;
        private ITransactionValidator TransactionValidator;
        public TransactionService(IUOW UnitOfWork, ITransactionValidator transactionValidator)
        {
            this.UnitOfWork = UnitOfWork;
            this.TransactionValidator = transactionValidator;
        }
        public async Task<int> Count(TransactionFilter filter)
        {
            if (filter == null) filter = new TransactionFilter { };
            return await UnitOfWork.TransactionRepository.Count(filter);
        }

        public async Task<Transaction> Create(Transaction transaction)
        {
            transaction.Id = Guid.NewGuid();

            if (transaction == null) return null;
            if (!await TransactionValidator.Create(transaction))
                return transaction;

            using (UnitOfWork.Begin())
            {
                try
                {
                    await UnitOfWork.TransactionRepository.Create(transaction);
                    await UnitOfWork.Commit();
                }
                catch (Exception ex)
                {
                    await UnitOfWork.Rollback();
                    transaction.AddError(nameof(TransactionService), nameof(Transaction.Id), CommonEnum.ErrorCode.SystemError);
                }
            }
            return transaction;
        }

        public async Task<Transaction> Delete(Transaction transaction)
        {
            if (!await TransactionValidator.Delete(transaction))
                return transaction;

            using (UnitOfWork.Begin())
            {
                try
                {
                    //var transactionB = await UnitOfWork.TransactionRepository.Get(transaction.Id);
                    await UnitOfWork.TransactionRepository.Delete(transaction);
                    await UnitOfWork.Commit();
                }
                catch (Exception ex)
                {
                    await UnitOfWork.Rollback();
                    transaction.AddError(nameof(TransactionService), nameof(Transaction.Id), CommonEnum.ErrorCode.SystemError);
                }
            }
            return transaction;
        }

        public async Task<Transaction> Get(Guid Id)
        {
            return await UnitOfWork.TransactionRepository.Get(Id);
        }

        public async Task<Transaction> Get(TransactionFilter filter)
        {
            return await UnitOfWork.TransactionRepository.Get(filter);
        }

        public async Task<List<Transaction>> List(TransactionFilter filter)
        {
            if (filter == null) filter = new TransactionFilter { };
            return await UnitOfWork.TransactionRepository.List(filter);
        }

        public async Task<Transaction> Update(Transaction transaction)
        {
            if (transaction == null) return null;
            if (!await TransactionValidator.Update(transaction))
                return transaction;

            using (UnitOfWork.Begin())
            {
                try
                {
                    await UnitOfWork.TransactionRepository.Update(transaction);
                    await UnitOfWork.Commit();
                    return await this.UnitOfWork.TransactionRepository.Get(transaction.Id);
                }
                catch (Exception ex)
                {
                    await UnitOfWork.Rollback();
                    transaction.AddError(nameof(TransactionService), nameof(transaction.Id), CommonEnum.ErrorCode.SystemError);
                    return transaction;
                }
            }
        }
    }
}
