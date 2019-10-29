using Moneyger.Common;
using System;
using System.Collections.Generic;

namespace Moneyger.Controllers.Transaction
{
    public class TransactionDayGroupDTO : DataDTO
    {
        public DateTime Date { get; set; }
        public decimal Inflow { get; set; }
        public decimal Outflow { get; set; }
        public List<TransactionDTO> Transactions { get; set; }
    }
}