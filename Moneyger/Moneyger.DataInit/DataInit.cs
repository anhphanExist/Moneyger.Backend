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
            InitUser();
            InitCategory();
            InitWallet();
            InitTransaction();
            wASContext.SaveChanges();
            return true;
        }

        private void InitTransaction()
        {
            throw new NotImplementedException();
        }

        private void InitWallet()
        {
            throw new NotImplementedException();
        }

        private void InitCategory()
        {
            throw new NotImplementedException();
        }

        private void InitUser()
        {
            
        }

        public void Clean()
        {

        }
    }
}
