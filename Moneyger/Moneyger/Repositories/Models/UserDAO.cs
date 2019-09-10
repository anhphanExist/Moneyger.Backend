using System;
using System.Collections.Generic;

namespace Moneyger.Repositories.Models
{
    public partial class UserDAO
    {
        public UserDAO()
        {
            Wallets = new HashSet<WalletDAO>();
        }

        public Guid Id { get; set; }
        public long CX { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public virtual ICollection<WalletDAO> Wallets { get; set; }
    }
}
