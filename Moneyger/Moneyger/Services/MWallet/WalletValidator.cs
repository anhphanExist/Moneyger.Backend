using Moneyger.Common;
using Moneyger.Entities;
using Moneyger.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Moneyger.Services.MWallet
{
    public interface IWalletValidator : IServiceScoped
    {
        Task<bool> Create(Wallet wallet);
        Task<bool> Update(Wallet wallet, string newName);
        Task<bool> Delete(Wallet wallet);
    }
    public class WalletValidator : IWalletValidator
    {
        public enum ErrorCode
        {
            WalletNotExisted,
            WalletDuplicated,
            StringLimited,
            StringEmpty,
            InvalidWalletName
        }
        private IUOW unitOfWork;
        public WalletValidator(IUOW unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<bool> Create(Wallet wallet)
        {
            bool isValid = true;
            isValid &= await ValidateNotExistedWallet(wallet);
            isValid &= ValidateName(wallet);

            return isValid;
        }

        public async Task<bool> Delete(Wallet wallet)
        {
            return await ValidateExistedWallet(wallet);
        }

        public async Task<bool> Update(Wallet wallet, string newName)
        {
            bool isValid = true;

            isValid &= ValidateName(wallet);
            isValid &= await ValidateExistedWallet(wallet);
            isValid &= ValidateNewWalletName(wallet, newName);
            return isValid;
        }

        private async Task<bool> WalletDuplicated(Wallet wallet)
        {
            WalletFilter walletFilter = new WalletFilter
            {
                Take = Int32.MaxValue,
                Id = new GuidFilter { Equal = wallet.Id }
            };
            int count = await unitOfWork.WalletRepository.Count(walletFilter);
            if (count > 0)
            {
                wallet.AddError(nameof(WalletValidator), nameof(Wallet.Name), ErrorCode.WalletDuplicated);
                return false;
            }
            return true;
        }
        private bool ValidateName(Wallet wallet)
        {
            if (wallet.Name.Length <= 0)
            {
                wallet.AddError(nameof(Wallet), nameof(Wallet.Name), ErrorCode.StringEmpty);
                return false;
            }
            else if (wallet.Name.Length > 500)
            {
                wallet.AddError(nameof(Wallet), nameof(Wallet.Name), ErrorCode.StringLimited);
                return false;
            }
            return true;
        }
        private bool ValidateNewWalletName(Wallet wallet, string newWalletName)
        {
            if(newWalletName == null)
            {
                wallet.AddError(nameof(WalletValidator), nameof(newWalletName), ErrorCode.InvalidWalletName);
                return false;
            }
            else if (newWalletName.Length <= 0)
            {
                wallet.AddError(nameof(Wallet), nameof(Wallet.Name), ErrorCode.StringEmpty);
                return false;
            }
            else if(newWalletName.Length > 500)
            {
                wallet.AddError(nameof(Wallet), nameof(Wallet.Name), ErrorCode.StringLimited);
                return false;
            }
            return true;
        }

        private async Task<bool> ValidateExistedWallet(Wallet wallet)
        {
            WalletFilter filter = new WalletFilter
            {
                Id = new GuidFilter { Equal = wallet.Id },
                UserId = new GuidFilter { Equal = wallet.UserId },
                Name = new StringFilter { Equal = wallet.Name}
            };

            int countWallet = await unitOfWork.WalletRepository.Count(filter);
            if(countWallet == 0)
            {
                wallet.AddError(nameof(WalletValidator), nameof(Wallet.Id), ErrorCode.WalletDuplicated);
                return false;
            }
            return true;
        }
        private async Task<bool> ValidateNotExistedWallet(Wallet wallet)
        {
            WalletFilter filter = new WalletFilter
            {
                UserId = new GuidFilter { Equal = wallet.UserId },
                Name = new StringFilter { Equal = wallet.Name }
            };

            int countWallet = await unitOfWork.WalletRepository.Count(filter);
            if (countWallet > 0)
            {
                wallet.AddError(nameof(WalletValidator), nameof(Wallet.Name), ErrorCode.WalletDuplicated);
                return false;
            }
            return true;
        }
    }
}
