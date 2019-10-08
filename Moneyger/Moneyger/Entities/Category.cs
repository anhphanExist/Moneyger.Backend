using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Moneyger.Entities
{
    public class Category
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool Type { get; set; }
        public byte[] Image { get; set; }
    }
}
