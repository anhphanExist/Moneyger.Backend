using Moneyger.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Moneyger.Entities
{
    public class TransactionDayGroup : DataEntity
    {
        public TransactionDayGroup()
        {
            Transactions = new List<Transaction>();
        }
        public DateTime Date { get; set; } 
        public decimal Inflow { get; set; }
        public decimal Outflow { get; set; }
        public List<Transaction> Transactions { get; set; }
    }

    public class TransactionDayGroupFilter : FilterEntity
    {
        public int Month{ get; set; }
        public int Year { get; set; }
        public string WalletName { get; set; }
        public Guid UserId { get; set; }
    }

}
