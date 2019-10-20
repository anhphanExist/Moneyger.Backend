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

        public List<string> Init(string walletId, string categoryId, int count = 1)
        {
            List<string> returnList = new List<string>();
            string baseCode = "Transaction";

            for (int i = 0; i < count; i++)
            {
                string code = baseCode + walletId + categoryId + i.ToString();
                
                wASContext.Transaction.Add(new TransactionDAO
                {
                    Id = CreateGuid(code),
                    WalletId = CreateGuid(walletId),
                    CategoryId = CreateGuid(categoryId),
                    Amount = i * 1000,
                    Date = new DateTime(2000, i % 12, i % 30),
                    Note = code,
                });
                returnList.Add(code);
            }
            TransactionCodes = returnList;
            return returnList;
        }
    }
}
