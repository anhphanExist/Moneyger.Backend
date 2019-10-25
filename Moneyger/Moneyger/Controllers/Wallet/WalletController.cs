using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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

    [Authorize]
    public class WalletController : ControllerBase
    {
        public WalletController()
        {

        }

        [Route(WalletRoute.List), HttpPost]
        public async Task<List<WalletDTO>> List([FromBody] WalletListRequestDTO walletListRequestDTO)
        {
            throw new NotImplementedException();
        }

        [Route(WalletRoute.Create), HttpPost]
        public async Task<WalletDTO> Create([FromBody] WalletDTO walletRequestDTO)
        {
            throw new NotImplementedException();
        }

        [Route(WalletRoute.Update), HttpPost]
        public async Task<WalletDTO> Update([FromBody] WalletDTO walletRequestDTO) 
        {
            throw new NotImplementedException();
        }

        [Route(WalletRoute.Delete), HttpPost]
        public async Task<WalletDTO> Delete([FromBody] WalletDTO walletRequestDTO) 
        {
            throw new NotImplementedException();
        }

        [Route(WalletRoute.Transfer), HttpPost]
        public async Task<bool> Transfer([FromBody] WalletTransferRequestDTO walletTransferRequestDTO)
        {
            throw new NotImplementedException();
        }
    }
}