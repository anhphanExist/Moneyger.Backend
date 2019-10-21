using Moneyger.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Moneyger.Entities
{
    public class Wallet : DataEntity
    {
        public Guid Id { get; set; }
        public long CX { get; set; }
        public string Name { get; set; }
        public decimal Balance { get; set; }
        public Guid UserId { get; set; }
    }
    public class WalletFilter : FilterEntity
    {
        public GuidFilter Id { get; set; }
        public StringFilter Name { get; set; }
        public DecimalFilter Balance { get; set; }
        public GuidFilter UserId { get; set; }
        public WalletOrder OrderBy { get; set; }
        public WalletFilter() : base()
        {

        }

    }
    public enum WalletOrder
    {
        Name,
        Balance
    }
}
