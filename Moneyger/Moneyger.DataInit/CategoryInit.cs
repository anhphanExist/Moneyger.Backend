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
            List<string> outflowCategories = new List<string>
            {
                "Bills",
                "Business",
                "Education",
                "Entertainment",
                "Family",
                "Punishment Charge",
                "Food",
                "Gifts and Donation",
                "Health and Fitness",
                "Insurances",
                "Investment",
                "Shopping",
                "OtherExpenses",
                "Transportation",
                "Travel",
                "Withdrawal"
            };
            List<string> inflowCategories = new List<string>
            {
                "Award",
                "Gifts",
                "Interest Money",
                "OtherIncome",
                "Salary",
                "Selling"
            };

            foreach (string item in outflowCategories)
            {
                wASContext.Add(new CategoryDAO
                {
                    Id = CreateGuid(item),
                    Type = false,
                    Name = item
                });
                returnList.Add(item);
            }
            
            foreach (string item in inflowCategories)
            {
                wASContext.Add(new CategoryDAO
                {
                    Id = CreateGuid(item),
                    Type = true,
                    Name = item
                });
                returnList.Add(item);
            }

            wASContext.Category.Add(new CategoryDAO
            {
                Id = CreateGuid("Wallet Transfer Source"),
                Type = false,
                Name = "Wallet Transfer",
            });
            returnList.Add("Wallet Transfer Source");
            wASContext.Category.Add(new CategoryDAO
            {
                Id = CreateGuid("Wallet Transfer Destination"),
                Type = true,
                Name = "Wallet Transfer"
            });
            returnList.Add("Wallet Transfer Destination");

            CategoryCodes.AddRange(returnList);
            return returnList;
        }
    }
}
