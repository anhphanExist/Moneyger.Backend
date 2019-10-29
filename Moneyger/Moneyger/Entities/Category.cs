using Moneyger.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Moneyger.Entities
{
    public class Category : DataEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public CategoryType Type { get; set; }
        public byte[] Image { get; set; }
    }
    public class CategoryFilter : FilterEntity
    {
        public GuidFilter Id { get; set; }
        public StringFilter Name { get; set; }
        public bool Type { get; set; }
        public CategoryOrder OrderBy { get; set; }
        public CategoryFilter() : base()
        {

        }
    }
    public enum CategoryOrder
    {
        CX,
        Name,
        Type
    }

    public enum CategoryType
    {
        Outflow,
        Inflow
    }
}
