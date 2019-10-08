using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Moneyger.Common
{
    public class DataDTO
    {
        public Dictionary<string, string> Errors;
    }

    public class FilterDTO
    {
        public int Skip { get; set; }
        public int Take { get; set; }
        public OrderType OrderType { get; set; }

        public FilterDTO()
        {
            Skip = 0;
            Take = 10;
            OrderType = OrderType.ASC;

        }
    }
}
