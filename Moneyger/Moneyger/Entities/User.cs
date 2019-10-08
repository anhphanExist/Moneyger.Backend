using Moneyger.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Moneyger.Entities
{
    public class User : DataEntity
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class UserFilter : FilterEntity
    {
        public StringFilter Username { get; set; }
        public StringFilter Password { get; set; }
        public UserOrder OrderBy { get; set; }
        public UserSelect Selects { get; set; }
        public UserFilter() : base()
        {

        }
    }
    public enum UserOrder
    {
        Id,
        CX,
        Username
    }

    [Flags]
    public enum UserSelect
    {
        Id = 1,
        CX = 2,
        Username = 4
    }
}
