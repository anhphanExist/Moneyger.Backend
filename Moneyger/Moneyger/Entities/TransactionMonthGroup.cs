using Moneyger.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Moneyger.Entities
{
    public class TransactionMonthGroup : DataEntity
    {
        public decimal Income { get; set; }
        public decimal Outcome { get; set; }
        public decimal InOutRate { get; set; }
        public DateTime Month { get; set; }
        public List<TransactionDayGroup> TransactionDayGroups { get; set; }
    }

    public class TransactionMonthGroupFilter : FilterEntity
    {
        public DateTimeFilter Month { get; set; }
    }
}
