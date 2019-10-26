using Moneyger.Common;
using System;
using System.Collections.Generic;

namespace Moneyger.Controllers.Transaction
{
    public class TransactionMonthGroupResponseDTO : DataDTO
    {
        public DateTime Month { get; set; }
        public decimal Inflow { get; set; }
        public decimal Outflow { get; set; }
        public decimal InOutRate { get; set; }
        public List<TransactionDayGroupResponseDTO> transactionDayGroups { get; set; }
    }
}