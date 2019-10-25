using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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

        [AllowAnonymous]
        [Route("signup"), HttpPost]
        public async Task<RegisterResponseDTO> Signup(RegisterRequestDTO registerUserDTO)
        {
            User user = new User
            {
                Username = registerUserDTO.Username,
                Password = registerUserDTO.Password
            };
            User res = await userService.Create(user);
            return new RegisterResponseDTO
            {
                Username = res.Username,
                Errors = res.Errors
            };
        }

        [Route("update"), HttpPost]
        public async Task<RegisterResponseDTO> Update(RegisterUpdateRequestDTO registerUpdateRequestDTO)
        {
            throw new NotImplementedException();
        }
    }
}