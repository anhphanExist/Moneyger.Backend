using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moneyger.Entities;
using Moneyger.Services;

namespace Moneyger.Controllers.Authentication
{
    [Route("api/Moneyger")]
    [Authorize]
    public class AuthenticationController : ControllerBase
    {
        private IUserService userService;
        public AuthenticationController(IUserService userService)
        {
            this.userService = userService;
        }

        [AllowAnonymous]
        [Route("login"), HttpPost]
        public async Task<LoginResponseDTO> Login([FromBody] LoginRequestDTO loginDTO)
        {
            User user = new User()
            {
                Username = loginDTO.Username,
                Password = loginDTO.Password
            };
            User res = await this.userService.Login(user);
            
            return new LoginResponseDTO()
            {
                Username = res.Username,
                Errors = res.Errors
            };
        }

        [Route("change-password"), HttpPost]
        public async Task<ChangePasswordResponseDTO> ChangePassword([FromBody] ChangePasswordRequestDTO changePasswordDTO)
        {
            User user = new User()
            {
                Username = changePasswordDTO.Username,
                Password = changePasswordDTO.Password
            };
            User result = await this.userService.ChangePassword(user, changePasswordDTO.NewPassword);
            if (result.Errors != null)
                return new ChangePasswordResponseDTO
                {
                    Username = result.Username,
                    Success = false,
                    Errors = result.Errors
                };
            else
                return new ChangePasswordResponseDTO
                {
                    Username = result.Username,
                    Success = true
                };
        }
    }
}