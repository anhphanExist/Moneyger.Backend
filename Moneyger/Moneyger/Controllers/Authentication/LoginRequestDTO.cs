using Moneyger.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Moneyger.Controllers.Authentication
{
    public class LoginRequestDTO : DataDTO
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
