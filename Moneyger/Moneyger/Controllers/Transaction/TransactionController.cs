﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
        //private ITransactionService transactionService;
        public TransactionController()
        {

        }

        [Route(TransactionRoute.Create), HttpPost]
        public async Task Create([FromBody] TransactionDTO transactionRequestDTO)
        {
            Entities.Transaction newTransaction = new Entities.Transaction {
                WalletName = transactionRequestDTO.WalletName,
                Amount = transactionRequestDTO.Amount,
                CategoryName = transactionRequestDTO.CategoryName,
                Date = transactionRequestDTO.Date,
                Note = transactionRequestDTO.Note
            };


        }

        [Route(TransactionRoute.GetTransactionMonthGroup), HttpPost]
        public async Task<TransactionMonthGroupResponseDTO> GetTransactionMonthGroup([FromBody] TransactionMonthGroupRequestDTO transactionMonthGroupRequestDTO)
        {
            throw new NotImplementedException();
        }
    }
}