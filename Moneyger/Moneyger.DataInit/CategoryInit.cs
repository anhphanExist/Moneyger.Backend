using Moneyger.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Moneyger.DataInit
{
    public class CategoryInit : CommonInit
    {
        public List<string> CategoryCodes { get ; private set; }
        public CategoryInit(WASContext wASContext) : base(wASContext)
        {
            CategoryCodes = new List<string>();
        }

        public List<string> Init(int count = 1)
        {
            List<string> returnList = new List<string>();
            string baseCode = "Category";

            for (int i = 0; i < count; i++)
            {
                string code = baseCode + i.ToString();


                wASContext.Category.Add(new CategoryDAO
                {
                    Id = CreateGuid(code),
                    Name = code,
                    Type = i % 2 == 0
                });
                returnList.Add(code);
            }

            CategoryCodes.AddRange(returnList);
            return returnList;
        }
    }
}
