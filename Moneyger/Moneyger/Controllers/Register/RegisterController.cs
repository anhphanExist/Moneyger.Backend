using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moneyger.Entities;
using Moneyger.Services;

namespace Moneyger.Controllers.Register
{
    [Route("api/register")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        private IUserService userService;
        public RegisterController(IUserService userService)
        {
            this.userService = userService;
        }
        [Route("Signup"), HttpPost]
        public async Task<RegisterResultDTO> Signup(RegisterDTO registerUserDTO)
        {
            User user = new User
            {
                Username = registerUserDTO.Username,
                Password = registerUserDTO.Password
            };
            User res = await userService.Create(user);
            return new RegisterResultDTO
            {
                Username = res.Username,
                Errors = res.Errors
            };
        }
    }
}