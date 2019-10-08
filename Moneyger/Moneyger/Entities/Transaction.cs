using Moneyger.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Moneyger.Entities
{
    public class Transaction : DataEntity
    {
        public Guid Id { get; set; }
        public Guid WalletId { get; set; }
        public string WalletName { get; set; }
        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; }
        public decimal Amount { get; set; }
        public string Note { get; set; }
        public DateTime Date { get; set; }
    }
}
