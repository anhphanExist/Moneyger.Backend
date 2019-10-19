using Moneyger.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Moneyger.Entities
{
    public class TransactionDayGroup : DataEntity
    {
        public DateTime Date { get; set; } 
        public decimal TotalAmount { get; set; }
        public List<Transaction> Transactions { get; set; }
    }
}
