using Moneyger.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Moneyger.DataInit
{
    public class CommonInit
    {
        protected WASContext wASContext;
        public CommonInit(WASContext wASContext)
        {
            this.wASContext = wASContext;
        }
        public static Guid CreateGuid(string name)
        {
            MD5 md5 = MD5.Create();
            Byte[] myStringBytes = ASCIIEncoding.Default.GetBytes(name);
            Byte[] hash = md5.ComputeHash(myStringBytes);
            return new Guid(hash);
        }
    }
}
