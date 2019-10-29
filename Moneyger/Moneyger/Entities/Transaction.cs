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
    public class TransactionFilter : FilterEntity
    {
        public GuidFilter Id { get; set; }
        public GuidFilter UserId { get; set; }
        public GuidFilter WalletId { get; set; }
        public StringFilter WalletName { get; set; }
        public GuidFilter CategoryId { get; set; }
        public StringFilter CategoryName { get; set; }
        public DateTimeFilter Date { get; set; }
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
