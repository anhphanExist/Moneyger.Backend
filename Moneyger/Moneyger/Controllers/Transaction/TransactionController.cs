using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Moneyger.Controllers.Transaction
{
    public class TransactionRoute : Root
    {
        public const string Default = Base + "/transaction";
        public const string Create = Default + "/create";
        public const string ListTransactionDayGroup = Default + "/list-transaction-day-group";
        public const string GetTransactionMonthGroup = Default + "/get-transaction-month-group";
    }

    [Authorize]
    public class TransactionController : ControllerBase
    {
        public TransactionController()
        {

        }

        [Route(TransactionRoute.Create), HttpPost]
        public async Task Create([FromBody] TransactionDTO transactionDTO)
        {

        }

        [Route(TransactionRoute.ListTransactionDayGroup), HttpPost]
        public async Task<List<TransactionDayGroupResponseDTO>> ListTransactionDayGroup([FromBody] TransactionDayGroupRequestDTO transactionDayGroupRequestDTO)
        {
            throw new NotImplementedException();
        }

        [Route(TransactionRoute.GetTransactionMonthGroup), HttpPost]
        public async Task<TransactionMonthGroupResponseDTO> GetTransactionMonthGroup([FromBody] TransactionMonthGroupRequestDTO transactionMonthGroupRequestDTO)
        {
            throw new NotImplementedException();
        }
    }
}