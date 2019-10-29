using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moneyger.Entities;
using Moneyger.Services.MTransaction;

namespace Moneyger.Controllers.Transaction
{
    public class TransactionRoute : Root
    {
        public const string Default = Base + "/transaction";
        public const string Create = Default + "/create";
        public const string GetTransactionMonthGroup = Default + "/get-transaction-month-group";
    }

    public class TransactionController : ApiController
    {
        private ITransactionService transactionService;
        public TransactionController(ITransactionService transactionService)
        {
            this.transactionService = transactionService;
        }

        [Route(TransactionRoute.Create), HttpPost]
        public async Task<TransactionDTO> Create([FromBody] TransactionDTO transactionRequestDTO)
        {
            Entities.Transaction newTransaction = new Entities.Transaction {
                WalletName = transactionRequestDTO.WalletName,
                Amount = transactionRequestDTO.Amount,
                CategoryName = transactionRequestDTO.CategoryName,
                Date = transactionRequestDTO.Date,
                Note = transactionRequestDTO.Note
            };

            Entities.Transaction res = await transactionService.Create(newTransaction);
            return new TransactionDTO
            {
                Errors = res.Errors,
                WalletName = res.WalletName,
                Note = res.Note,
                Date = res.Date,
                CategoryName = res.CategoryName,
                Amount = res.Amount
            };
        }

        [Route(TransactionRoute.GetTransactionMonthGroup), HttpPost]
        public async Task<TransactionMonthGroupResponseDTO> GetTransactionMonthGroup([FromBody] TransactionMonthGroupRequestDTO transactionMonthGroupRequestDTO)
        {
            // Tao filter cho MonthGroup
            TransactionMonthGroupFilter filter = new TransactionMonthGroupFilter
            {
                Month = transactionMonthGroupRequestDTO.Month,
                Year = transactionMonthGroupRequestDTO.Year,
                WalletName = transactionMonthGroupRequestDTO.WalletName,
                UserId = currentUserId
            };

            // Get MonthGroup tu filter
            TransactionMonthGroup res = await transactionService.GetTransactionMonthGroup(filter);
            
            // Tao List DayGroupDTO de gan vao trong ket qua
            List<TransactionDayGroupDTO> transactionDayGroupDTOs = new List<TransactionDayGroupDTO>();

            // Voi moi DayGroupDTO thi gan list transactionDTO
            res.TransactionDayGroups.ForEach(tdg => 
            {
                // Tao list transactionDTO
                List<TransactionDTO> transactionDTOs = new List<TransactionDTO>();
                tdg.Transactions.ForEach(t => transactionDTOs.Add(new TransactionDTO
                {
                    WalletName = t.WalletName,
                    Date = t.Date,
                    Errors = t.Errors,
                    Amount = t.Amount,
                    CategoryName = t.CategoryName,
                    Note = t.Note
                }));

                // Gan list transactionDTO vao DayGroupDTO
                transactionDayGroupDTOs.Add(new TransactionDayGroupDTO
                {
                    Errors = tdg.Errors,
                    Date = tdg.Date,
                    Inflow = tdg.Inflow,
                    Outflow = tdg.Outflow,
                    Transactions = transactionDTOs
                });
            });
            
            // Tra ket qua MonthGroupDTO
            return new TransactionMonthGroupResponseDTO
            {
                TransactionDayGroups = transactionDayGroupDTOs,
                InOutRate = res.InOutRate,
                Inflow = res.Inflow,
                Outflow = res.Outflow,
                Month = res.Month,
                Year = res.Year,
                Errors = res.Errors
            };
        }
    }
}