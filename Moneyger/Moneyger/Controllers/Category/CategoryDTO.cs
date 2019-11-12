using Moneyger.Common;
using Moneyger.Entities;

namespace Moneyger.Controllers.Category
{
    public class CategoryDTO : DataDTO
    {
        public string Name { get; set; }
        public CategoryType Type { get; set; }
    }
}