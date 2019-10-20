using Microsoft.EntityFrameworkCore;
using Moneyger.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Moneyger.DataInit
{
    public class DataInit
    {
        public WASContext wASContext;
        public UserInit userInit;
        public WalletInit walletInit;
        public CategoryInit categoryInit; 
        public TransactionInit transactionInit;

        public DataInit(WASContext wASContext)
        {
            this.wASContext = wASContext;
            this.userInit = new UserInit(this.wASContext);
            this.walletInit = new WalletInit(this.wASContext);
            this.categoryInit = new CategoryInit(this.wASContext);
            this.transactionInit = new TransactionInit(this.wASContext);
        }

        public bool Init()
        {
            Clean();
            //InitUser();
            //InitCategory();
            //InitWallet();
            //InitTransaction();
            wASContext.SaveChanges();
            return true;
        }

        private void InitTransaction()
        {
            for (int i = 0; i < walletInit.WalletCodes.Count; i++)
            {
                for (int j = 0; j < categoryInit.CategoryCodes.Count; j++)
                {
                    transactionInit.Init(walletInit.WalletCodes[i], categoryInit.CategoryCodes[j], 1);
                }
            }
            
        }

        private void InitWallet()
        {
            for (int i = 0; i < userInit.UserCodes.Count; i++)
            {
                walletInit.Init(userInit.UserCodes[i], 3);
            }
        }

        private void InitCategory()
        {
            categoryInit.Init(10);
        }

        private void InitUser()
        {
            userInit.Init(2);
        }

        public void Clean()
        { 
            string command = string.Format(
              "TRUNCATE" 
                    + "\u0022" + "Transaction" + "\u0022" + ", "
                    +"\u0022" + "Wallet" + "\u0022" + ", "
                    +"\u0022" + "Category" + "\u0022" + ", "
                    +"\u0022" + "User" + "\u0022" + " "
                + "RESTART IDENTITY;");
            var result = wASContext.Database.ExecuteSqlCommand(command);
        }
    }
}
