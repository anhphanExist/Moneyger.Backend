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
        public long CX { get; set; }
        public string Name { get; set; }
        public bool Type { get; set; }
        public byte[] Image { get; set; }
    }
    public class CategoryFilter : FilterEntity
    {
        public GuidFilter Id { get; set; }
        public StringFilter Name { get; set; }
        public bool Type { get; set; }
        public CategoryOrder OrderBy { get; set; }
        //public CategorySelect Selects { get; set; }
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

    /*[Flags]
    public enum CategorySelect
    {
        Id = 1,
        CX = 2,
        Name = 4,
        Type = 8,
    }*/
}
