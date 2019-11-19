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
            TransactionCodes = new List<string>();
        }

        public List<string> Init(string walletId, string categoryId, int count = 1)
        {
            List<string> returnList = new List<string>();
            string baseCode = "Transaction";

            for (int i = 0; i < count; i++)
            {
                string code = categoryId + "." + walletId + "." + baseCode + i.ToString();
                
                wASContext.Transaction.Add(new TransactionDAO
                {
                    Id = CreateGuid(code),
                    WalletId = CreateGuid(walletId),
                    CategoryId = CreateGuid(categoryId),
                    Amount = i * 10000 + 10000,
                    Date = new DateTime(2019, i % 12 + 1, i % 28 + 1),
                    Note = code,
                });
                returnList.Add(code);
            }
            TransactionCodes.AddRange(returnList);
            return returnList;
        }
    }
}
