using Moneyger.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Moneyger.Repositories
{
    public interface IUOW
    {
        Task Begin();
        Task Commit();
        Task Rollback();
        IUserRepository UserRepository { get; }
    }
    public class UOW : IUOW
    {
        private WASContext wASContext;
        public IUserRepository UserRepository { get; }

        public UOW(WASContext wASContext)
        {
            this.wASContext = wASContext;
            UserRepository = new UserRepository(this.wASContext);
        }

        public Task Begin()
        {
            throw new NotImplementedException();
        }

        public Task Commit()
        {
            throw new NotImplementedException();
        }

        public Task Rollback()
        {
            throw new NotImplementedException();
        }
    }
}
