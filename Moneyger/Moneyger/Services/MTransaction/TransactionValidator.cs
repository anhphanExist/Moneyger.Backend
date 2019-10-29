using Moneyger.Common;
using Moneyger.Entities;
using Moneyger.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Moneyger.Services.MTransaction
{
    public interface ITransactionValidator : IServiceScoped
    {
        Task<bool> Create(Transaction transaction);
        Task<bool> Update(Transaction transaction);
        Task<bool> Delete(Transaction transaction);
    }
    public class TransactionValidator : ITransactionValidator
    {
        public enum ErrorCode
        {
            IdNotFound,
            DateInvalid,
            AmountInvalid
        }
        private IUOW unitOfWork;
        public TransactionValidator(IUOW unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<bool> Create(Transaction transaction)
        {
            List<Transaction> transactions = new List<Transaction> { transaction };

            bool IsValid = true;
            IsValid &= await ValidateInput(transactions);
            return IsValid;

        }

        public async Task<bool> Delete(Transaction transaction)
        {
            bool IsValid = true;
            IsValid &= await ValidateId(transaction);
            return transaction.IsValidated;
        }

        public async Task<bool> Update(Transaction transaction)
        {
            //List<Transaction> transactions = new List<Transaction> { transaction };
            bool IsValid = true;
            IsValid &= await ValidateId(transaction);

            if (!IsValid) return false;

            return transaction.IsValidated;
        }
        private async Task<bool> ValidateId(Transaction transaction)
        {
            TransactionFilter transactionFilter = new TransactionFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                Id = new GuidFilter { Equal = transaction.Id},
                OrderBy = TransactionOrder.Id,
                OrderType = OrderType.ASC
            };

            int count = await unitOfWork.TransactionRepository.Count(transactionFilter);
            if (count == 0)
                transaction.AddError(nameof(TransactionValidator), nameof(transaction.Id), ErrorCode.IdNotFound);

            return count == 1;
        }
        private async Task<bool> ValidateInput(List<Transaction> transactions)
        {
            foreach(var transaction in transactions)
            {
                if (transaction.Amount < 0)
                    transaction.AddError(nameof(TransactionValidator), nameof(transaction.Amount), ErrorCode.AmountInvalid);
                if (transaction.Date == null)
                    transaction.AddError(nameof(TransactionValidator), nameof(transaction.Date), ErrorCode.DateInvalid);
            }
            bool IsValid = true;
            transactions.ForEach(e => IsValid &= e.IsValidated);
            return IsValid;
        }
    }
}
