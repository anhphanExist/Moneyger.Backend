using Moneyger.Common;
using System;
using System.Collections.Generic;

namespace Moneyger.Controllers.Transaction
{
    public class TransactionDayGroupResponseDTO : DataDTO
    {
        public DateTime Date { get; set; }
        public decimal TotalRate { get; set; }
        public List<TransactionDTO> TransactionsInDate { get; set; }
    }
}