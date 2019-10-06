using Moneyger.Common;
using Moneyger.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Moneyger.Repositories
{
    public interface IUOW : IServiceScoped
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

        public async Task Begin()
        {
            await wASContext.Database.BeginTransactionAsync();
        }

        public Task Commit()
        {
            wASContext.Database.CommitTransaction();
            return Task.CompletedTask;
        }

        public Task Rollback()
        {
            wASContext.Database.RollbackTransaction();
            return Task.CompletedTask;
        }
    }
}
