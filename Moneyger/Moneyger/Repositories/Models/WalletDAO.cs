using System;
using System.Collections.Generic;

namespace Moneyger.Repositories.Models
{
    public partial class WalletDAO
    {
        public WalletDAO()
        {
            Transactions = new HashSet<TransactionDAO>();
        }

        public Guid Id { get; set; }
        public long CX { get; set; }
        public string Name { get; set; }
        public decimal Balance { get; set; }
        public Guid UserId { get; set; }

        public virtual UserDAO User { get; set; }
        public virtual ICollection<TransactionDAO> Transactions { get; set; }
    }
}
