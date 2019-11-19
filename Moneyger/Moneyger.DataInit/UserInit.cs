using Moneyger.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Moneyger.DataInit
{
    public class UserInit : CommonInit
    {
        public List<string> UserCodes { get; private set; }
        public UserInit(WASContext wASContext) : base(wASContext)
        {
            UserCodes = new List<string>();
        }

        public List<string> Init(int count = 1)
        {
            List<string> returnList = new List<string>();
            string baseCode = "User";

            for (int i = 0; i < count; i++)
            {
                string code = baseCode + i.ToString();


                wASContext.User.Add(new UserDAO
                {
                    Id = CreateGuid(code),
                    Username = code,
                    Password = "123456"
                });
                returnList.Add(code);
            }

            UserCodes.AddRange(returnList);
            return returnList;
        }
    }
}
