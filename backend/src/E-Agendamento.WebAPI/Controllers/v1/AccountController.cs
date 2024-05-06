using E_Agendamento.Application.Contracts.Services;
using E_Agendamento.Application.DTOs.Account;
using E_Agendamento.WebAPI.Controllers.Common;
using Microsoft.AspNetCore.Mvc;

namespace E_Agendamento.WebAPI.Controllers.v1
{
    [Route("v1/account/")]
    public class AccountController : BaseController
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("auth/")]
        public async Task<IActionResult> AuthenticateAsync(AuthenticationRequest request)
        {
            return Ok(await _accountService.AuthenticateAsync(request));
        }

        [HttpPost("register/")]
        public async Task<IActionResult> RegisterAsync(RegisterRequest request)
        {
            string origin = Request.Headers.Origin;
            return Ok(await _accountService.RegisterAsync(request, origin));
        }

        [HttpPost("confirm-email/")]
        public async Task<IActionResult> ConfirmEmailAsync([FromQuery] string userId, [FromQuery] string code)
        {
            return Ok(await _accountService.ConfirmEmailAsync(userId, code));
        }
    }
}