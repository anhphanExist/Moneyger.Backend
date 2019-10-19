using Moneyger.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Moneyger.DataInit
{
    public class WalletInit : CommonInit
    {
        public List<string> WalletCodes { get; private set; }
        public WalletInit(WASContext wASContext) : base(wASContext)
        {

        }

        public List<string> Init(string userId, int count = 1)
        {
            List<string> returnList = new List<string>();
            string baseCode = "Wallet";

            for (int i = 0; i < count; i++)
            {
                string code = baseCode + i.ToString();


                wASContext.Wallet.Add(new WalletDAO
                {
                    Id = CreateGuid(code),
                    Balance = i * 1000000,
                    Name = code,
                    UserId = CreateGuid(userId)
                });
                returnList.Add(code);
            }

            WalletCodes = returnList;
            return returnList;
        }
    }
}
