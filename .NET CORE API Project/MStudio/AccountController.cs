using MediaStudio.Core;
using MediaStudioService.AccountService;
using MediaStudioService.Core;
using MediaStudioService.Core.Classes;
using MediaStudioService.Models.Input;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MediaStudio.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        AccountService accountService;

        public AccountController(AccountService accountService)
        {
            this.accountService = accountService;
        }

		// Вход в систему, получение токена
        [HttpPost("SignIn")]
        public ActionResult<Responce> SignIn(InputAccount inputAccount)
        {
            return new ObjectResult(accountService.GetToken(inputAccount));
        }

		// Авторизация пользователя
        [Route("SignUp")]
        public ActionResult<Responce> SignUp(InputAccount inputAccount)
        {
            //по умолчанию ставим роль "Пользователь"
            inputAccount.Role = Roles.Пользователь;
            return new ObjectResult(accountService.TryCreateAccountAsync(inputAccount).Result);
        }

         //метод доступный только для лиц, имеющих привелегию "SignUpWithRole"
        [Authorize(Policy = Policy.SignUpWithRole)]
        [Route("SignUpWithRole")]
        public ActionResult<Responce> SignUpWithRole(InputAccount inputAccount) 
        {
            return new ObjectResult(accountService.TryCreateAccountAsync(inputAccount).Result);
        }
    }
}