using Moneyger.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Moneyger.Entities
{
    public class TransactionMonthGroup : DataEntity
    {
        public TransactionMonthGroup()
        {
            TransactionDayGroups = new List<TransactionDayGroup>();
        }
        public decimal Inflow { get; set; }
        public decimal Outflow { get; set; }
        public decimal InOutRate { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public List<TransactionDayGroup> TransactionDayGroups { get; set; }
    }

    public class TransactionMonthGroupFilter : FilterEntity
    {
        public int Month { get; set; }
        public int Year { get; set; }
        public string WalletName { get; set; }
        public Guid UserId { get; set; }
    }
}
