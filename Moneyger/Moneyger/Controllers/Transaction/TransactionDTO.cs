using Moneyger.Common;
using System;

namespace Moneyger.Controllers.Transaction
{
    public class TransactionDTO : DataDTO
    {
        public string WalletName { get; set; }
        public string CategoryName { get; set; }
        public decimal Amount { get; set; }
        public string Note { get; set; }
        public DateTime Date { get; set; }
    }
}