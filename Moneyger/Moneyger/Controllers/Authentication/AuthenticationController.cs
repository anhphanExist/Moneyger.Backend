using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moneyger.Entities;
using Moneyger.Services;

namespace Moneyger.Controllers.Authentication
{
    [Route("api/authentication")]
    public class AuthenticationController : ControllerBase
    {
        private IUserService userService;
        public AuthenticationController(IUserService userService)
        {
            this.userService = userService;
        }
        [Route("login"), HttpPost]
        public async Task<LoginResultDTO> Login([FromBody] LoginDTO loginDTO)
        {
            User user = new User()
            {
                Username = loginDTO.Username,
                Password = loginDTO.Password
            };
            User res = await this.userService.Login(user);
            
            return new LoginResultDTO()
            {
                Username = res.Username,
                Errors = res.Errors
            };
        }

        [Route("change-password"), HttpPost]
        public async Task<ChangePasswordResultDTO> ChangePassword([FromBody] ChangePasswordDTO changePasswordDTO)
        {
            User user = new User()
            {
                Username = changePasswordDTO.Username,
                Password = changePasswordDTO.Password
            };
            User result = await this.userService.ChangePassword(user, changePasswordDTO.NewPassword);
            if (result.Errors.Count > 0)
                return new ChangePasswordResultDTO
                {
                    Username = result.Username,
                    Success = false,
                    Errors = result.Errors
                };
            else
                return new ChangePasswordResultDTO
                {
                    Username = result.Username,
                    Success = true
                };
        }
    }
}