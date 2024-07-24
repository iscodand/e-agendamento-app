using E_Agendamento.Application.Contracts.Services;
using E_Agendamento.Application.DTOs.Account;
using E_Agendamento.WebAPI.Controllers.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Agendamento.WebAPI.Controllers.v1
{
    [Route("api/v1/account/")]
    public class AccountController : BaseController
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("auth/")]
        public async Task<IActionResult> AuthenticateAsync([FromBody] AuthenticationRequest request)
        {
            return Ok(await _accountService.AuthenticateAsync(request));
        }

        [HttpPost("register/")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterRequest request)
        {
            string origin = Request.Headers.Origin;
            return Ok(await _accountService.RegisterAsync(request, origin));
        }

        [Authorize]
        [HttpGet("verify-token/")]
        public IActionResult VerifyToken()
        {
            return Ok("Teste");
        }

        [Authorize]
        [HttpGet("me/")]
        public async Task<IActionResult> GetAuthenticatedUserAsync()
        {
            return Ok(await _accountService.GetAuthenticatedUserAsync(AuthenticatedUser.UserId));
        }

        [HttpPost("confirm-email/")]
        public async Task<IActionResult> ConfirmEmailAsync([FromQuery] string userId, [FromQuery] string code)
        {
            return Ok(await _accountService.ConfirmEmailAsync(userId, code));
        }

        [HttpPost("forgot-password/")]
        public async Task<IActionResult> ForgotPasswordAsync([FromBody] ForgotPasswordRequest request)
        {
            string origin = Request.Headers.Origin;
            await _accountService.ForgotPasswordAsync(request, origin);
            return Ok();
        }

        [HttpPost("reset-password/")]
        public async Task<IActionResult> ResetPasswordAsync([FromBody] ResetPasswordRequest request)
        {
            return Ok(await _accountService.ResetPasswordAsync(request));
        }
    }
}