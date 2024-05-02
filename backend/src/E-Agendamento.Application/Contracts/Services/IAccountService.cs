using E_Agendamento.Application.DTOs.Account;
using E_Agendamento.Application.Wrappers;

namespace E_Agendamento.Application.Contracts.Services
{
    public interface IAccountService
    {
        public Task<Response<string>> AuthenticateAsync(AuthenticationRequest request);
        public Task<Response<string>> RegisterAsync(RegisterRequest request);
        public Task<Response<string>> ForgotPassword();
        public Task<Response<string>> ResetPassword();
    }
}