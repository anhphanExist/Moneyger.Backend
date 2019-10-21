using Moneyger.Common;
using Moneyger.Entities;
using Moneyger.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Moneyger.Services.MWallet
{
    public interface IWalletService : IServiceScoped
    {
        Task<Wallet> Get(Guid Id);
        Task<Wallet> Get(WalletFilter filter);
        Task<Wallet> Create(Wallet wallet);
        Task<Wallet> UpdateWalletName(Wallet wallet, string newName);
        Task<Wallet> Delete(Wallet wallet);
        Task<int> Count(WalletFilter filter);
        Task<List<Wallet>> List(WalletFilter filter);
    }
    public class WalletService : IWalletService
    {
        private IUOW UnitOfWork;
        private IWalletValidator WalletValidator;
        public WalletService(IUOW UnitOfWork, IWalletValidator WalletValidator)
        {
            this.UnitOfWork = UnitOfWork;
            this.WalletValidator = WalletValidator;
        }
        public async Task<int> Count(WalletFilter filter)
        {
            if (filter == null) filter = new WalletFilter { };
            return await UnitOfWork.WalletRepository.Count(filter);
        }

        public async Task<Wallet> Create(Wallet wallet)
        {
            wallet.Id = Guid.NewGuid();

            if (wallet == null) return null;
            if (!await WalletValidator.Create(wallet))
                return wallet;

            using (UnitOfWork.Begin())
            {
                try
                {
                    await UnitOfWork.WalletRepository.Create(wallet);
                    await UnitOfWork.Commit();
                }
                catch (Exception ex)
                {
                    await UnitOfWork.Rollback();
                    wallet.AddError(nameof(WalletService), nameof(Wallet.Id), CommonEnum.ErrorCode.SystemError);
                }
            }
            return wallet;
        }

        public async Task<Wallet> Delete(Wallet wallet)
        {
            if (!await WalletValidator.Delete(wallet))
                return wallet;

            using (UnitOfWork.Begin())
            {
                try
                {
                    var walletB = await UnitOfWork.WalletRepository.Get(wallet.Id);
                    await UnitOfWork.WalletRepository.Delete(wallet);
                    await UnitOfWork.Commit();
                }
                catch (Exception ex)
                {
                    await UnitOfWork.Rollback();
                    wallet.AddError(nameof(WalletService), nameof(Wallet.Id), CommonEnum.ErrorCode.SystemError);
                }
            }
            return wallet;
        }

        public async Task<Wallet> Get(WalletFilter filter)
        {
            return await UnitOfWork.WalletRepository.Get(filter);
        }
        public async Task<Wallet> Get(Guid Id)
        {
            return await UnitOfWork.WalletRepository.Get(Id);
        }

        public async Task<List<Wallet>> List(WalletFilter filter)
        {
            if (filter == null) filter = new WalletFilter { };
            return await UnitOfWork.WalletRepository.List(filter);
        }

        public async Task<Wallet> UpdateWalletName(Wallet wallet, string newName)
        {
            if (wallet == null) return null;
            if (!await WalletValidator.Update(wallet, newName))
                return wallet;

            using (UnitOfWork.Begin())
            {
                try
                {
                    WalletFilter filter = new WalletFilter
                    {
                        Id = new GuidFilter { Equal = wallet.Id },
                        UserId = new GuidFilter { Equal = wallet.UserId },
                        Name = new StringFilter { Equal = wallet.Name },
                        Balance = new DecimalFilter { Equal = wallet.Balance }

                    };
                    wallet = await Get(filter);
                    wallet.Name = newName;

                    await UnitOfWork.WalletRepository.Update(wallet);
                    await UnitOfWork.Commit();
                    return await Get(new WalletFilter
                    {
                        Id = new GuidFilter { Equal = wallet.Id },
                        UserId = new GuidFilter { Equal = wallet.UserId },
                        Name = new StringFilter { Equal = wallet.Name },
                        Balance = new DecimalFilter { Equal = wallet.Balance }
                    });
                }
                catch (Exception ex)
                {
                    await UnitOfWork.Rollback();
                    wallet.AddError(nameof(WalletService), nameof(Wallet.Id), CommonEnum.ErrorCode.SystemError);
                }
            }
            return wallet;
        }
    }
}
