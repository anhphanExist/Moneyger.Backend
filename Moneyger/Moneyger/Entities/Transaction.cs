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
        public long CX { get; set; }
        public Guid WalletId { get; set; }
        public Guid CategoryId { get; set; }
        public decimal Amount { get; set; }
        public string Note { get; set; }
        public DateTime Date { get; set; }
    }
    public class TransactionFilter : FilterEntity
    {
        public GuidFilter Id { get; set; }
        public GuidFilter WalletId { get; set; }
        public GuidFilter CategoryId { get; set; }
        public TransactionOrder OrderBy { get; set; }
        public TransactionFilter() : base()
        {

        }
    }
    public enum TransactionOrder
    {
        Id,
        WalletId,
        CategoryId
    }

}
