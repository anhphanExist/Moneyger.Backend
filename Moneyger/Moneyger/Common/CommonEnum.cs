using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Moneyger.Common
{
    public static class CommonEnum
    {
        public enum ErrorCode
        {
            SystemError
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
