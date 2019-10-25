using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Moneyger.Controllers
{
    [Authorize]
    public class ApiController : ControllerBase
    {
        public Guid currentUserId
        {
            get
            {
                return Guid.TryParse(User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value, out Guid u) ? u : Guid.Empty;
            }
        }
    }
}