using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moneyger.Common;
using Moneyger.Entities;
using Moneyger.Services.MWallet;

namespace Moneyger.Controllers.Wallet
{
    public class WalletRoute : Root
    {
        public const string Default = Base + "/wallet";
        public const string List = Default + "/list";
        public const string Create = Default + "/create";
        public const string Update = Default + "/update";
        public const string Delete = Default + "/delete";
        public const string Transfer = Default + "/transfer";
    }

    
    public class WalletController : ApiController
    {
        private IWalletService walletService;
        public WalletController(IWalletService walletService)
        {
            this.walletService = walletService;
        }

        [Route(WalletRoute.List), HttpPost]
        public async Task<List<WalletDTO>> List()
        {
            WalletFilter walletFilter = new WalletFilter()
            {
                UserId = new GuidFilter { Equal = currentUserId },
                
            };
            List<Entities.Wallet> wallets = await walletService.List(walletFilter);
            List<WalletDTO> walletDTOs = new List<WalletDTO>();
            wallets.ForEach(w => walletDTOs.Add(new WalletDTO
            {
                UserId = w.UserId,
                Name = w.Name,
                Balance = w.Balance,
                Errors = w.Errors
            }));
            return walletDTOs;
        }

        [Route(WalletRoute.Create), HttpPost]
        public async Task<WalletDTO> Create([FromBody] WalletDTO walletRequestDTO)
        {
            Entities.Wallet requestWallet = new Entities.Wallet
            {
                UserId = currentUserId,
                Balance = walletRequestDTO.Balance,
                Name = walletRequestDTO.Name
            };
            Entities.Wallet resultWallet = await walletService.Create(requestWallet);
            return new WalletDTO
            {
                Name = resultWallet.Name,
                Balance = resultWallet.Balance,
                UserId = resultWallet.UserId,
                Errors = resultWallet.Errors,
            };

        }

        [Route(WalletRoute.Update), HttpPost]
        public async Task<WalletDTO> Update([FromBody] WalletUpdateRequestDTO walletUpdateRequestDTO) 
        {
            Entities.Wallet requestWallet = new Entities.Wallet
            {
                UserId = currentUserId,
                Balance = walletUpdateRequestDTO.Balance,
                Name = walletUpdateRequestDTO.Name
            };
            Entities.Wallet resultWallet = await walletService.UpdateWalletName(requestWallet, walletUpdateRequestDTO.NewName);
            return new WalletDTO
            {
                Name = resultWallet.Name,
                Balance = resultWallet.Balance,
                UserId = resultWallet.UserId,
                Errors = resultWallet.Errors,
            };
        }

        [Route(WalletRoute.Delete), HttpPost]
        public async Task<WalletDTO> Delete([FromBody] WalletDTO walletRequestDTO) 
        {
            Entities.Wallet requestWallet = new Entities.Wallet
            {
                UserId = currentUserId,
                Name = walletRequestDTO.Name
            };
            Entities.Wallet resultWallet = await walletService.Delete(requestWallet);
            return new WalletDTO
            {
                Name = resultWallet.Name,
                Balance = resultWallet.Balance,
                UserId = resultWallet.UserId,
                Errors = resultWallet.Errors
            };
        }

        [Route(WalletRoute.Transfer), HttpPost]
        public async Task<WalletTransferResponseDTO> Transfer([FromBody] WalletTransferRequestDTO walletTransferRequestDTO)
        {
            // Tao 2 wallet BO de gui cho Service xu ly Transfer
            Entities.Wallet sourceWallet = new Entities.Wallet
            {
                Name = walletTransferRequestDTO.SourceWalletName,
                UserId = currentUserId
            };
            Entities.Wallet destWallet = new Entities.Wallet
            {
                Name = walletTransferRequestDTO.DestWalletName,
                UserId = currentUserId
            };
            Tuple<Entities.Wallet, Entities.Wallet> res = await walletService.Transfer(
                Tuple.Create(sourceWallet, destWallet), 
                walletTransferRequestDTO.Amount, 
                walletTransferRequestDTO.Note);

            // Tao response object
            WalletTransferResponseDTO resDTO = new WalletTransferResponseDTO
            {
                SourceWalletName = res.Item1.Name,
                DestWalletName = res.Item2.Name,
                UserId = res.Item1.UserId,
                Errors = new List<string>()
            };
            if (res.Item1.Errors != null)
                resDTO.Errors.AddRange(res.Item1.Errors);
            if (res.Item2.Errors != null)
                resDTO.Errors.AddRange(res.Item2.Errors);
            return resDTO;
        }
    }
}