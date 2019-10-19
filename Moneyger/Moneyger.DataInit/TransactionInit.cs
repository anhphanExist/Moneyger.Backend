using Moneyger.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Moneyger.DataInit
{
    public class TransactionInit : CommonInit
    {
        public List<string> TransactionCodes { get; private set; }
        public TransactionInit(WASContext wASContext) : base(wASContext)
        {

        }

        public List<string> Init(List<string> walletIds, List<string> categoryIds, int count = 1)
        {
            List<string> returnList = new List<string>();
            string baseCode = "Transaction";

            for (int i = 0; i < count; i++)
            {
                string code = baseCode + i.ToString();
                for (int j = 0; j < walletIds.Count; j++)
                {
                    for (int k = 0; k < categoryIds.Count; k++)
                    {
                        wASContext.Transaction.Add(new TransactionDAO
                        {
                            Id = CreateGuid(code),
                            WalletId = CreateGuid(walletIds[j]),
                            CategoryId = CreateGuid(categoryIds[k]),
                            Amount = i * 1000,
                            Date = new DateTime(2000, i % 12, i % 30),
                            Note = code,
                        });
                        returnList.Add(code);
                    }
                }
                
            }

            TransactionCodes = returnList;
            return returnList;
        }
    }
}
