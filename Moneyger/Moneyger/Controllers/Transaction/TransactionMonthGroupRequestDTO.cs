using Moneyger.Common;
using System;

namespace Moneyger.Controllers.Transaction
{
    public class TransactionMonthGroupRequestDTO : DataDTO
    {
        public int Month { get; set; }
        public int Year { get; set; }
        public string WalletName { get; set; }
    }
}