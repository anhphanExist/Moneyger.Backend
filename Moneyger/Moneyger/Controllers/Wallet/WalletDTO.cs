﻿using Moneyger.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Moneyger.Controllers.Wallet
{
    public class WalletDTO : DataDTO
    {
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public decimal Balance { get; set; }
    }
}
