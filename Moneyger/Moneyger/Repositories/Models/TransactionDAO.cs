using System;
using System.Collections.Generic;

namespace Moneyger.Repositories.Models
{
    public partial class TransactionDAO
    {
        public Guid Id { get; set; }
        public long CX { get; set; }
        public Guid WalletId { get; set; }
        public Guid CategoryId { get; set; }
        public decimal Amount { get; set; }
        public string Note { get; set; }
        public DateTime Date { get; set; }

        public virtual CategoryDAO Category { get; set; }
        public virtual WalletDAO Wallet { get; set; }
    }
}
