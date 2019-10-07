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
            UserFilter userFilter = new UserFilter()
            {
                Username = loginDTO.Username,
                Password = loginDTO.Password
            };
            User user = await this.userService.Login(userFilter);
            if (user != null)
            {
                return new LoginResultDTO()
                {
                    Username = user.Username
                };
            }
            return null;
        }

        [Route("change-password"), HttpPost]
        public async Task<bool> ChangePassword([FromBody] ChangePasswordDTO changePasswordDTO)
        {
            UserFilter userFilter = new UserFilter()
            {
                Username = changePasswordDTO.Username,
                Password = changePasswordDTO.Password
            };
            User user = await this.userService.ChangePassword(userFilter, changePasswordDTO.NewPassword);
            return user != null;
        }
    }
}