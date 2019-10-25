using Moneyger.Common;
using System;

namespace Moneyger.Controllers.Wallet
{
    public class WalletTransferRequestDTO : DataDTO
    {
        public string SourceWalletName { get; set; }
        public string DestWalletName { get; set; }
        public decimal Amount { get; set; }
        public string Note { get; set; }
    }
}