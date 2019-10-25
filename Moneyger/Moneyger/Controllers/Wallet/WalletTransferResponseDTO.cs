using Moneyger.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Moneyger.Controllers.Wallet
{
    public class WalletTransferResponseDTO : DataDTO
    {
        public Guid UserId { get; set; }
        public string SourceWalletName { get; set; }
        public string DestWalletName { get; set; }
    }
}
