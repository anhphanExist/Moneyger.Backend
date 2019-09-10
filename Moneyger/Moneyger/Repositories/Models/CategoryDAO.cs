using System;
using System.Collections.Generic;

namespace Moneyger.Repositories.Models
{
    public partial class CategoryDAO
    {
        public CategoryDAO()
        {
            Transactions = new HashSet<TransactionDAO>();
        }

        public Guid Id { get; set; }
        public long CX { get; set; }
        public string Name { get; set; }
        public bool Type { get; set; }
        public byte[] Image { get; set; }

        public virtual ICollection<TransactionDAO> Transactions { get; set; }
    }
}
