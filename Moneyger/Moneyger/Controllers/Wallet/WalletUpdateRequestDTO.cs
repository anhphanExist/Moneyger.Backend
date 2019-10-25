using Moneyger.Common;
using System;

namespace Moneyger.Controllers.Wallet
{
    public class WalletUpdateRequestDTO : DataDTO
    {
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public decimal Balance { get; set; }
        public string NewName { get; set; }
    }
}