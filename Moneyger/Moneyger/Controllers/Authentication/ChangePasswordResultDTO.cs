using Moneyger.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Moneyger.Controllers.Authentication
{
    public class ChangePasswordResultDTO : DataDTO
    {
        public string Username { get; set; }
        public bool Success { get; set; }
    }
}
