using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Moneyger.Controllers.Register
{
    [Route("api/Register")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        [Route("Signup"), HttpPost]
        public async Task Signup()
        {

        }
    }
}