using Moneyger.Common;
using Moneyger.Entities;
using Moneyger.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Moneyger.Services.MTransaction
{
    public interface ITransactionService : IServiceScoped
    {
        Task<Transaction> Get(Guid Id);
        Task<Transaction> Get(TransactionFilter filter);
        Task<Transaction> Create(Transaction transaction);
        Task<Transaction> Update(Transaction transaction);
        Task<Transaction> Delete(Transaction transaction);
        Task<int> Count(TransactionFilter filter);
        Task<List<Transaction>> List(TransactionFilter filter);
        Task<TransactionMonthGroup> GetTransactionMonthGroup(TransactionMonthGroupFilter filter);
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
            if (!await TransactionValidator.Create(transaction))
                return transaction;

            using (UnitOfWork.Begin())
            {
                try
                {
                    CategoryFilter categoryFilter = new CategoryFilter
                    {
                        Name = new StringFilter { Equal = transaction.CategoryName }
                    };
                    Category category = await UnitOfWork.CategoryRepository.Get(categoryFilter);

                    WalletFilter walletFilter = new WalletFilter
                    {
                        Name = new StringFilter { Equal = transaction.WalletName },
                        UserId = new GuidFilter { Equal = transaction.UserId }
                    };
                    Wallet wallet = await UnitOfWork.WalletRepository.Get(walletFilter);


                    transaction.Id = Guid.NewGuid();
                    transaction.WalletId = wallet.Id;
                    transaction.CategoryId = category.Id;
                    if (category.Type == CategoryType.Inflow)
                        wallet.Balance += transaction.Amount;
                    else wallet.Balance -= transaction.Amount;

                    await UnitOfWork.WalletRepository.Update(wallet);
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

        public async Task<TransactionMonthGroup> GetTransactionMonthGroup(TransactionMonthGroupFilter filter)
        {
            // Filter theo thang va nam de list transaction day group theo thang do
            TransactionDayGroupFilter transactionDayGroupFilter = new TransactionDayGroupFilter
            {
                Month = filter.Month,
                Year = filter.Year,
                WalletName = filter.WalletName,
                UserId = filter.UserId
            };
            List<TransactionDayGroup> transactionDayGroups = await ListTransactionDayGroup(transactionDayGroupFilter);

            // Tinh toan voi outflow < 0 va inflow > 0
            decimal inflow = 0;
            decimal outflow = 0;
            transactionDayGroups.ForEach(tdg =>
            {
                inflow += tdg.Inflow;
                outflow += tdg.Outflow;
            });

            // Tra ve transaction month group
            return new TransactionMonthGroup
            {
               Outflow = outflow,
               Inflow = inflow,
               InOutRate = inflow + outflow,
               Month = filter.Month,
               TransactionDayGroups = transactionDayGroups
            };
        }

        private async Task<List<TransactionDayGroup>> ListTransactionDayGroup(TransactionDayGroupFilter filter)
        {
            // Khoi tao List chua ket qua
            List<TransactionDayGroup> result = new List<TransactionDayGroup>();

            // Lay het ngay trong thang filter ra
            List<DateTime> dates = GetDates(filter.Year, filter.Month);

            // Trong moi ngay ma co Transaction thi tao 1 TransactionDayGroup
            dates.ForEach(async d =>
            {
                // Tao filter theo tung ngay trong thang
                TransactionFilter transactionFilter = new TransactionFilter
                {
                    Date = new DateTimeFilter { Equal = d },
                    WalletName = new StringFilter { Equal = filter.WalletName },
                    UserId = new GuidFilter { Equal = filter.UserId }
                };

                // List transactions trong ngay hom do
                List<Transaction> transactions = await List(transactionFilter);

                // Neu ngay hom do co transactions thi tao 1 TransactionDayGroup moi
                if (transactions != null)
                {
                    decimal inflow = 0;
                    decimal outflow = 0;
                    transactions.ForEach(t => 
                    {
                        if (t.Amount < 0) outflow += t.Amount;
                        else inflow += t.Amount;
                    });
                    result.Add(new TransactionDayGroup
                    {
                        Date = d,
                        Transactions = transactions,
                        Inflow = inflow,
                        Outflow = outflow
                    });
                }
            });

            // Tra ket qua
            return result;
            
        }

        private static List<DateTime> GetDates(int year, int month)
        {
            return Enumerable.Range(1, DateTime.DaysInMonth(year, month))  // Days: 1, 2 ... 31 etc.
                             .Select(day => new DateTime(year, month, day)) // Map each day to a date
                             .ToList(); // Load dates into a list
        }
    }
}
